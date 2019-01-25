using Application.Core.Identity.AppSession;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Helper.ContentWrapper.Core.BaseApiController
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BaseApiController : ControllerBase
    {
        protected string currentUserId;
        protected string currentUsername;
        protected string currentUserEmail;
        protected readonly IAppSession AppSession;
        public BaseApiController()
        {
            AppSession = NullAppSession.Instance;
        }   
//        protected void SetUserClaimInfo()
//        {
//             try
//            {

//                var claimsIdentity = this.User.Identity as ClaimsIdentity;
//                var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;


//                string userId1 = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


//                var claimEmail = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email);
//                var email = (claimEmail == null ? string.Empty : claimEmail.Value);

//                currentUserId = ClaimsPrincipal.Current.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault() != null ? ClaimsPrincipal.Current.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value : string.Empty; // will give the user's userId
//                currentUsername = User.FindFirst(ClaimTypes.Name).Value; // will give the user's userName
//                currentUserEmail = User.FindFirst(ClaimTypes.Email).Value; // will give the user's Email
//            }
//            catch (System.Exception ex)
//            {
//                this.currentUserId = string.Empty;
//                this.currentUsername = string.Empty;
//                this.currentUserEmail = string.Empty;
//            }
//}
    }
}
