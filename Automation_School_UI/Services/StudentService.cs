using Automation_School_UI.Models;
using Blazored.LocalStorage;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Text;

namespace Automation_School_UI.Services
{
    public class StudentService : IStudentService
    {
        private HttpClient _httpClient;
        private ILocalStorageService _localStorageService;

        private IConfiguration _configuration;
        public StudentService(IConfiguration configuration, ILocalStorageService localStorageService)
        {
            _httpClient = new HttpClient();
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(configuration["ServiceUrl"]); 
            _localStorageService = localStorageService;
        }
        public async Task<string> GetSchoolId() 
        {
            try {
                string jsonString = await _localStorageService.GetItemAsync<string>("authToken");

                // Parse the JSON to get the token
                var jsonDocument = JsonDocument.Parse(jsonString);
                string token = jsonDocument.RootElement.GetProperty("token").GetString();
                var handler = new JwtSecurityTokenHandler();
                //this can check lifetime
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "YourIssuer",
                    ValidAudience = "YourAudience",
                    IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes("YourSuperSecretKeyThatIsAtLeast32CharactersLong")
                            )
                };

                var principal = handler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                if (principal == null && validatedToken is JwtSecurityToken jwtToken)
                {
                    var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
                    principal = new ClaimsPrincipal(identity);
                }

                //var jwtToken = handler.ReadJwtToken(token);

                var claims = principal.Claims;
                var schoolId = claims.FirstOrDefault(x => x.Type == "SchoolId").Value;
                return schoolId;
            } catch { return ""; }
            
        }
        public async Task<Student> CreateStudent(StudentDTO dto)
        {
            dto.SchoolId = await GetSchoolId();
            var response = await _httpClient.PostAsJsonAsync($"/api/Student",dto);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Student>(await response.Content.ReadAsStringAsync());
            }
            return null;
        }

        public async Task<bool> DeleteStudent(string id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Student/{id}");
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            }
            return false;
        }

        public async Task<Student> GetStudent(string id)
        {
            var schoolId = await GetSchoolId();
            var response = await _httpClient.GetAsync($"/api/Student/{id}?schoolId={schoolId}");
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Student>(await response.Content.ReadAsStringAsync());
            }
            return null;
        }

        public async Task<List<Student>> GetStudents()
        {
            var schoolId = await GetSchoolId();
            var response = await _httpClient.GetAsync($"/api/Student?schoolId={schoolId}");
            if (response.IsSuccessStatusCode) 
            {
                return JsonConvert.DeserializeObject<List<Student>>(await response.Content.ReadAsStringAsync());
            }

            return new List<Student>();
        }

        public async Task<Student> UpdateStudent(string id, StudentDTO updatedInfo)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/Student/{id}", updatedInfo);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Student>(await response.Content.ReadAsStringAsync());
            }
            return null;
        }
    }
}
