using System.Runtime.Serialization;

namespace RestaurantDL.Exceptions
{
    [Serializable]
    internal class LocatieRepositoryException : Exception
    {
        public LocatieRepositoryException()
        {
        }

        public LocatieRepositoryException(string? message) : base(message)
        {
        }

        public LocatieRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected LocatieRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}