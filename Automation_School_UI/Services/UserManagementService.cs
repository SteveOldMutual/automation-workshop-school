using Automation_School_UI.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Automation_School_UI.Services
{
    public class UserManagementService : IUserManagementService
    {
        private HttpClient _httpClient;
        private IConfiguration _configuration;
        public UserManagementService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _configuration = configuration;
           _httpClient.BaseAddress = new Uri(configuration["ServiceUrl"]); 
        }
        public async Task<List<User>> GetUsers()
        {
            var response = await _httpClient.GetAsync("/api/UserManagement/users");
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<User>>(await response.Content.ReadAsStringAsync());
            }

            else 
            {
                return new List<User>();
            }
        }

      
    }
}
