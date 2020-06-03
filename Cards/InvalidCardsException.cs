using System;
using System.Runtime.Serialization;

namespace Cards
{
    public class InvalidCardsException : Exception
    {
        public InvalidCardsException()
        {
        }

        public InvalidCardsException(string message) : base(message)
        {
        }

        public InvalidCardsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidCardsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
