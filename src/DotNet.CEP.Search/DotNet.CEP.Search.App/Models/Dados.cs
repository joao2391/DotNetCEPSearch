using Newtonsoft.Json;

namespace DotNet.CEP.Search.App.Models
{
    public class Dados
    {
        [JsonProperty("uf")]
        public string Uf { get; set; }

        [JsonProperty("localidade")]
        public string Localidade { get; set; }

        [JsonProperty("locNoSem")]
        public string LocalidadeNoSem { get; set; }

        [JsonProperty("locNu")]
        public string LocalidadeNu { get; set; }

        [JsonProperty("localidadeSubordinada")]
        public string LocalidadeSubordinada { get; set; }

        [JsonProperty("logradouroDNEC")]
        public string LogradouroDNEC { get; set; }

        [JsonProperty("logradouroTextoAdicional")]
        public string LogradouroTextoAdicional { get; set; }

        [JsonProperty("logradouroTexto")]
        public string LogradouroTexto { get; set; }

        [JsonProperty("bairro")]
        public string Bairro { get; set; }

        [JsonProperty("baiNu")]
        public string BairroNu { get; set; }

        [JsonProperty("nomeUnidade")]
        public string NomeUnidade { get; set; }

        [JsonProperty("cep")]
        public string Cep { get; set; }

        [JsonProperty("tipoCep")]
        public string TipoCep { get; set; }

        [JsonProperty("numeroLocalidade")]
        public string NumeroLocalidade { get; set; }

        [JsonProperty("situacao")]
        public string Situacao { get; set; }
        
        [JsonIgnore]
        public string[] FaixasCaixaPostal { get; set; }
        
        [JsonIgnore]
        public string[] FaixasCep { get; set; }

    }
}
