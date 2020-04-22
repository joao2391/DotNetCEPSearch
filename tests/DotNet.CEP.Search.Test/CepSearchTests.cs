using DotNet.CEP.Search.App;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Tests
{
    public class CepSearchTests
    {

        CepSearch cep;
        string validCep = "12515520";

        [SetUp]
        public void Setup()
        {
            cep = new CepSearch();
        }

        [Test]
        public async Task Should_Return_A_Valid_Address_Async()
        {
            var result = await cep.GetAddressByCepAsync(validCep);

            Assert.IsNotNull(result);
        }

        [Test]
        public void Should_Return_A_Valid_Address()
        {
            var result = cep.GetAddressByCep(validCep);

            Assert.IsNotNull(result);
        }
    }
}