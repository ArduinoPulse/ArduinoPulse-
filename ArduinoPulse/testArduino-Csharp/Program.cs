//=====================================================
// ThibCott 30.11.2020
// Projet ArduinoPulse
// app qui va chercher les donnée que l arduino renvoie  et qui cherche le bon port COM
//=====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace testArduino
{
    class Program
    {
        static SerialPort serialPort;
        static void Main(string[] args)
        {
            serialPort = new SerialPort();
            bool bPort = false;
            int iPort = 0;
            while (bPort == false)
            {
                try
                {
                    // Do not initialize this variable here.
                    serialPort.PortName = "COM"+ Convert.ToString(iPort);//Set your board COM
                    serialPort.BaudRate = 9600;
                    serialPort.Open();
                    bPort = true;
                    Console.WriteLine(Convert.ToString(iPort));
                }
                catch
                {
                    bPort = false;
                    iPort++;
                }
            }
            Console.WriteLine("-----------------");
            
            while (true)
            {
                string a = serialPort.ReadExisting();
                Console.WriteLine(a);
                Thread.Sleep(200);
            }
        }
    }
}
