using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;


namespace ForceRefreshRateService
{
    internal static class Program
    {
        static void Main()
        {
            #if DEBUG
                //While debugging this section is used.
                Service1 myService = new Service1();
                myService.onDebug();
                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

            #else
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
                { 
                    new Service1() 
                };
                ServiceBase.Run(ServicesToRun);
            #endif
        }


    static void MainOld()
        {    
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new Service1()
                };
                ServiceBase.Run(ServicesToRun);// Put the body of your old Main method here.          
          
        }
    }
}
