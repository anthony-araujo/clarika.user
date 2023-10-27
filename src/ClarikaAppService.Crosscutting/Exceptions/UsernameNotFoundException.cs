using System.Security.Authentication;

namespace ClarikaAppService.Crosscutting.Exceptions;

public class UsernameNotFoundException : AuthenticationException
{
    public UsernameNotFoundException(string message) : base(message)
    {
    }
}
