using System.Runtime.Serialization;

namespace RestaurantBL.Exceptions
{
    [Serializable]
    public class GebruikerServiceException : Exception
    {
        public GebruikerServiceException()
        {
        }

        public GebruikerServiceException(string? message) : base(message)
        {
        }

        public GebruikerServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected GebruikerServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}