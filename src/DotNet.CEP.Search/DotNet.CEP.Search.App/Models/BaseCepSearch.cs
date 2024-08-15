﻿using DotNet.CEP.Search.App.Utils;
using System;
using System.Net.Http;

namespace DotNet.CEP.Search.App.Models
{
    public abstract class BaseCepSearch
    {
        protected HttpClient _client;

        public BaseCepSearch()
        {
            _client = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(5)
            };
        }

        protected string UrlCorreioGetAddress { get => Constants.URL_CORREIO_GET_ADDRESS; }
        protected string UrlCorreioGetCep { get => Constants.URL_CORREIO_GET_CEP; }

    }
}
