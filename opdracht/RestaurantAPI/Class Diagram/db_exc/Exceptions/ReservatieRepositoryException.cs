using System.Runtime.Serialization;

namespace RestaurantDL.Exceptions
{
    [Serializable]
    public class ReservatieRepositoryException : Exception
    {
        public ReservatieRepositoryException()
        {
        }

        public ReservatieRepositoryException(string? message) : base(message)
        {
        }

        public ReservatieRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ReservatieRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}