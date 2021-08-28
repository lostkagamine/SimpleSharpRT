using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public class ThreadSafeRandom
    {
        private static readonly Random _global = new Random();
        [ThreadStatic] private static Random _local;

        public double NextDouble()
        {
            if (_local == null)
            {
                double seed;
                lock (_global)
                {
                    seed = _global.NextDouble();
                }
                _local = new Random((int)seed * 100000);
            }

            return _local.NextDouble();
        }
    }

    public class Misc
    {
        public static float HitSphere(Vector3 center, float radius, Ray r)
        {
            Vector3 oc = r.Origin - center;
            var a = Vector3.Dot(r.Direction, r.Direction);
            var b = 2.0f * Vector3.Dot(oc, r.Direction);
            var c = Vector3.Dot(oc, oc) - radius * radius;
            var discriminant = b * b - 4 * a * c;
            
            if (discriminant < 0)
            {
                return -1.0f;
            } else
            {
                return (-b - MathF.Sqrt(discriminant)) / (2.0f * a);
            }
        }

        public static ThreadSafeRandom rand = new();

        public static float RandomFloat()
        {
            return (float)rand.NextDouble();
        }

        public static float RandomFloat(float a, float b)
        {
            return (RandomFloat() * (b - a)) + a;
        }

        public static Vector3 RandomVec3()
        {
            return new Vector3(
                RandomFloat(),
                RandomFloat(),
                RandomFloat());
        }

        public static Vector3 RandomVec3(float a, float b)
        {
            return new Vector3(
                RandomFloat(a, b),
                RandomFloat(a, b),
                RandomFloat(a, b));
        }

        public static Vector3 RandomInUnitSphere()
        {
            while (true)
            {
                var v = RandomVec3(-1, 1);
                if (v.LengthSquared() >= 1) continue;
                return v;
            }
        }

        public static Vector3 RandomUnitVector()
        {
            var t = RandomInUnitSphere();
            return t / t.Length();
        }

        public static bool CloseToZeroVec3(Vector3 vec)
        {
            var s = 1e-8;
            return
                (MathF.Abs(vec.X) < s) &&
                (MathF.Abs(vec.Y) < s) &&
                (MathF.Abs(vec.Z) < s);
        }

        public static Vector3 Unit(Vector3 a)
        {
            return a / a.Length();
        }

        public static float DegRad(float deg)
        {
            return deg * (MathF.PI / 180f);
        }
    }
}
