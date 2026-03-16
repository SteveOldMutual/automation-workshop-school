using Automation_School_UI.Models;
using Automation_School_UI.Pages.Students;
using Blazored.LocalStorage;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Automation_School_UI.Services
{
    public class RollcallService : IRollcallService
    {
        private HttpClient _httpClient;
        private IConfiguration _configuration;
        private ILocalStorageService _localStorageService;

        public RollcallService(IConfiguration configuration, ILocalStorageService localStorageService)
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
                Console.WriteLine(schoolId);

                return schoolId;
            } catch(Exception ex) { 
                Console.WriteLine(ex.Message);
                return ""; }
            
        }
        public async Task<ROLLCALLOUTCOME> DismissStudent(string id)
        {
            var response = await _httpClient.GetAsync($"/api/Rollcall/Dismiss/{id}");
            if (response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.BadRequest) 
            {
                return Enum.Parse<ROLLCALLOUTCOME>(await response.Content.ReadAsStringAsync());
            }
            return ROLLCALLOUTCOME.ERROR;
        }

        public async Task<RollcallMetadata> GetRollCallData(DateTime dateTime)
        {
            try
            {
                var schoolId = await GetSchoolId();
                var response = await _httpClient.GetAsync($"/api/Rollcall?dateTime={dateTime.Date.ToString()}&schoolId={schoolId}");
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<RollcallMetadata>(await response.Content.ReadAsStringAsync());
                }
            
            } catch(Exception e) {
                Console.WriteLine(e.Message);
                return null; 
            }
            return null;
           
        }

        public async Task<RollcallMetadata> GetStudentRollcallData(string id)
        {
            var schoolId = await GetSchoolId();
            var response = await _httpClient.GetAsync($"/api/Rollcall/{id}?schoolId={schoolId}");
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<RollcallMetadata>(await response.Content.ReadAsStringAsync());
            }
            return null;
        }

        public async Task<ROLLCALLOUTCOME> MarkAbsent(string id)
        {
             var schoolId = await GetSchoolId();
            var response = await _httpClient.GetAsync($"/api/Rollcall/MarkAbsent/{id}?schoolId={schoolId}");
            if (response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return Enum.Parse<ROLLCALLOUTCOME>(await response.Content.ReadAsStringAsync());
            }
            return ROLLCALLOUTCOME.ERROR;
        }

        public async Task<ROLLCALLOUTCOME> MarkPresent(string id)
        {
             var schoolId = await GetSchoolId();
            var response = await _httpClient.GetAsync($"/api/Rollcall/MarkPresent/{id}?schoolId={schoolId}");
            if (response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return Enum.Parse<ROLLCALLOUTCOME>(await response.Content.ReadAsStringAsync());
            }
            return ROLLCALLOUTCOME.ERROR;
        }

        public async Task<ROLLCALLOUTCOME> UndoDismiss(string id)
        {
            var response = await _httpClient.GetAsync($"/api/Rollcall/UndoDismiss/{id}");
            if (response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return Enum.Parse<ROLLCALLOUTCOME>(await response.Content.ReadAsStringAsync());
            }
            return ROLLCALLOUTCOME.ERROR;
        }
    }
}
