using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public class HittableList : Hittable
    {
        public List<Hittable> Hittables;

        public HittableList()
        {
            Hittables = new();
        }

        public void Add(Hittable h)
            => Hittables.Add(h);

        public void Clear()
            => Hittables.Clear();

        public override bool Hit(Ray r, float tMin, float tMax, out RayHit hit)
        {
            RayHit tempHit = new RayHit();
            hit = tempHit;
            var hitAnything = false;
            var closestSoFar = tMax;

            foreach (var obj in Hittables)
            {
                if (obj.Hit(r, tMin, closestSoFar, out tempHit))
                {
                    hitAnything = true;
                    closestSoFar = tempHit.T;
                    hit = tempHit;
                }
            }

            return hitAnything;
        }
    }
}
