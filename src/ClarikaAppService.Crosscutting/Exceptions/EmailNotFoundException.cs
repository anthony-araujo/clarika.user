using ClarikaAppService.Crosscutting.Constants;

namespace ClarikaAppService.Crosscutting.Exceptions;

public class EmailNotFoundException : BaseException
{
    public EmailNotFoundException() : base(ErrorConstants.EmailNotFoundType, "Email address not registered")
    {
    }
}
