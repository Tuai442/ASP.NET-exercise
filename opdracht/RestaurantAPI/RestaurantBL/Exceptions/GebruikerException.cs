using System.Runtime.Serialization;

[Serializable]
public class GebruikerException : Exception
{
    public GebruikerException()
    {
    }

    public GebruikerException(string? message) : base(message)
    {
    }

    public GebruikerException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected GebruikerException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}