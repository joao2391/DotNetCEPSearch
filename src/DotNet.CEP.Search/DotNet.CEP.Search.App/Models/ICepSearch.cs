using System.Threading.Tasks;
using System.Threading;

namespace DotNet.CEP.Search.App.Models
{
    public interface ICepSearch
    {
        Task<ResponseAddress> GetAddressByCepAsync(string cep, CancellationToken cancellationToken);

        ResponseAddress GetAddressByCep(string cep);

        Task<ResponseCep> GetCepByAddressAsync(string address, CancellationToken cancellationToken);

        ResponseCep GetCepByAddress(string address);
    }
}
