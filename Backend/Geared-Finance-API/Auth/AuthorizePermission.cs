using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Service.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using Utilities;
namespace Geared_Finance_API.Auth
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizePermission : Attribute, IAuthorizationFilter
    {
        private readonly string _module;
        private readonly string _permission;
        private readonly string _property;

        public AuthorizePermission(string module, string permission, string property = "")
        {
            _permission = permission;
            _module = module;
            _property = property;
        }
        async void IAuthorizationFilter.OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"];
            token = token.ToString()["Bearer ".Length..].Trim();
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            if (tokenHandler.ReadToken(token) is not JwtSecurityToken)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var claimsRoleName = jwtToken.Claims
                  .FirstOrDefault(claim => claim.Type == Constants.USER_ROLE)?.Value;
            if (string.IsNullOrEmpty(claimsRoleName))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            int roleId = ExtensionMethods.GetRoleIdFromRoleEnum(claimsRoleName);
            if (roleId == -1)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            IRolePermissionService? permissionService = context.HttpContext.RequestServices.GetService<IRolePermissionService>();
            if (ExtensionMethods.IsNullObject(permissionService))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            RightsDTO permissionRight = await permissionService.GetRightByModule(_module, roleId);
            if (ExtensionMethods.IsNullObject(permissionRight))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            bool isEdit = false;
            if (!string.IsNullOrWhiteSpace(_property))
            {
                context.HttpContext.Request.EnableBuffering();
                string requestBody;
                //context.HttpContext.Request.Body.Position = 0;
                using (var reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8, false, 1, true))
                {
                    requestBody = await reader.ReadToEndAsync();
                }
                context.HttpContext.Request.Body.Position = 0;
                using JsonDocument jsonDoc = JsonDocument.Parse(requestBody);
                var rootElement = jsonDoc.RootElement;
                if (rootElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var element in rootElement.EnumerateArray())
                    {
                        foreach (var item in element.EnumerateObject())
                        {
                            if (item.Name.ToLower().Equals(_property.ToLower()))
                            {
                                var idValue = item.Value;
                                if (idValue.ValueKind == JsonValueKind.Null || (idValue.ValueKind == JsonValueKind.Number && idValue.GetInt32() == 0))
                                {
                                    isEdit = false;
                                }
                                else
                                {
                                    isEdit = true;
                                }
                                break;
                            }
                        }
                        if (isEdit) { break; }
                    }
                }
                else
                {
                    foreach (var item in rootElement.EnumerateObject())
                    {
                        if (item.Name.ToLower().Equals(_property.ToLower()))
                        {
                            var idValue = item.Value;
                            if (idValue.ValueKind == JsonValueKind.Null || (idValue.ValueKind == JsonValueKind.Number && idValue.GetInt32() == 0))
                            {
                                isEdit = false;
                            }
                            else
                            {
                                isEdit = true;
                            }
                        }
                    }
                }
            }
            bool hasPermission = false;
            switch (_permission)
            {
                case Constants.CAN_VIEW: hasPermission = permissionRight.CanView ?? false; break;
                case Constants.CAN_ADD: hasPermission = permissionRight.CanAdd ?? false; ; break;
                case Constants.CAN_EDIT: hasPermission = permissionRight.CanEdit ?? false; ; break;
                case Constants.CAN_DELETE: hasPermission = permissionRight.CanDelete ?? false; break;
                case Constants.CAN_UPSERT:
                    if (isEdit)
                    {
                        hasPermission = permissionRight.CanEdit ?? false;
                    }
                    else
                    {
                        hasPermission = permissionRight.CanAdd ?? false;
                    }
                    break;
            }
            if (!hasPermission)
            {
                context.Result = new UnauthorizedResult();
            }

        }
    }
}