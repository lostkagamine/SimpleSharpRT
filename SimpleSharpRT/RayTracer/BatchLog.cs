using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public class BatchLog
    {
        public List<string> Buffer;

        public BatchLog()
        {
            Buffer = new();
        }

        public void Start()
        {
            Buffer.Clear();
        }

        public void Log(object a)
        {
            Console.WriteLine(a.ToString());
            Buffer.Add(a.ToString());
        }

        public void End()
        {
            /*
            var s = "";
            foreach (var a in Buffer)
            {
                s += $"{a}\n";
            }
            */
        }
    }
}
