//==================================================================================
//  ArduinoPulse Connection v2 
//      18/12/2020
//      ThibCott juliDeva
//      Thibaut Cotture
//      EPTM - HVS (Sion)
//      C#
//      Detection de l arduino et affichage dans la console des resultat du capteur 
//==================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        static SerialPort serialPort;
        static void Main(string[] args)
        {
            //Inisialisation des variables
            int iPort = 0;
            int iTemp = 0;
            bool bPort = false;

            //Conncetion 

            while (bPort == false)
            {
                //teste si le port COM fonctionne et si il n est pas vide 
                try
                {
                    serialPort = new SerialPort();
                    serialPort.PortName = "COM" + Convert.ToString(iPort);
                    serialPort.BaudRate = 9600;
                    serialPort.Open();
                    string t = serialPort.ReadExisting();
                    if (string.IsNullOrEmpty(t))
                    {
                        Console.Write(iPort);
                        bPort = false;
                        Console.WriteLine("vide");
                        //Console.WriteLine(t);

                        iPort++;
                    }
                    else 
                    {
                        bPort = true;
                        Console.WriteLine("arduino détécter");
                        Console.WriteLine(iPort);
                    }
                }
                catch
                {
                    bPort = false;
                    Console.WriteLine(iPort);
                    iPort++;
                }

                iTemp++;
                 //c'est quoi 50?
                if (iTemp >= 50)
                {
                    Console.Write("press key for continue : ");
                    Console.ReadKey();
                    Console.WriteLine();
                    iTemp = 0;
                }
            }

            Console.WriteLine("-----------------");

            //affichage dans la console

            while (true)
            {
                string a = serialPort.ReadExisting();
                Console.WriteLine(a);
                Thread.Sleep(200);
            }
        }
    }
}
