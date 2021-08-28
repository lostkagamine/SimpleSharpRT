using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RayTracer.Materials;

namespace RayTracer
{
    public struct Job
    {
        public int w;
        public int h;
        public int startX;
        public int startY;
        public int threadNo;
        public ThreadedScanlineTimer logger;
    }

    public struct ThreadHolder
    {
        public Thread t;
        public Job j;
    }

    public class MultithreadedTracer
    {
        public static Image img;
        public static HittableList world;
        static int maxDepth;
        static Camera cam;
        static int width;
        static int height;

        public static int threadCount = Environment.ProcessorCount;

        public static List<ThreadHolder> ThreadPool = new();
        public static bool[] Done;

        static MultithreadedTracer()
        {
            var aspectRatio = 3 / 2f;

            cam = new Camera(
                new Vector3(-2, 2, 1),
                new Vector3(0, 0, -1),
                new Vector3(0, 1, 0),
                30.0f, aspectRatio);

            width = 1280;
            height = (int)(width / aspectRatio);

            img = new Image((int)width, (int)height, 255, 100);

            var green = new Vector3(34 / 255f, 199 / 255f, 70 / 255f);
            var gray = new Vector3(0.1f, 0.1f, 0.1f);
            var centerMat = new LambertianMaterial(green);
            var leftMat = new MetalMaterial(Vector3.One, 0.5f);
            var worldMat = new LambertianMaterial(gray);
            var rightMat = new DielectricMaterial(1.5f);

            world = new HittableList();
            world.Add(new Sphere(new Vector3(0, 0, -1), 0.5f, centerMat));
            world.Add(new Sphere(new Vector3(-1f, 0, -1f), 0.5f, leftMat));
            world.Add(new Sphere(new Vector3(0, -100.5f, -1), 100, worldMat));
            world.Add(new Sphere(new Vector3(1f, 0, -1f), -0.4f, rightMat));
            maxDepth = 20;
        }

        public void Trace()
        {
            Console.WriteLine($"Tracing on {threadCount} threads...");

            Done = new bool[threadCount];
            var division = height / threadCount;

            for (int i=0; i< threadCount; i++)
            {
                Done[i] = false;

                var job = new Job
                {
                    w = width,
                    h = division,
                    startX = 0,
                    startY = division * i,
                    threadNo = i,
                    logger = new ThreadedScanlineTimer(division, i)
                };

                var t = new Thread(TraceOn);

                var th = new ThreadHolder
                {
                    j = job,
                    t = t
                };

                ThreadPool.Add(th);
            }

            Console.WriteLine("Threads built. Starting to trace!");

            var twoHundredFiftySixMegabytes = (long)2.54e+8;
            //GC.TryStartNoGCRegion(twoHundredFiftySixMegabytes);

            foreach (var t in ThreadPool)
            {
                t.t.Start(t.j);
            }

            while (true)
            {
                if (Done.All(e => e))
                    break;
                Thread.Sleep(100);
            }

            //GC.EndNoGCRegion();
        }

        static Vector3 GetColour(Ray r, Hittable world, int depth)
        {
            if (depth <= 0)
                return Vector3.Zero;

            if (world.Hit(r, 0.001f, float.PositiveInfinity, out RayHit h))
            {
                //var target = h.P + h.Normal + Misc.RandomUnitVector();
                //var newRay = new Ray(h.P, target - h.P);

                if (h.Material.Scatter(r, h, out Vector3 attenuation, out Ray scattered))
                    return attenuation * GetColour(scattered, world, depth - 1);

                return Vector3.Zero;
                //return 0.5f * (h.Normal + new Vector3(1f, 1f, 1f));
            }

            var unitDir = r.Direction / r.Direction.Length();
            var t = 0.5f * (unitDir.Y + 1.0f);
            return (1.0f - t) * new Vector3(1, 1, 1) + t * new Vector3(0.5f, 0.7f, 1.0f);
        }

        public static void TraceOn(object data)
        {
            var j = (Job)data;
            TraceOn(j.w, j.h, j.startX, j.startY, j.threadNo, j.logger);
        }

        public static void TraceOn(int w, int h, int startX, int startY, int threadNo, ThreadedScanlineTimer l)
        {
            Console.WriteLine($"Thread {threadNo} tracing..");
            Console.WriteLine($"{threadNo} > ({startX}, {startY}) > ({w}, {startY + h}) [{h}]");
            //l.Begin();
            //for (var j = (startY+h)-1; j >= startY; j--)
            for (var j = startY; j < (startY + h); j++)
            {
                Console.WriteLine($"Thread {threadNo} entering scanline {j}");
                for (int i = startX; i < w; i++)
                {
                    //Console.WriteLine($"thread {threadNo} pixel {i},{j}");
                    var colour = new Vector3(0, 0, 0);
                    for (int x = 0; x < img.DefaultSamples; x++)
                    {
                        var u = (float)(i + Misc.RandomFloat()) / (width - 1);
                        var v = (float)(j + Misc.RandomFloat()) / (height - 1);
                        var r = cam.GetRay(u, v);
                        var c = GetColour(r, world, maxDepth);
                        colour += c;
                    }

                    //lock (img.Data[j, i].Lock)
                    {
                        img.SetPixel(i, j, colour);
                    }
                }
                //l.NextLine();
            }
            //l.EndRender();
            Done[threadNo] = true;
            Console.WriteLine("Done.");
        }
    }
}
