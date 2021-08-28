using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public class ThreadedScanlineTimer
    {
        //public Stopwatch Watch;
        int lineCount;
        int line;
        int thread;
        double totalTime;

        public ThreadedScanlineTimer(int totalLines, int t)
        {
            //Watch = new Stopwatch();
            //Logger = new BatchLog();
            lineCount = totalLines;
            line = 0;
            thread = t;
        }

        public void Begin()
        {
            //Watch.Start();
        }

        public void NextLine()
        {
            //var ts = Watch.Elapsed;
            //var ms = ts.TotalSeconds * 1000d;
            //Watch.Restart();
            Console.WriteLine($"{thread} > Rendering... {++line}/{lineCount} scanlines");
            //Console.WriteLine($"{thread} > Scanline took {ms} ms");
            //totalTime += ts.TotalSeconds;
        }

        public void EndRender()
        {
            //Watch.Stop();
            Console.WriteLine($"{thread} > Done! Took {totalTime} sec to draw {lineCount} lines.");
            //Logger.End();
        }
    }
}
