using DotNet.CEP.Search.App;
using DotNet.CEP.Search.App.Models;
using NUnit.Framework;
using System.Threading.Tasks;
using Moq;
using System.Threading;

namespace Tests
{
    public class CepSearchTests
    {
<<<<<<< HEAD

        CepSearch cep;        
        string validCep = "08499";
        string validAddress = "Rua Frei Caneca";

=======
        FakeData fakeData;
        
        
>>>>>>> test/net5
        [SetUp]
        public void Setup()
        {            
            fakeData = new FakeData();
        }

        [Test]
        public async Task Should_Return_A_Valid_Address_Async()
        {
            var cep = new CepSearch();
            
            var result = await cep.GetAddressByCepAsync(FakeData.validCep);            

            Assert.IsNotNull(result);
        }

        [Test]
        public void Should_Return_A_Valid_Address()
        {
            var cep = new CepSearch();

            var result = cep.GetAddressByCep(FakeData.validCep);

            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Should_Return_A_Valid_Cep_Async()
        {
            var cep = new CepSearch();

            var result = await cep.GetCepByAddressAsync(FakeData.validAddress);

            Assert.IsNotNull(result);
        }

        [Test]
        public void Should_Return_A_Valid_Cep()
        {
            var cep = new CepSearch();
            
            var result = cep.GetCepByAddress(FakeData.validAddress);

            Assert.IsNotNull(result);
        }
    }
}