using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public class ScanlineTimer
    {
        public Stopwatch Watch;
        public BatchLog Logger;

        int lineCount;
        int line;
        double totalTime;

        public ScanlineTimer(int totalLines)
        {
            Watch = new Stopwatch();
            Logger = new BatchLog();
            lineCount = totalLines;
            line = 0;
        }

        public void Begin()
        {
            Watch.Start();
        }

        public void NextLine()
        {
            var ts = Watch.Elapsed;
            var ms = ts.TotalSeconds * 1000d;
            Watch.Restart();
            Logger.Log($"Rendering... {++line}/{lineCount} scanlines");
            Logger.Log($"Scanline took {ms} ms");
            totalTime += ts.TotalSeconds;
        }

        public void EndRender()
        {
            Watch.Stop();
            Logger.Log("---");
            Logger.Log("Done rendering!");
            Logger.Log("");
            Logger.Log($"Traced {lineCount} lines in {totalTime} seconds.");
            Logger.End();
        }
    }
}
