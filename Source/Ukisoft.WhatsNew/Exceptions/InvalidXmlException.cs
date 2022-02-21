using System.Runtime.Serialization;
namespace Ukisoft.WhatsNew.Exceptions;

public sealed class InvalidXmlException : Exception
{
    public InvalidXmlException()
    {
    }

    public InvalidXmlException(string? message)
        : base(message)
    {
    }

    public InvalidXmlException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    private InvalidXmlException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
