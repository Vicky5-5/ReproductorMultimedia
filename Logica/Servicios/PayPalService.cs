using Logica.PayPal_Settings;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class PayPalService
{
    private readonly PayPalSettings _settings; // Configuración de PayPal
    private readonly HttpClient _httpClient; // Cliente HTTP para hacer solicitudes a la API de PayPal

    // Constructor que recibe la configuración de PayPal
    public PayPalService(PayPalSettings settings)
    {
        _settings = settings;  // Asignar la configuración recibida a la variable privada
        _httpClient = new HttpClient // Cofiguración del cliente HTTP con la URL base de PayPal
        {
            BaseAddress = new Uri(_settings.BaseUrl)
        };
    }

    public string ObtenerAccessToken()
    {
        // Crear la cadena de autenticación en Base64
        var authString = $"{_settings.ClientId}:{_settings.ClientSecret}";
        var authBytes = Encoding.UTF8.GetBytes(authString);
        var authHeader = Convert.ToBase64String(authBytes);

        // Configurar el encabezado de autorización para la solicitud
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", authHeader);

        // Configurar el contenido de la solicitud para obtener el token
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });

        // Se define el cuerpo de la solicitud POST para obtener el token

        var response = _httpClient.PostAsync("/v1/oauth2/token", content).Result;

        // Manejar la respuesta
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error al obtener el token de acceso de PayPal");
        }

        var json = response.Content.ReadAsStringAsync().Result;

        var token = JsonSerializer.Deserialize<PayPalTokenResponse>(json);

        return token.access_token;
    }

    public string CrearOrden()
    {
        // Llamar al método para obtener el token de acceso
        var accessToken = ObtenerAccessToken();

        _httpClient.DefaultRequestHeaders.Clear(); // Limpiar encabezados previos
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken); // Configurar el encabezado de autorización con el token
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // Aceptar respuestas en formato JSON

        // Se define el cuerpo de la solicitud POST para crear una orden
        var orden = new
        {
            intent = "CAPTURE", // Tipo de intención de la orden
            purchase_units = new[] // Define que el usuario va a comprar
            {
                new {
                    amount = new {
                        currency_code = "USD",
                        value = "9.99"
                    }
                }
            },
            application_context = new
            {
                // URLs de retorno y cancelación
                return_url = "",
                cancel_url = ""
            }
        };

        // Serializar el objeto orden a JSON
        var json = JsonSerializer.Serialize(orden);
        var contenido = new StringContent(json, Encoding.UTF8, "application/json"); // Se empaqueta el JSON en el contenido de la solicitud

        // Enviar la solicitud POST para crear la orden
        var response = _httpClient.PostAsync("/v2/checkout/orders", contenido).Result;
        var responseJson = response.Content.ReadAsStringAsync().Result;

        // Manejar la respuesta
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error al crear orden: " + responseJson);
        }

        return responseJson;
    }
}

// Clase auxiliar para deserializar la respuesta del token de PayPal
public class PayPalTokenResponse
{
    public string access_token { get; set; }
    public string token_type { get; set; }
    public int expires_in { get; set; }
}
