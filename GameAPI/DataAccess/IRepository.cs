using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    interface IRepository
    {
        String CreateUser(String Fname, String Lname, String Uname);
        Boolean CheckTokenValidation(String apikey);
    }
}
