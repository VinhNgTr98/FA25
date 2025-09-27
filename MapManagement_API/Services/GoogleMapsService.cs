using System.Text.Json.Nodes;

namespace MapManagement_API.Services
{
    public class GoogleMapsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        private const string BaseUrl = "https://maps.googleapis.com/maps/api";


        public GoogleMapsService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }


        private string GetApiKey() => _config["GoogleMaps:ApiKey"]
        ?? throw new InvalidOperationException("Google Maps API key not configured");


        private async Task<JsonNode> SendAsync(string url, CancellationToken ct = default)
        {
            var client = _httpClientFactory.CreateClient();
            using var resp = await client.GetAsync(url, ct);
            var content = await resp.Content.ReadAsStringAsync(ct);
            resp.EnsureSuccessStatusCode();
            return JsonNode.Parse(content)!;
        }


        public async Task<JsonNode> GeocodeAsync(string address, string? language, string? region, CancellationToken ct = default)
        {
            var key = GetApiKey();
            var qp = new List<string> { $"address={Uri.EscapeDataString(address)}", $"key={key}" };
            if (!string.IsNullOrWhiteSpace(language)) qp.Add($"language={Uri.EscapeDataString(language)}");
            if (!string.IsNullOrWhiteSpace(region)) qp.Add($"region={Uri.EscapeDataString(region)}");
            var url = $"{BaseUrl}/geocode/json?{string.Join("&", qp)}";
            return await SendAsync(url, ct);
        }


        public async Task<JsonNode> DirectionsAsync(string origin, string destination, string? mode, string? language, CancellationToken ct = default)
        {
            var key = GetApiKey();
            var qp = new List<string> {
$"origin={Uri.EscapeDataString(origin)}",
$"destination={Uri.EscapeDataString(destination)}",
$"key={key}"
};
            if (!string.IsNullOrWhiteSpace(mode)) qp.Add($"mode={Uri.EscapeDataString(mode)}");
            if (!string.IsNullOrWhiteSpace(language)) qp.Add($"language={Uri.EscapeDataString(language)}");
            var url = $"{BaseUrl}/directions/json?{string.Join("&", qp)}";
            return await SendAsync(url, ct);
        }


        public async Task<JsonNode> PlaceAutocompleteAsync(string input, string? language, string? types, string? locationBias, CancellationToken ct = default)
        {
            var key = GetApiKey();
            var qp = new List<string> { $"input={Uri.EscapeDataString(input)}", $"key={key}" };
            if (!string.IsNullOrWhiteSpace(language)) qp.Add($"language={Uri.EscapeDataString(language)}");
            if (!string.IsNullOrWhiteSpace(types)) qp.Add($"types={Uri.EscapeDataString(types)}");
            if (!string.IsNullOrWhiteSpace(locationBias)) qp.Add($"locationbias={Uri.EscapeDataString(locationBias)}");
            var url = $"{BaseUrl}/place/autocomplete/json?{string.Join("&", qp)}";
            return await SendAsync(url, ct);
        }
    }
}
