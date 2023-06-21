using System;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public class Boid
    {
        public double X;
        public double Y;
        public double Xspeed;
        public double Yspeed;

        public Boid(double width, double height, Random rand_point)
        {
            X = rand_point.NextDouble() * width;
            Y = rand_point.NextDouble() * height;
            Xspeed = (rand_point.NextDouble() - 0.35);
            Yspeed = (rand_point.NextDouble() - 0.35);
        }

        //Заставляем боидов двигаться быстрее, чем некоторая минимальная скорость, и медленнее, чем некоторая максимальная скорость.
        public void MoveForward(double minSpeed = 0.8, double maxSpeed = 3)
        {
            X += Xspeed;
            Y += Yspeed;

            var speed = GetSpeed();
            if (speed > maxSpeed)
            {
                Xspeed = (Xspeed / speed) * maxSpeed;
                Yspeed = (Yspeed / speed) * maxSpeed;
            }
            else if (speed < minSpeed)
            {
                Xspeed = (Xspeed / speed) * minSpeed;
                Yspeed = (Yspeed / speed) * minSpeed;
            }
            //вслучии деления на ноль
            if (double.IsNaN(Xspeed))
                Xspeed = 0;
            if (double.IsNaN(Yspeed))
                Yspeed = 0;
        }
        //вычисление угла между направлением движения.
        public double GetAngle()
        {
            if (double.IsNaN(Xspeed) || double.IsNaN(Yspeed))
                return 0;
            if (Xspeed == 0 && Yspeed == 0)
                return 0;

            double angle = Math.Atan(Yspeed / Xspeed) * 180 / Math.PI - 90;
            if (Xspeed < 0 || Yspeed < 0)
                angle += 180;
            return angle;
        }

        public double GetSpeed()
        {
            return Math.Sqrt(Xspeed * Xspeed + Yspeed * Yspeed);
        }

        public double GetDistance(Boid otherBoid)
        {
            double dX = otherBoid.X - X;
            double dY = otherBoid.Y - Y;
            double dist = Math.Sqrt(dX * dX + dY * dY);
            return dist;
        }
    }
}
