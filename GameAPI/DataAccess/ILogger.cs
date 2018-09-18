using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    interface ILogger
    {
        void LogMessage(string Request, string Response, String Exceptions, String Comment);
    }
}
