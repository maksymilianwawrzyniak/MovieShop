using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebApplication.Models;

namespace WebApplication.Utils
{
    public static class SessionUtil
    {
        public static User GetUser(IHttpContextAccessor httpContextAccessor)
        {
            if (!httpContextAccessor.HttpContext.Session.TryGetValue("user", out var value))
                return null;
            var serializedUser = Encoding.UTF8.GetString(value);
            return JsonConvert.DeserializeObject<User>(serializedUser);
        }
    }
}