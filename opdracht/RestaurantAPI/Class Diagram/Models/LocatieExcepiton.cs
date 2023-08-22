using System.Runtime.Serialization;

namespace RestaurantBL.Models
{
    [Serializable]
    internal class LocatieExcepiton : Exception
    {
        public LocatieExcepiton()
        {
        }

        public LocatieExcepiton(string? message) : base(message)
        {
        }

        public LocatieExcepiton(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected LocatieExcepiton(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}