using DotNet.CEP.Search.App.Models;
using DotNet.CEP.Search.App.Utils;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotNet.CEP.Search.App
{
    /// <summary>
    /// Cep Search
    /// </summary>
    public class CepSearch : BaseCepSearch, ICepSearch
    {
        private readonly Regex rx = new Regex(@"[0-9]{5}[\d]{3}");

        /// <summary>
        /// Returns the address
        /// </summary>
        /// <param name="cep">CEP number without '-'</param>
        /// <returns>JSON with address</returns>
        public async Task<string> GetAddressByCepAsync(string cep)
        {
            if (!rx.IsMatch(cep))
            {
                return Constants.INVALID_CEP;
            }

            try
            {
                var dict = new Dictionary<string, string>
                {
                    {"relaxation",cep},
                    {"tipoCEP","ALL" },
                    {"semelhante","N" }
                };
               

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, UrlCorreio)
                {
                    Content = new FormUrlEncodedContent(dict)
                };

                var httpResponse = await _client.SendAsync(httpRequest).Result.Content.ReadAsStringAsync();
                var html = httpResponse;

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var name = htmlDoc.DocumentNode.SelectNodes("//td");
                if (name == null)
                {
                    return Constants.NOT_FOUND_MESSAGE;
                }

                string bairro = name[1].InnerText.Replace("&nbsp;", string.Empty);
                string numeroCep = name[3].InnerText.Replace("&nbsp;", string.Empty);
                string cidade = name[2].InnerText.Replace("&nbsp;", string.Empty);
                string rua = name[0].InnerText.Replace("&nbsp;", string.Empty);

                var responseCep = new ResponseCep
                {
                    Bairro = bairro,
                    Cep = numeroCep,
                    Cidade = cidade,
                    Rua = rua
                };

                return JsonConvert.SerializeObject(responseCep);
            }
            catch (HttpRequestException httpEx)
            {
                throw httpEx;
            }
            catch (HtmlWebException htmlEx)
            {
                throw htmlEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Returns the address
        /// </summary>
        /// <param name="cep">CEP number without '-'</param>
        /// <returns>JSON with address</returns>
        public string GetAddressByCep(string cep)
        {
            if (!rx.IsMatch(cep))
            {
                return string.Empty;
            }

            try
            {
                var dict = new Dictionary<string, string>
                {
                    {"relaxation",cep},
                    {"tipoCEP","ALL" },
                    {"semelhante","N" }
                };

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, UrlCorreio)
                {
                    Content = new FormUrlEncodedContent(dict)
                };

                var httpResponse = _client.SendAsync(httpRequest).Result.Content.ReadAsStringAsync().Result;
                var html = httpResponse;

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var name = htmlDoc.DocumentNode.SelectNodes("//td");
                if (name == null)
                {
                    return Constants.NOT_FOUND_MESSAGE;
                }

                string bairro = name[1].InnerText.Replace("&nbsp;", string.Empty);
                string numeroCep = name[3].InnerText.Replace("&nbsp;", string.Empty);
                string cidade = name[2].InnerText.Replace("&nbsp;", string.Empty);
                string rua = name[0].InnerText.Replace("&nbsp;", string.Empty);

                var responseCep = new ResponseCep
                {
                    Bairro = bairro,
                    Cep = numeroCep,
                    Cidade = cidade,
                    Rua = rua
                };

                return JsonConvert.SerializeObject(responseCep);
            }
            catch (HttpRequestException httpEx)
            {
                throw httpEx;
            }
            catch (HtmlWebException htmlEx)
            {
                throw htmlEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Returns the CEP number
        /// </summary>
        /// <param name="address">Full or partial address</param>
        /// <returns>JSON with CEP number</returns>
        public async Task<string> GetCepByAddressAsync(string address)
        {
            try
            {
                var lstEnderecos = new List<ResponseEndereco>();
                bool hasNextPage = false;
                var hsRespEndereco = new HashSet<ResponseEndereco>();
                var dict = new Dictionary<string, string>
                {
                    {"relaxation",address},
                    {"tipoCEP","ALL" },
                    {"semelhante","N" }
                };

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, UrlCorreio)
                {
                    Content = new FormUrlEncodedContent(dict)
                };

                var httpResponse = await _client.SendAsync(httpRequest).Result.Content.ReadAsStringAsync();
                var html = httpResponse;

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var link = htmlDoc.DocumentNode.SelectNodes("//div//a");

                if (link == null)
                {
                    return Constants.NOT_FOUND_MESSAGE;
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
                    return Constants.NOT_FOUND_MESSAGE;
                }

                for (int i = 0; i < name.Count; i++)
                {
                    if (i % 4 == 0)
                    {
                        var reponseEndereco = new ResponseEndereco
                        {
                            Bairro = name[i + 1].InnerText.Replace("&nbsp;", string.Empty),
                            Cep = name[i + 3].InnerText.Replace("&nbsp;", string.Empty),
                            Cidade = name[i + 2].InnerText.Replace("&nbsp;", string.Empty),
                            Rua = name[i].InnerText.Replace("&nbsp;", string.Empty),
                        };

                        hsRespEndereco.Add(reponseEndereco);
                    }
                }

                if (hasNextPage)
                {
                    AcessaProximasPaginas(hsRespEndereco, address, numPages);
                }

                return JsonConvert.SerializeObject(hsRespEndereco);

            }
            catch (HttpRequestException httpEx)
            {

                throw httpEx;
            }
            catch (HtmlWebException htmlEx)
            {
                throw htmlEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Returns the CEP number
        /// </summary>
        /// <param name="address">Full or partial address</param>
        /// <returns>JSON with CEP number</returns>
        public string GetCepByAddress(string address)
        {
            try
            {
                var lstEnderecos = new List<ResponseEndereco>();
                bool hasNextPage = false;
                var hsRespEndereco = new HashSet<ResponseEndereco>();
                var dict = new Dictionary<string, string>
                {
                    {"relaxation",address},
                    {"tipoCEP","ALL" },
                    {"semelhante","N" }
                };

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, UrlCorreio)
                {
                    Content = new FormUrlEncodedContent(dict)
                };

                var httpResponse = _client.SendAsync(httpRequest).Result.Content.ReadAsStringAsync().Result;
                var html = httpResponse;

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var link = htmlDoc.DocumentNode.SelectNodes("//div//a");

                if (link == null)
                {
                    return Constants.NOT_FOUND_MESSAGE;
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
                    return Constants.NOT_FOUND_MESSAGE;
                }

                for (int i = 0; i < name.Count; i++)
                {
                    if (i % 4 == 0)
                    {
                        var reponseEndereco = new ResponseEndereco
                        {
                            Bairro = name[i + 1].InnerText.Replace("&nbsp;", string.Empty),
                            Cep = name[i + 3].InnerText.Replace("&nbsp;", string.Empty),
                            Cidade = name[i + 2].InnerText.Replace("&nbsp;", string.Empty),
                            Rua = name[i].InnerText.Replace("&nbsp;", string.Empty),
                        };

                        hsRespEndereco.Add(reponseEndereco);
                    }
                }

                if (hasNextPage)
                {
                    AcessaProximasPaginas(hsRespEndereco, address, numPages);
                }

                return JsonConvert.SerializeObject(hsRespEndereco);

            }
            catch (HttpRequestException httpEx)
            {

                throw httpEx;
            }
            catch (HtmlWebException htmlEx)
            {
                throw htmlEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region Private Methods

        private void AcessaProximasPaginas(HashSet<ResponseEndereco> hsRespEnd, string endereco,
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
                    var reponseEndereco = new ResponseEndereco
                    {
                        Bairro = name[i + 1].InnerText.Replace("&nbsp;", string.Empty),
                        Cep = name[i + 3].InnerText.Replace("&nbsp;", string.Empty),
                        Cidade = name[i + 2].InnerText.Replace("&nbsp;", string.Empty),
                        Rua = name[i].InnerText.Replace("&nbsp;", string.Empty),
                    };

                    hsRespEnd.Add(reponseEndereco);
                }
            }

            if (hasNextPage)
            {
                AcessaProximasPaginas(hsRespEnd, endereco, numPages, pageIni, pageFim);
            }


        }

        #endregion
    }
}
