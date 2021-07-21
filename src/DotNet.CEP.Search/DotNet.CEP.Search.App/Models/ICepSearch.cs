using System.Threading.Tasks;
using System.Threading;

namespace DotNet.CEP.Search.App.Models
{
    public interface ICepSearch
    {
        Task<string> GetAddressByCepAsync(string cep, CancellationToken cancellationToken);

        string GetAddressByCep(string cep);

        Task<string> GetCepByAddressAsync(string address, CancellationToken cancellationToken);

        string GetCepByAddress(string address);
    }
}
