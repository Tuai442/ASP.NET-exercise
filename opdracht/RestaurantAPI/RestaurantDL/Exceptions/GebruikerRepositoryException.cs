using System.Runtime.Serialization;

namespace RestaurantDL.Exceptions
{
    [Serializable]
    internal class GebruikerRepositoryException : Exception
    {
        public GebruikerRepositoryException()
        {
        }

        public GebruikerRepositoryException(string? message) : base(message)
        {
        }

        public GebruikerRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected GebruikerRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}