namespace DotNet.CEP.Search.App.Models
{
    public abstract class BaseInfo
    {
        public string Rua { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public string Cep { get; set; }
        public string Uf { get; set; }
    }
}
