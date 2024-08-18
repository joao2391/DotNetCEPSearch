using System.IO;

namespace Tests
{
    public class FakeData
    {
        public static string validCep = "01327-903";
        public static string validAddress = "Rua Frei Caneca";


        public FakeData()
        {
            var fakeReturn = File.ReadAllText(@"./fakeData.json");

        }

    }
}