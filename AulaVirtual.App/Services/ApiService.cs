using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace AulaVirtual.App.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        
        
        
        private readonly string _baseUrl = "http://10.0.2.2:5291/api";

        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService()
        {
            var handler = new HttpClientHandler();
            
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            _httpClient = new HttpClient(handler) { BaseAddress = new Uri(_baseUrl) };
            
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public void LoadToken()
        {
            var token = SecureStorage.Default.GetAsync("auth_token").Result;
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<string?> LoginAsync(string correo, string contrasena)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/autenticacion/login", new { Correo = correo, Contrasena = contrasena });
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                
                string? token = null;
                string? role = null;

                if (result.TryGetProperty("Token", out var tokenElement) || result.TryGetProperty("token", out tokenElement))
                {
                    token = tokenElement.GetString();
                }

                if (result.TryGetProperty("Rol", out var roleElement) || result.TryGetProperty("rol", out roleElement) || result.TryGetProperty("Role", out roleElement) || result.TryGetProperty("role", out roleElement))
                {
                    role = roleElement.GetString();
                }
                
                if (token != null)
                {
                    await SecureStorage.Default.SetAsync("auth_token", token);
                    await SecureStorage.Default.SetAsync("user_role", role ?? "");
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    return role;
                }
                else
                {
                    throw new Exception("El inicio de sesión fue exitoso pero el servidor no devolvió un token válido.");
                }
            }
            return null;
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            LoadToken();
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/{endpoint}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
                }
                return default;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get error: {ex.Message}");
                return default;
            }
        }

        public async Task<bool> PostAsync<T>(string endpoint, T data)
        {
            LoadToken();
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/{endpoint}", data, _jsonOptions);
            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error del servidor: {response.StatusCode}. {errorMsg}");
            }
            return true;
        }

        public async Task<bool> PutAsync<T>(string endpoint, T data)
        {
            LoadToken();
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{endpoint}", data, _jsonOptions);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Put error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            LoadToken();
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/{endpoint}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete error: {ex.Message}");
                return false;
            }
        }
    }
}
