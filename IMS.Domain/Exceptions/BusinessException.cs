namespace IMS.Domain.Exceptions;

public class BusinessException : Exception
{
    public string Code { get; }
    
    public BusinessException(string message, string code = "business_error") : base(message)
    {
        Code = code;
    }
}