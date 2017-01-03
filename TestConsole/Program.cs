using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VoC.DataAccess;
using VoC.DataAccess.DbInit;
using VoC.ExternalService;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            DbInit.InitDB();

            UserManager manager = new UserManager();
            manager.AddNewUser("alex", "123456");
            manager.RegistredUserActivity(1);
            Thread.Sleep(182000);
            manager.RegistredUserActivity(1);
            Console.Read();
        }
    }
}
