using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Materials
{
    public class MetalMaterial : Material
    {
        public Vector3 Albedo;
        public float Fuzziness = 0.0f;

        static Vector3 Reflect(Vector3 v, Vector3 n)
        {
            return v - 2 * Vector3.Dot(v, n) * n;
        }

        public MetalMaterial(Vector3 albedo, float fuzz = 0.0f)
        {
            Albedo = albedo;
            Fuzziness = fuzz;
        }

        public override bool Scatter(Ray r, RayHit hit, out Vector3 attenuation, out Ray scattered)
        {
            var reflected = Reflect(Misc.Unit(r.Direction), hit.Normal);
            var fuzzFactor = Fuzziness * Misc.RandomInUnitSphere();
            scattered = new Ray(hit.P, reflected + fuzzFactor);
            attenuation = Albedo;
            return (Vector3.Dot(scattered.Direction, hit.Normal) > 0);
        }
    }
}
