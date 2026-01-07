using Amazon.S3.Model;
using System.Runtime.Intrinsics.Arm;

namespace EasyWorkout.Identity.Api
{
    public class Endpoints
    {
        private const string ApiBase = "api";
        public static class Auth
        {
            private const string Base = $"{ApiBase}/auth";
            public const string Register = $"{Base}/register";
            public const string Login = $"{Base}/login";
            public const string Refresh = $"{Base}/refresh";
            public const string Revoke = $"{Base}revoke/{{refreshToken}}";
            public const string Get = $"{Base}/me";
            public const string Update = $"{Base}/me";
            public const string ChangeEmail = $"{Base}/email";
            public const string ChangePassword = $"{Base}/password";
            public const string DeleteAccount = $"{Base}/me";
        }
    }
}
