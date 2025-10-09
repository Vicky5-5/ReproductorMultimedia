using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Spotify
{
    public class SpotifyPerfilResponse
    {
        [JsonProperty("display_name")]
        public string NombreVisible { get; set; }

        [JsonProperty("email")]
        public string CorreoElectronico { get; set; }

        [JsonProperty("country")]
        public string Pais { get; set; }

        [JsonProperty("id")]
        public string Identificador { get; set; }

        [JsonProperty("product")]
        public string TipoCuenta { get; set; }
    }
}
