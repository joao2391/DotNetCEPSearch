using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace DotNet.CEP.Search.App.Models
{
    public interface ICepSearch
    {
        Task<ResponseAddress> GetAddressByCepAsync(string cep, CancellationToken cancellationToken);

        ResponseAddress GetAddressByCep(string cep);

        Task<HashSet<ResponseCep>> GetCepByAddressAsync(string address, CancellationToken cancellationToken);

        HashSet<ResponseCep> GetCepByAddress(string address);
    }
}
