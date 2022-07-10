using System.Reflection;
using Application.Models.Dtos.Admin;
using Microsoft.AspNetCore.Mvc;
using Xunit.Sdk;

namespace WebApi.Tests.Attributes;

public class LoginDataAttribute : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        LoginDto adminLogin = new("admin", "admin");
        LoginDto badLogin = new("bad", "admin");
        LoginDto wrongPassword = new("admin", "wrong_password");

        yield return new object[] { typeof(OkObjectResult), adminLogin };
        yield return new object[] { typeof(BadRequestResult), badLogin };
        yield return new object[] { typeof(UnauthorizedResult), wrongPassword};
    }
}