using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{

    public enum ErrorCodes
    {
        NOT_FOUND = 1,
        COULDNOT_STORE_DATA = 2,
        MISSING_REQUIRED_INFORMATION = 3,
        INVALID_EMAIL = 4,
        CLIENT_NOT_FOUND = 6,
        ADDRESS_DUPLICATE =7,
        EMAIL_DUPLICATE=8
        
    }
    public abstract class Response
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public ErrorCodes ErrorCode { get; set; }
    }
}
