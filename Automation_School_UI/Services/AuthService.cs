using Automation_School_UI.Models;
using System.Net.Http.Json;

namespace Automation_School_UI.Services
{
    public class AuthService : IAuthService
    {
        private HttpClient _httpClient;
        private IConfiguration _configuration;
        public AuthService(IConfiguration configuration) 
        {
            _httpClient = new HttpClient();
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(configuration["ServiceUrl"]); 
        }
        public async Task<string> Authenticate(LoginDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Authentication/login", dto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            else 
            {
                return null;
            }
        }

        public async Task<HttpResponseMessage> RegisterUser(RegisterDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Authentication/register", dto);

            return response;
            
        }
    }
}
