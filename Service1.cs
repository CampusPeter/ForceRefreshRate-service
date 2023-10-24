using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;



namespace ForceRefreshRateService
{

    public partial class Service1 : ServiceBase
    {

        [DllImport("user32.dll")]
        public static extern int ChangeDisplaySettingsEx(string lpszDeviceName, ref DEVMODE devMode, IntPtr hwnd, int dwflags, IntPtr lParam);
        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            private const int CCHDEVICENAME = 32;
            private const int CCHFORMNAME = 32;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            public string dmDeviceName;
            public Int16 dmSpecVersion;
            public Int16 dmDriverVersion;
            public Int16 dmSize;
            public Int16 dmDriverExtra;
            public Int32 dmFields;
            public Int16 dmPositionX;
            public Int16 dmPositionY;
            public Int32 dmDisplayOrientation;
            public Int32 dmDisplayFixedOutput;
            public Int32 dmColor;
            public Int32 dmDuplex;
            public Int32 dmYResolution;
            public Int32 dmTTOption;
            public Int32 dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
            public string dmFormName;
            public Int32 dmLogPixels;
            public Int32 dmBitsPerPel;
            public Int32 dmPelsWidth;
            public Int32 dmPelsHeight;
            public Int32 dmDisplayFlags;
            public Int32 dmDisplayFrequency;
        }


        public void onDebug()
        {
            OnStart(null);
        }

        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        }

        public int numberofMonitors = 0;
        public int autoRefresh = 0;
        public Service1()
        {        
            InitializeComponent();

            System.Timers.Timer monitorCheckTimer = new System.Timers.Timer();
            monitorCheckTimer.Interval = 5000; // Controleer elke 5 seconden
            monitorCheckTimer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            monitorCheckTimer.Start();  
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Elapsed");
 //           // tel aantal monitoren
            int nieuwAantalMonitoren = Screen.AllScreens.Length;


            // Gewijzigd? 
            // SetRefreshRate();
           if (!(numberofMonitors == nieuwAantalMonitoren))
           {
  //              Console.WriteLine("oud aantal : " + numberofMonitors.ToString());
  //              Console.WriteLine("Nieuw aantal " + nieuwAantalMonitoren.ToString());
                
                numberofMonitors = nieuwAantalMonitoren;
                SetRefreshRate();
           }       
           else
            {
                autoRefresh += 1;
                if (autoRefresh > 60) // auto refresh op 5 minuten
                {
                    SetRefreshRate();
                    autoRefresh = 0;
                }
            }
        }
        
        private void SetRefreshRate()
        {
        
           Console.WriteLine("SET");


            // SET 60 hZ

            DEVMODE devMode = new DEVMODE();
            devMode.dmSize = (Int16)Marshal.SizeOf(typeof(DEVMODE));

            if (ChangeDisplaySettingsEx(null, ref devMode, IntPtr.Zero, 0, IntPtr.Zero) == 0)
            {
                devMode.dmDisplayFrequency = 60; // 60Hz

                int result = ChangeDisplaySettingsEx(null, ref devMode, IntPtr.Zero, 0x02, IntPtr.Zero);

                if (result == 0)
                {
//                    Console.WriteLine("Verversingssnelheid is ingesteld op 60Hz.");
                }
                else
                {
 //                   Console.WriteLine("Fout bij het instellen van verversingssnelheid.");
                }
            }
            else
            {
 //               Console.WriteLine("Fout bij het verkrijgen van scherminstellingen.");
            }
        }

        protected override void OnStart(string[] args)
        {

        }

        protected override void OnStop()
        {
        }
    }
}










