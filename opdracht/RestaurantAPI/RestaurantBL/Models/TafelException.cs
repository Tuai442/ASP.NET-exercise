using System.Runtime.Serialization;

namespace RestaurantBL.Models
{
    [Serializable]
    internal class TafelException : Exception
    {
        public TafelException()
        {
        }

        public TafelException(string? message) : base(message)
        {
        }

        public TafelException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected TafelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}