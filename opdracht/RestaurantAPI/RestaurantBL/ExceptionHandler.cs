using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBL
{
    public static class ExceptionHandler
    {
        public static string GetMessage(Exception ex)
        {
            if (ex.InnerException != null)
            {
                return ex.InnerException.Message;
            }
            else
            {
                return ex.Message;
            }
        }

       
    }
}
