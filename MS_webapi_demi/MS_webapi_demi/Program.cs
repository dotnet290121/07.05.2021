using System;
using System.Threading.Tasks;

namespace MS_webapi_demi
{
    class Program
    {
        static void Main(string[] args)
        {

            Task.Run(() => {
                var flights = DbContext.ReadFromDb("sp_getflights"); 
            });
            Task.Run(() => {
                var flights = DbContext.ReadFromDb("sp_getarilines");
            });
            Task.Run(() => {
                var flights = DbContext.ReadFromDb("sp_gettickets");
            });
            Task.Run(() => {
                var flights = DbContext.ReadFromDb("sp_getcountries");
            });
            Task.Run(() => {
                var flights = DbContext.ReadFromDb("sp_getcustomers");
            });
        }
    }
}
