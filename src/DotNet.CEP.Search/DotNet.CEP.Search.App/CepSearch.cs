using DotNet.CEP.Search.App.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;

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
        /// Returns the raw data address 
        /// </summary>
        /// <param name="cep">CEP number without '-'</param>
        /// <param name="cancellationToken">Token to cancel task</param>
        /// <returns>JSON with address</returns>
        public async Task<ResponseCorreios> GetAddressByCepRawDataAsync(string cep, CancellationToken cancellationToken = default)
        {
            try
            {
                var address = await GetAddressFromCorreiosByCepRawData(cep, cancellationToken).ConfigureAwait(false);

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
            catch (JsonSerializationException)
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
                    {"pagina","/app/endereco/index.php"},
                    {"endereco",cep },
                    {"tipoCEP","ALL" }
                };

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, UrlCorreio)
            {
                Content = new FormUrlEncodedContent(dict),
            };

            var httpResponse = await _client.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
            var cepResponse = await httpResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            var respConvertido = JsonConvert.DeserializeObject<ResponseCorreios>(cepResponse);

            var size = respConvertido.Dados.Length;

            var address = new ResponseAddress()
            {
                Infos = new AddressInfo[size]
            };

            for (int i = 0; i < size; i++)
            {
                address.Infos[i] = new AddressInfo
                {
                    Bairro = respConvertido.Dados[i].Bairro,
                    Cep = respConvertido.Dados[i].Cep,
                    Cidade = respConvertido.Dados[i].Localidade,
                    Rua = respConvertido.Dados[i].LogradouroDNEC,
                    Uf = respConvertido.Dados[i].Uf
                };
            }

            return address;
        }

        private async Task<ResponseCorreios> GetAddressFromCorreiosByCepRawData(string cep, CancellationToken cancellationToken)
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

            var httpResponse = await _client.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
            var cepResponse = await httpResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            var respConvertido = JsonConvert.DeserializeObject<ResponseCorreios>(cepResponse);

            return respConvertido;
        }

        private async Task<ResponseCep> GetCepFromCorreiosByAddress(string address, CancellationToken cancellationToken)
        {
            var dict = new Dictionary<string, string>
                {
                    {"pagina","/app/endereco/index.php"},
                    {"endereco",address },
                    {"tipoCEP","ALL" }
                };

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, UrlCorreio)
            {
                Content = new FormUrlEncodedContent(dict),
            };

            var httpResponse = await _client.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);

            var cepResponse = await httpResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            var respConvertido = JsonConvert.DeserializeObject<ResponseCorreios>(cepResponse);

            var dataLength = respConvertido.Dados.Length;

            var cep = new ResponseCep
            {
                Infos = new CepInfo[dataLength]
            };

            for (int i = 0; i < respConvertido?.Dados.Length; i++)
            {

                cep.Infos[i] = new CepInfo
                {
                    Bairro = respConvertido.Dados[i].Bairro,
                    Cep = respConvertido.Dados[i].Cep,
                    Cidade = respConvertido.Dados[i].Localidade,
                    Rua = respConvertido.Dados[i].LogradouroDNEC,
                    Uf = respConvertido.Dados[i].Uf
                };
            }


            return cep;
        }

    }
}
