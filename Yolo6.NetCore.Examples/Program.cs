using System;
using System.Collections.Generic;
using System.Drawing;

namespace Yolo6.NetCore.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            // Single frame / image example.
            //CpuImageExample.Run();

            // Web camera example.
           CpuCameraExample.Run();
        }
    }
}
