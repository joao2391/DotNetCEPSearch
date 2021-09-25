using Newtonsoft.Json;

namespace DotNet.CEP.Search.App.Models
{
    public class ResponseCorreios
    {
        [JsonProperty("erro")]
        public bool Erro { get; set; }

        [JsonProperty("mensagem")]
        public string Mensagem { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("dados")]
        public Dados[] Dados { get; set; }
    }
}
