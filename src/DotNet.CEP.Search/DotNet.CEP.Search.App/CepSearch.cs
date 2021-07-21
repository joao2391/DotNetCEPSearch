using DotNet.CEP.Search.App.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace DotNet.CEP.Search.App
{
    /// <summary>
    /// Cep Search
    /// </summary>
    public class CepSearch : BaseCepSearch, ICepSearch
    {
        // public CepSearch()
        // {
        //     ServicePointManager.ServerCertificateValidationCallback =  delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) 
        //     { 
        //         return true; 
        //     };
        // }

        /// <summary>
        /// Returns the address
        /// </summary>
        /// <param name="cep">CEP number without '-'</param>
        /// <param name="cancellationToken">Token to cancel task</param>
        /// <returns>JSON with address</returns>
        public async Task<string> GetAddressByCepAsync(string cep, CancellationToken cancellationToken = default)
        {
            try
            {
                var dict = new Dictionary<string, string>
                {
                    {"pagina","/app/endereco/index.php"},
                    {"endereco",cep },
                    {"tipoCEP","ALL" }
                };
                
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, UrlCorreio)
                {
                    Content = new FormUrlEncodedContent(dict),                    
                };

                var httpResponse = await _client.SendAsync(httpRequest, cancellationToken);
                var cepResponse = await httpResponse.Content.ReadAsStringAsync();

                return cepResponse;
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
            try
            {
                var dict = new Dictionary<string, string>
                {
                    {"pagina","/app/endereco/index.php"},
                    {"endereco",cep },
                    {"tipoCEP","ALL" }
                };
                
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, UrlCorreio)
                {
                    Content = new FormUrlEncodedContent(dict),                    
                };

                var httpResponse = _client.SendAsync(httpRequest).Result;
                var cepResponse = httpResponse.Content.ReadAsStringAsync().Result;

                return cepResponse;
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
        /// <param name="cancellationToken">Token to cancel task</param>
        /// <returns>JSON with CEP number</returns>
        public async Task<string> GetCepByAddressAsync(string address, CancellationToken cancellationToken = default)
        {
            try
            {
                var dict = new Dictionary<string, string>
                {
                    {"pagina","/app/endereco/index.php"},
                    {"endereco",address},
                    {"tipoCEP","ALL" }
                };
                
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, UrlCorreio)
                {
                    Content = new FormUrlEncodedContent(dict),                    
                };

                var httpResponse = await _client.SendAsync(httpRequest, cancellationToken);
                var cepResponse = await httpResponse.Content.ReadAsStringAsync();

                return cepResponse;
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
                var dict = new Dictionary<string, string>
                {
                    {"pagina","/app/endereco/index.php"},
                    {"endereco",address},
                    {"tipoCEP","ALL" }
                };
                
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, UrlCorreio)
                {
                    Content = new FormUrlEncodedContent(dict),                    
                };

                var httpResponse = _client.SendAsync(httpRequest).Result;
                var cepResponse =  httpResponse.Content.ReadAsStringAsync().Result;

                return cepResponse;
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
        
    }
}
