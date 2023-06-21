using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace WindowsFormsApp1
{
    public class Vector
    {

        public double X;
        public double Y;
        public double Xspeed;
        public double Yspeed;
        // Vector norm
        public double Norm(double X, double Y)
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        // Vector addition
        public static (double X,double Y) Addition((double X1, double Y1)vector1, (double X2, double Y2) vector2)
        {
            double xnew = vector1.X1 + vector2.X2;
            double ynew = vector1.Y1 + vector2.Y2;
            return (xnew, ynew);
        }

        // Vector subtraction
        public static (double X, double Y) Subtraction((double X1, double Y1) vector1, (double X2, double Y2)vector2)
        {
            double xnew = vector1.X1 - vector2.X2;
            double ynew = vector1.Y1 - vector2.Y2;
            return (xnew, ynew);
        }

        // Multiplication with a scalar
        public static (double X, double Y) Multiplication_Scalar((double X1, double Y1) vector, double Scalar)
        {
            double xnew = vector.X1 * Scalar;
            double ynew = vector.Y1 * Scalar;
            return (xnew, ynew);
        }

        // Division by a scalar
        public static (double X, double Y) Division_Scalar(double X1, double Y1, double Scalar)
        {
            double xnew = X1 / Scalar;
            double ynew = Y1 / Scalar;
            return (xnew, ynew);
        }

        // Vector dot product
        public static double Dot(double X1, double Y1, double X2, double Y2)
        {
            double xnew = X1 * X2;
            double ynew = Y1 * Y2;
            return xnew+ ynew;
        }

        // Vector cross product
        public static double Pseudoscalar_Product(double X1, double Y1, double X2, double Y2)
        {
            double xnew = X1 * Y2;
            double ynew = Y1 * X2;
            return xnew- ynew;
        }
    }
}