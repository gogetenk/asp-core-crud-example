using System;

namespace SecurePrivacy.Sample.Dal.Exceptions
{
    public class DalException : Exception
    {
        public DalException(string message) : base(message)
        {
        }
    }
}
