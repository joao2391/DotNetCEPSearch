using DotNet.CEP.Search.App.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;
using HtmlAgilityPack;

namespace DotNet.CEP.Search.App
{
    /// <summary>
    /// Cep Search
    /// </summary>
    public class CepSearch : BaseCepSearch, ICepSearch
    {

        /// <summary>
        /// Returns the address
        /// </summary>
        /// <param name="cep">CEP number without '-'</param>
        /// <param name="cancellationToken">Token to cancel task</param>
        /// <returns>JSON with address</returns>
        public async Task<ResponseAddress> GetAddressByCepAsync(string cep, CancellationToken cancellationToken = default)
        {
            try
            {
                var address = await GetAddressFromCorreiosByCep(cep, cancellationToken).ConfigureAwait(false);

                return address;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (JsonSerializationException)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the address
        /// </summary>
        /// <param name="cep">CEP number without '-'</param>
        /// <returns>JSON with address</returns>
        public ResponseAddress GetAddressByCep(string cep)
        {
            try
            {

                var address = GetAddressFromCorreiosByCep(cep, new CancellationToken()).Result;

                return address;

            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (HtmlWebException)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the CEP number
        /// </summary>
        /// <param name="address">Full or partial address</param>
        /// <param name="cancellationToken">Token to cancel task</param>
        /// <returns>JSON with CEP number</returns>
        public async Task<ResponseCep> GetCepByAddressAsync(string address, CancellationToken cancellationToken = default)
        {
            try
            {
                var cep = await GetCepFromCorreiosByAddress(address, cancellationToken).ConfigureAwait(false);

                return cep;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (JsonSerializationException)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the CEP number
        /// </summary>
        /// <param name="address">Full or partial address</param>
        /// <returns>JSON with CEP number</returns>
        public ResponseCep GetCepByAddress(string address)
        {
            try
            {
                var cep = GetCepFromCorreiosByAddress(address, new CancellationToken()).Result;

                return cep;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (JsonSerializationException)
            {
                throw;
            }
        }

        private async Task<ResponseAddress> GetAddressFromCorreiosByCep(string cep, CancellationToken cancellationToken)
        {
            var dict = new Dictionary<string, string>
                {
                    {"CEP",cep}
                };

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, UrlCorreio)
            {
                Content = new FormUrlEncodedContent(dict),
            };

            var httpResponse = await _client.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);

            var html = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(html);

            var name = htmlDoc.DocumentNode.SelectNodes("//td");

            if (name == null)
            {
                //TODO - Check
                throw new NodeNotFoundException();
            }

            var responseAddress = BuildResponseAddress(name);

            return responseAddress;
        }

        private async Task<ResponseCep> GetCepFromCorreiosByAddress(string address, CancellationToken cancellationToken)
        {
            var dict = new Dictionary<string, string>
                {
                    {"relaxation", address},
                    {"tipoCEP", "ALL"},
                    {"semelhante", "N"}
                };

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, UrlCorreio)
            {
                Content = new FormUrlEncodedContent(dict),
            };

            var httpResponse = await _client.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);

            var html = await httpResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            var hsRespEndereco = new HashSet<ResponseCep>();

            var hasNextPage = false;

            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(html);

            var link = htmlDoc.DocumentNode.SelectNodes("//div//a");

            if (link is null)
            {
                //TODO
            }

            for (int i = 0; i < link.Count; i++)
            {
                if (link[i].InnerHtml.Contains("[ Próxima ]"))
                {
                    hasNextPage = true;
                }
            }

            var name = htmlDoc.DocumentNode.SelectNodes("//td");
            var divCtrlContent = htmlDoc.DocumentNode.SelectNodes("//div[@class='ctrlcontent']");
            var numPages = divCtrlContent[0].OuterHtml.Substring(1068, 13);

            if (name == null)
            {
                //TODO - check
                throw new NodeNotFoundException();
            }

            for (int i = 0; i < name.Count; i++)
            {
                if (i % 4 == 0)
                {
                    var reponseEndereco = BuildResponseCep(name);

                    hsRespEndereco.Add(reponseEndereco);
                }
            }

            if (hasNextPage)
            {
                AcessaProximasPaginas(hsRespEndereco, address, numPages);
            }

            return new ResponseCep();
        }

        private void AcessaProximasPaginas(HashSet<ResponseCep> hsRespEnd, string endereco,
                                            string numPages, int pageIni = 51, int pageFim = 100)
        {
            bool hasNextPage = false;
            var dict = new Dictionary<string, string>
            {
                {"relaxation", endereco},
                {"tipoCEP", "ALL"},
                {"semelhante", "N"},
                {"qtdrow", "50"},
                {"pagIni", pageIni.ToString()},
                {"pagFim", pageFim.ToString()}
            };

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, UrlCorreio)
            {
                Content = new FormUrlEncodedContent(dict)
            };

            var httpResponse = _client.SendAsync(httpRequest).Result.Content.ReadAsStringAsync();

            var html = httpResponse.Result;

            var docHtml = new HtmlDocument();
            docHtml.LoadHtml(html);

            var name = docHtml.DocumentNode.SelectNodes("//td");

            if (!hasNextPage)
            {
                var link = docHtml.DocumentNode.SelectNodes("//div//a");

                for (int i = 0; i < link.Count; i++)
                {
                    if (link[i].InnerHtml.Contains("[ Próxima ]"))
                    {
                        hasNextPage = true;
                        pageIni += 25;
                        pageFim += 25;
                    }
                }
            }

            for (int i = 0; i < name.Count; i++)
            {
                if (i % 4 == 0)
                {
                    var reponseEndereco = BuildResponseCep(name);

                    hsRespEnd.Add(reponseEndereco);
                }
            }

            if (hasNextPage)
            {
                AcessaProximasPaginas(hsRespEnd, endereco, numPages, pageIni, pageFim);
            }


        }

        private ResponseAddress BuildResponseAddress(HtmlNodeCollection nodes) //where T: new()
        {

            var cidade = nodes[2].InnerText.Replace("&nbsp;", string.Empty).Split("/")[0];
            var uf = nodes[2].InnerText.Replace("&nbsp;", string.Empty).Split("/")[1];

            var response = new ResponseAddress
            {
                Bairro = nodes[1].InnerText.Replace("&nbsp;", string.Empty),
                Cep = nodes[3].InnerText.Replace("&nbsp;", string.Empty),
                Cidade = cidade,
                Rua = nodes[0].InnerText.Replace("&nbsp;", string.Empty),
                Uf = uf
            };

            return response;
        }

        private ResponseCep BuildResponseCep(HtmlNodeCollection nodes)
        {
            var cidade = nodes[2].InnerText.Replace("&nbsp;", string.Empty).Split("/")[0];
            var uf = nodes[2].InnerText.Replace("&nbsp;", string.Empty).Split("/")[1];

            var response = new ResponseCep
            {
                Bairro = nodes[1].InnerText.Replace("&nbsp;", string.Empty),
                Cep = nodes[3].InnerText.Replace("&nbsp;", string.Empty),
                Cidade = cidade,
                Rua = nodes[0].InnerText.Replace("&nbsp;", string.Empty),
                Uf = uf
            };

            return response;
        }

    }
}
