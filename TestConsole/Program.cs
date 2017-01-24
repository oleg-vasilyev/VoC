using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using VoC.ExternalService;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            VoC.DataAccess.DbInit.DbInit.InitDB();
        }
    }
}
