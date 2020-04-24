using System.Threading.Tasks;

namespace DotNet.CEP.Search.App.Models
{
    internal interface ICepSearch
    {
        Task<string> GetAddressByCepAsync(string cep);

        string GetAddressByCep(string cep);

        Task<string> GetCepByAddressAsync(string address);

        string GetCepByAddress(string address);
    }
}
