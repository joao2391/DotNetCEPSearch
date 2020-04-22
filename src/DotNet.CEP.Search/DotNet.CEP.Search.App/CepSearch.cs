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
    public class CepSearch : BaseCepSearch
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
            catch (Exception ex)
            {

                return string.Concat(Constants.ERROR_MESSAGE, ex.Message);
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
            catch (Exception ex)
            {

                return string.Concat(Constants.ERROR_MESSAGE, ex.Message);
            }
        }

    }
}
