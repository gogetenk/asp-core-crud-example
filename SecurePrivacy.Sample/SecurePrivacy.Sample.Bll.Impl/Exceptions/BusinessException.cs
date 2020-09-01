using System;
using System.Collections.Generic;
using System.Text;

namespace SecurePrivacy.Sample.Bll.Impl.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
        }
    }
}
