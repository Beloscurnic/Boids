using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public static class Render
    {
        public static Bitmap RenderRegion(Region region)
        {
            Bitmap bmp = new Bitmap((int)region.Width, (int)region.Height);
            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gfx.Clear(ColorTranslator.FromHtml("#ffffff"));
                for (int i = 0; i < region.Boids.Count(); i++)
                {
                    if (i < 3)
                        RenderBoid(gfx, region.Boids[i], Color.Red);
                    else
                        RenderBoid(gfx, region.Boids[i], Color.Black);
                }
            }
            return bmp;
        }

        private static void RenderBoid(Graphics gfx, Boid boid, Color color)
        {
            var boidOutline = new Point[]
            {
                new Point(-5, 5),
                new Point(5, 7),
                new Point(8, -5),
            };

            using (var brush = new SolidBrush(color))
            {
                gfx.TranslateTransform((float)boid.X, (float)boid.Y);
                gfx.RotateTransform((float)boid.GetAngle());
                gfx.FillClosedCurve(brush, boidOutline);
                gfx.ResetTransform();
            }
        }
    }
}
