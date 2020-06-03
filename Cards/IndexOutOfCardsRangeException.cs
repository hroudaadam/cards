using System;
using System.Runtime.Serialization;

namespace Cards
{
    public class IndexOutOfCardsRangeException : Exception
    {
        public IndexOutOfCardsRangeException()
        {
        }

        public IndexOutOfCardsRangeException(string message) : base(message)
        {
        }

        public IndexOutOfCardsRangeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IndexOutOfCardsRangeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
