using Moq;
using DotNet.CEP.Search.App;
using DotNet.CEP.Search.App.Models;
using System.Threading;
using System.IO;

namespace Tests
{
    public class FakeData
    {
        Mock<ICepSearch> mockCep;
        ICepSearch cepSearch;
        public static string validCep = "08499";
        public static string validAddress = "Rua Frei Caneca";


        public FakeData()
        {
            var fakeReturn = File.ReadAllText(@"./fakeData.json");

            mockCep = new Mock<ICepSearch>();
            mockCep.Setup(x => x.GetAddressByCepAsync(validCep, new CancellationToken())).ReturnsAsync(fakeReturn);
            mockCep.Setup(x => x.GetAddressByCep(validCep)).Returns(fakeReturn);

            mockCep.Setup(x => x.GetCepByAddressAsync(validAddress, new CancellationToken())).ReturnsAsync(fakeReturn);
            mockCep.Setup(x => x.GetCepByAddress(validAddress)).Returns(fakeReturn);

            cepSearch = mockCep.Object;


        }

    }
}