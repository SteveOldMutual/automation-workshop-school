namespace Automation_School_API.Domain.Authentication
{
    public class ApiKeyValidator : IApiKeyValidator
    {
        private IConfiguration _configuration;
        public ApiKeyValidator(IConfiguration config) 
        {
            _configuration = config;
        }
        public bool IsValid(string apiKey)
        {
            // Implement your validation logic here
            return apiKey == _configuration["ApiKey"];
        }
    }

    public interface IApiKeyValidator
    {
        bool IsValid(string apiKey);
    }
}
