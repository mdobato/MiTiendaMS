using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiTiendaMS.Authorization
{
    // IAuthorizationRequirement: es una interfaz marcadora, no necesita ninguna implementación
    public class AuthorizationRequirement : IAuthorizationRequirement
    {
        public string AUProperty1 { get; set; }
        public string AUProperty2 { get; set; }
    }

    public class AuthorizationRequirementHandler : AuthorizationHandler<AuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement)
        {
            var resource = context.Resource;
            var prop = requirement.AUProperty1;
            //context.Fail();
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
