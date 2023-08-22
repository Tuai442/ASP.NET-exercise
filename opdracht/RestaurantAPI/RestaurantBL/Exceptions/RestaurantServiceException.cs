using System.Runtime.Serialization;

namespace RestaurantBL.Exceptions
{
    [Serializable]
    internal class RestaurantServiceException : Exception
    {
        public RestaurantServiceException()
        {
        }

        public RestaurantServiceException(string? message) : base(message)
        {
        }

        public RestaurantServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RestaurantServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}