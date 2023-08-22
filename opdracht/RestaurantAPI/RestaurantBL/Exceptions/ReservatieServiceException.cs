using System.Runtime.Serialization;

namespace RestaurantBL.Exceptions
{
    [Serializable]
    internal class ReservatieServiceException : Exception
    {
        public ReservatieServiceException()
        {
        }

        public ReservatieServiceException(string? message) : base(message)
        {
        }

        public ReservatieServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ReservatieServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}