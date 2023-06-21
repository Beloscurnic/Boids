using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsFormsApp1
{
    public class Region
    {
        public readonly double Width;
        public readonly double Height;
        public readonly List<Boid> Boids = new List<Boid>();
        private readonly Random Rand = new Random();
        public Vector Vector;

        public int PredatorCount = 3;

        public Region(double width, double height, int boidCount = 100)
        {

            (Width, Height) = (width, height);
            for (int i = 0; i < boidCount; i++)
                Boids.Add(new Boid(width, height, Rand));
            Vector = new Vector();
        }
        private void BounceOffWalls(Boid boid)
        {
            double pad = 55;
            double turn = 0.7;
            if (boid.X < pad)
                boid.Xspeed += turn;
            if (boid.X > Width - pad)
                boid.Xspeed -= turn;
            if (boid.Y < pad)
                boid.Yspeed += turn;
            if (boid.Y > Height - pad)
                boid.Yspeed -= turn;
        }
        //сплоченость стаи. Изменение направление боидов к центру масс ближайших боидов
        private (double xVel, double yVel) Cohesion(Boid boid, double distance, double power)
        {

            var neighbors = Boids.Where(x => x.GetDistance(boid) < distance);
            double meanX = neighbors.Sum(x => x.X) / neighbors.Count();
            double meanY = neighbors.Sum(x => x.Y) / neighbors.Count();
            return Vector.Multiplication_Scalar(Vector.Subtraction((meanX, meanY), (boid.X, boid.Y)), power);
        }

        //Разделение. Избегать близких боидов 
        private (double xVel, double yVel) Separation(Boid boid, double distance, double power)
        {
            var neighbors = Boids.Where(x => x.GetDistance(boid) < distance);

            (double sumClosenessX, double sumClosenessY) sumClosenessX = (0, 0);
            foreach (var neighbor in neighbors)
            {
                //closeness величина обратно пропорциональна силе отталкивания между боидами:
                //чем ближе боиды, тем сильнее отталкиваются друг от друга.
                double closeness = distance - boid.GetDistance(neighbor);
                var sub = Vector.Subtraction((boid.X, boid.Y), (neighbor.X, neighbor.Y));
                //sumClosenessX - вектор отталкивания
                sumClosenessX = Vector.Addition(sumClosenessX, Vector.Multiplication_Scalar((sub.X, sub.Y), closeness));
            }
            return Vector.Multiplication_Scalar(sumClosenessX, power);
        }

        //Выравнивание. Корректировку скорости, необходимую для приближения к средней скорости и направлению ближайших боидов.
        private (double xVel, double yVel) Align(Boid boid, double distance, double power)
        {
            var neighbors = Boids.Where(x => x.GetDistance(boid) < distance);
            // вычисление средние значения горизонтальной и вертикальной скоростей 
            double meanXvel = neighbors.Sum(x => x.Xspeed) / neighbors.Count();
            double meanYvel = neighbors.Sum(x => x.Yspeed) / neighbors.Count();
            var sub = Vector.Subtraction((meanXvel, meanYvel), (boid.Xspeed, boid.Yspeed));
            return Vector.Multiplication_Scalar(sub, power);
        }

        private (double xVel, double yVel) Predator(Boid boid, double distance, double power)
        {
            (double sumClosenessX, double sumClosenessY) sumClosenessX = (0, 0);
            for (int i = 0; i < PredatorCount; i++)
            {
                Boid predator = Boids[i];
                double distanceAway = boid.GetDistance(predator);
                if (distanceAway < distance)
                {
                    double closeness = distance - distanceAway;
                    var sub = Vector.Subtraction((boid.X, boid.Y), (predator.X, predator.Y));
                    sumClosenessX = Vector.Addition(sumClosenessX, Vector.Multiplication_Scalar((sub.X, sub.Y), closeness));
                }
            }
            return Vector.Multiplication_Scalar(sumClosenessX, power);
        }


    
        private List<Boid> Destroid_Boid(Boid boid, double distance)
        {
            var copyboid = new List<Boid>();        
            for (int i = 0; i < Boids.Count; i++)
            {
                if(i> 3)
                if (boid.GetDistance(Boids[i]) < 40)
                {
                    copyboid.Add(Boids[i]);
                }
            }
            return copyboid;
        }

        public void Advance()
        {
            var list_boid = new List<Boid>();
            int i = 0;
            foreach (var boid in Boids)
            {
                if (i >= 3)
                {
                    (double flockXvel, double flockYvel) = Cohesion(boid, 150, .004);//сплоченость
                    (double alignXvel, double alignYvel) = Align(boid, 70, .005);//Выравнивание. Корректировку скорости
                    (double avoidXvel, double avoidYvel) = Separation(boid, 30, .005);//Разделение. Избегать близких боидов 
                    (double predXvel, double predYval) = Predator(boid, 110, .0003);//Атака
                    boid.Xspeed += flockXvel + avoidXvel + alignXvel + predXvel;
                    boid.Yspeed += flockYvel + avoidYvel + alignYvel + predYval;
                }
                else
                {
                    (double flockXvel, double flockYvel) = Cohesion(boid, 190, .0005);
                    list_boid = Destroid_Boid(boid, 5);
                    boid.Xspeed += flockXvel;
                    boid.Yspeed += flockYvel;
                }
                i++;
            }
            foreach (var boid in list_boid)
            {
                Boids.Remove(boid);
            }
           
            foreach (var boid in Boids)
            {
                boid.MoveForward();
                BounceOffWalls(boid);
            }
        }
    }
}
