using System.Runtime.Serialization;

namespace RestaurantAPI.Mappers
{
    [Serializable]
    internal class MapException : Exception
    {
        public MapException()
        {
        }

        public MapException(string? message) : base(message)
        {
        }

        public MapException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected MapException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}