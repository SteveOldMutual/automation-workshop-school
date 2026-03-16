using Microsoft.AspNetCore.Mvc;

namespace Automation_School_API.Domain.Authentication
{
    public class ApiKeyAttribute : ServiceFilterAttribute
    {
        public ApiKeyAttribute() : base(typeof(ApiKeyAuthorizationFilter))
        {
        }
    }
}
