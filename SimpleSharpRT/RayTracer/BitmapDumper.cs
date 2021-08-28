using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RayTracer
{
    public class BitmapDumper
    {
        public static Bitmap Dump(Image i)
        {
            var bmp = new Bitmap(i.Width, i.Height);

            Console.WriteLine("Dumping bitmap");
            for (var y=0; y<i.Height; y++)
            {
                for (var x=0; x<i.Width; x++)
                {
                    var gaming = i.Data[y, x].Normalise();
                    var col = Color.FromArgb(255,
                        (int)(gaming.Red * 255),
                        (int)(gaming.Green * 255),
                        (int)(gaming.Blue * 255));
                    bmp.SetPixel(x, (i.Height - y) - 1, col);
                }
            }
            Console.WriteLine("Done!");

            return bmp;
        }
    }
}
