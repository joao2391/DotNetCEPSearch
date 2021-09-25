using DotNet.CEP.Search.App.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace DotNet.CEP.Search.App.Models
{
    public abstract class BaseCepSearch
    {        
        protected HttpClient _client;

        public BaseCepSearch()
        {
            
            HttpClientHandler clientHandler = new HttpClientHandler();
            //clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            clientHandler.SslProtocols = System.Security.Authentication.SslProtocols.None;
            
            _client = new HttpClient(clientHandler)
            {
                Timeout = TimeSpan.FromSeconds(5)
            };
        }
        
        protected string UrlCorreio { get => Constants.URL_CORREIO; }

    }
}
