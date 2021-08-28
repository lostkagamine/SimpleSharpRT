using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public class Sphere : Hittable
    {
        public Vector3 Center = new Vector3(0, 0, -1);
        public float Radius = 0.5f;
        public Material Material;

        public Sphere(Vector3 c, float r, Material m)
        {
            Center = c;
            Radius = r;
            Material = m;
        }

        public override bool Hit(Ray r, float tMin, float tMax, out RayHit hit)
        {
            var oc = r.Origin - Center;
            var a = r.Direction.LengthSquared();
            var halfB = Vector3.Dot(oc, r.Direction);
            var c = oc.LengthSquared() - Radius * Radius;

            var discriminant = halfB * halfB - a * c;
            if (discriminant < 0)
            {
                hit = new RayHit();
                return false;
            }

            var sqrtD = MathF.Sqrt(discriminant);
            var root = (-halfB - sqrtD) / a;
            if (root < tMin || tMax < root)
            {
                root = (-halfB + sqrtD) / a;
                if (root < tMin || tMax < root)
                {
                    hit = new RayHit();
                    return false;
                }
            }

            hit = new RayHit();
            hit.T = root;
            hit.P = r.At(hit.T);
            hit.Material = Material;
            var outwardNormal = (hit.P - Center) / Radius;
            hit.SetFaceNormal(r, outwardNormal);

            return true;
        }
    }
}
