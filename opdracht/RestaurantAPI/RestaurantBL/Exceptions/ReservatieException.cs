using System.Runtime.Serialization;

[Serializable]
internal class ReservatieException : Exception
{
    public ReservatieException()
    {
    }

    public ReservatieException(string? message) : base(message)
    {
    }

    public ReservatieException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected ReservatieException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}