using System.Security.Claims;

namespace Calculator.DataAccess
{
    public interface ISupportUser
    {
        ClaimsPrincipal User { get; set; }
    }
}
