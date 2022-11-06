using System.ComponentModel;

namespace CarBom.Models
{
    public class Service
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
    }

    public enum ServiceTypes
    {
        [Description("Serviços Gerais")]
        ServicosGerais,

        [Description("Tinturaria")]
        Tinturaria,

        [Description("Funilaria")]
        Funilaria,

        [Description("Peças")]
        Pecas,

        [Description("Conserto")]
        Conserto
    }
}
