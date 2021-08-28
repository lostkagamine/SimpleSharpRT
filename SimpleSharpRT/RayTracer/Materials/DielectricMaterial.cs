using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Materials
{
    public class DielectricMaterial : Material
    {
        static Vector3 Reflect(Vector3 v, Vector3 n)
        {
            return v - 2 * Vector3.Dot(v, n) * n;
        }

        static Vector3 Refract(Vector3 uv, Vector3 n, float etaiOverEtat)
        {
            var cosTheta = MathF.Min(Vector3.Dot(-uv, n), 1.0f);
            var rOutPerp = etaiOverEtat * (uv + cosTheta * n);
            var rOutParallel = -MathF.Sqrt(MathF.Abs(1f - rOutPerp.LengthSquared())) * n;
            return rOutPerp + rOutParallel;
        }

        static float Reflectance(float cos, float refIdx)
        {
            var r0 = (1 - refIdx) / (1 + refIdx);
            r0 *= r0;
            return r0 + (1 - r0) * MathF.Pow((1 - cos), 5);
        }

        public float IndexOfRefraction = 1.0f;

        public DielectricMaterial(float index)
        {
            IndexOfRefraction = index;
        }

        public override bool Scatter(Ray r, RayHit hit, out Vector3 attenuation, out Ray scattered)
        {
            attenuation = Vector3.One;
            var refracRatio = hit.FrontFace ? (1f / IndexOfRefraction) : IndexOfRefraction;

            var unitDirection = Misc.Unit(r.Direction);

            var cosTheta = MathF.Min(Vector3.Dot(-unitDirection, hit.Normal), 1.0f);
            var sinTheta = MathF.Sqrt(1.0f - cosTheta * cosTheta);

            var cannotRefract = refracRatio * sinTheta > 1.0f;

            Vector3 direction;

            if (cannotRefract ||
                Reflectance(cosTheta, refracRatio) > Misc.RandomFloat())
            {
                direction = Reflect(unitDirection, hit.Normal);
            } else
            {
                direction = Refract(unitDirection, hit.Normal, refracRatio);
            }

            scattered = new Ray(hit.P, direction);
            return true;
        }
    }
}
