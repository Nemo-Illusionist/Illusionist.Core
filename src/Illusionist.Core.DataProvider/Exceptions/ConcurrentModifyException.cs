using System;
using System.Runtime.Serialization;

namespace Illusionist.Core.DataProvider.Exceptions
{
    public class ConcurrentModifyException : DataProviderException
    {
        public ConcurrentModifyException()
        {
        }

        public ConcurrentModifyException(string message)
            : base(message)
        {
        }

        public ConcurrentModifyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ConcurrentModifyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}