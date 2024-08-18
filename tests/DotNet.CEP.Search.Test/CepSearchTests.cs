using DotNet.CEP.Search.App;
using DotNet.CEP.Search.App.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Tests
{
    public class CepSearchTests
    {
        //private string fakeData = File.ReadAllText(@"./fakeData.html");

        [SetUp]
        public void Setup() 
        {

        }

        [Test]
        public async Task Should_Return_A_Valid_Address_Async()
        {
            var cep = new CepSearch();
            
            var result = await cep.GetAddressByCepAsync(FakeData.validCep);            

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ResponseAddress>(result);
            Assert.IsNotNull(result.Bairro);
        }

        [Test]
        public void Should_Return_A_Valid_Address()
        {
            var cep = new CepSearch();

            var result = cep.GetAddressByCep(FakeData.validCep);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ResponseAddress>(result);
            Assert.IsNotNull(result.Bairro);
        }

        [Test]
        public async Task Should_Return_A_Valid_Cep_Async()
        {
            var cep = new CepSearch();

            var result = await cep.GetCepByAddressAsync(FakeData.validAddress);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<HashSet<ResponseCep>>(result);
            Assert.AreEqual(166, result.Count);
        }

        [Test]
        public void Should_Return_A_Valid_Cep()
        {
            var cep = new CepSearch();
            
            var result = cep.GetCepByAddress(FakeData.validAddress);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<HashSet<ResponseCep>>(result);
            Assert.AreEqual(166, result.Count);
        }
    }
}