using System;
using System.IO;
using System.Diagnostics;
using System.Numerics;

namespace RayTracer
{
    class Program
    {
        static void Main(string[] args)
        {
            var tr = new MultithreadedTracer();
            tr.Trace();
            var img = MultithreadedTracer.img;

            /*
            var sw = new Stopwatch();
            Console.WriteLine("Dumping ppm data to disk...");
            sw.Start();
            File.WriteAllText("out.ppm", img.ProduceData());
            sw.Stop();
            Console.WriteLine($"It took {sw.Elapsed.TotalSeconds} sec to dump that image to disk.");
            Console.WriteLine("That's a long-ass time tbh.");
            Console.WriteLine("Anyway, goodbye.");
            */

            var t = BitmapDumper.Dump(img);
            t.Save("out.bmp");
            Console.WriteLine("Image saved! Check it out at out.bmp.");
        }
    }
}
