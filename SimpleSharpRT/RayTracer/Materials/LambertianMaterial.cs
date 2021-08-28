using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Materials
{
    public class LambertianMaterial : Material
    {
        public Vector3 Albedo;

        public LambertianMaterial(Vector3 albedo)
        {
            Albedo = albedo;
        }

        public override bool Scatter(Ray r, RayHit hit, out Vector3 attenuation, out Ray scattered)
        {
            var direction = hit.Normal + Misc.RandomUnitVector();

            if (Misc.CloseToZeroVec3(direction))
            {
                direction = hit.Normal;
            }

            scattered = new Ray(hit.P, direction);
            attenuation = Albedo;
            return true;
        }
    }
}
