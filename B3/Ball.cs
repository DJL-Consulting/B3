using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace B3
{
    public enum BallState
    {
        New,
        Hit1,
        Hit2,
        Hit3,
        Hit4,
        Hit5,
        Hit6,
        Dead
    }

    public class Ball
    {
        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }

        public int x, y, xv, yv;

        int pad = 20;

        public int radius = 20;
        public static int minVel = 2;
        public static int maxVel = 20;

        public int V2 => xv * xv + yv * yv; //Velocity squared
        public double V => Math.Sqrt(V2);  //Velocity magnitude


        public BallState state;

        private double degreeToRad = Math.PI / 180; //0.0174532927777778;

        public Ball(bool RandomRadius = false, int rad = 20)
        {
            radius = rad;
            state = BallState.New;

            Random random = new Random();
            y = random.Next(pad, ScreenHeight - pad);
            x = random.Next(pad, ScreenWidth - pad);

            var vel = random.Next(minVel, maxVel);

            double angle = random.Next(0, 359) * degreeToRad;
            xv = (int)MathF.Round((float)(vel * Math.Cos(angle)));
            yv = (int)MathF.Round((float)(vel * Math.Sin(angle)));

            if (Math.Abs(xv) < 3)
                xv = 3;

            if (Math.Abs(yv) < 3)
                yv = 3;

            if (RandomRadius)
                radius = random.Next(10, 100);
        }

        private Color GetColor()
        {
            switch (this.state)
            {
                case BallState.New:
                    return Color.White;
                case BallState.Hit1:
                    return Color.LightSkyBlue;
                case BallState.Hit2:
                    return Color.Orange;
                case BallState.Hit3:
                    return Color.Silver;
                case BallState.Hit4:
                    return Color.Green;
                case BallState.Hit5:
                    return Color.Yellow;
                case BallState.Hit6:
                    return Color.Red;
                default:
                    return Color.Black;
            }

            //switch (this.state)
            //{
            //    case BallState.New:
            //        return Color.White;
            //    case BallState.Hit1:
            //        return Color.Gray;
            //    case BallState.Hit2:
            //        return Color.LightGreen;
            //    case BallState.Hit3:
            //        return Color.Green;
            //    case BallState.Hit4:
            //        return Color.DarkGreen;
            //    case BallState.Hit5:
            //        return Color.GreenYellow;
            //    case BallState.Hit6:
            //        return Color.LightYellow;
            //    case BallState.Hit7:
            //        return Color.Yellow;
            //    case BallState.Hit8:
            //        return Color.Pink;
            //    case BallState.Hit9:
            //        return Color.Red;
            //    case BallState.Hit10:
            //        return Color.DarkRed;
            //    default:
            //        return Color.Black;
            //}
        }

        public void Move(Graphics g)
        {
            if ((x - radius < pad && xv < 0) || (x + radius > ScreenWidth - pad && xv > 0))
                xv = -xv;

            if ((y - radius < pad && yv < 0) || (y + radius > ScreenHeight - pad && yv > 0))
                yv = -yv;

            x += xv;
            y += yv;

            var pen = new Pen(GetColor(), 2);
            var brush1 = new HatchBrush(HatchStyle.DiagonalBrick, GetColor(), Color.Black);  //.Shingle
            g.DrawCircle(pen, x, y, radius);
            g.FillCircle(brush1, x, y, radius); // myBrush, centerX, centerY, radius);
        }

        public bool IsTouching(Ball other)
        {
            return (Math.Sqrt(Math.Pow((other.x - this.x), 2) + Math.Pow((other.y - this.y), 2)) <= (this.radius + other.radius));
        }

        public void Collide(Ball other)
        {
            var add90 = Math.PI / 2;
            var twoPi = Math.PI * 2.0;

            var thisOldxv = xv;
            var thisOldyv = yv;

            var thisOldV = V;

            var thisThetaV = add90 - Math.Atan2(yv, xv); //Velocity Direciton

            var xc = -(this.x <= other.x ? this.x - other.x : other.x - this.x);
            var yc = (this.y <= other.y ? this.y - other.y : other.y - this.y);

            xc = (other.x - this.x);
            yc = (this.y - other.y);

            var thisThetaA = add90 - Math.Atan2(yc, xc); // Angle of ball intersection

            var thisThetaVI = thisThetaA + thisThetaV;

            var vThisParallel = xv * Math.Sin(thisThetaA) + yv * Math.Cos(thisThetaA); //V * Math.Cos(thisThetaVI);
            var vThisTangent = xv * Math.Cos(thisThetaA) + yv * Math.Sin(thisThetaA);  //V * Math.Sin(thisThetaVI);

            var thisThetaOpposite = Math.PI - thisThetaA;

            //Other ball
            var oldxv = other.xv;
            var oldyv = other.yv;

            var oldV = V;

            var thetaV = add90 - Math.Atan2(other.yv, other.xv); //Velocity Direciton

            var thetaA = Math.PI + thisThetaA;  // Opposite direction 

            var thetaVI = thetaA + thetaV;

            var vParallel = other.xv * Math.Sin(thetaA) + other.yv * Math.Cos(thetaA); //other.V * Math.Cos(thetaVI);
            var vTangent = other.xv * Math.Cos(thetaA) + other.yv * Math.Sin(thetaA); //other.V * Math.Sin(thetaVI);

            var thetaOpposite = Math.PI - thetaA;

            // Assign calculated velcocities:

            xv = (int)Math.Round(vParallel * Math.Cos(thetaOpposite) + vThisTangent * Math.Sin(thisThetaV));
            yv = (int)Math.Round(vParallel * Math.Sin(thetaOpposite) + vThisTangent * Math.Cos(thisThetaV));

            state++;

            other.xv = (int)Math.Round(vThisParallel * Math.Cos(thisThetaOpposite) + vTangent * Math.Sin(thetaV));
            other.yv = (int)Math.Round(vThisParallel * Math.Sin(thisThetaOpposite) + vTangent * Math.Cos(thetaV));

            other.state++;

            return;
        }

        public void CollideOld(Ball other)
        {
            var add90 = Math.PI / 2;
            var twoPi = Math.PI * 2.0;            

            var oxv = xv;
            var oyv = yv;

            var oldV = V;

            var thetaV = add90 - Math.Atan2(yv, xv); //Velocity Direciton

            var xc = -(this.x <= other.x ? this.x - other.x : other.x - this.x);
            var yc = (this.y <= other.y ? this.y - other.y : other.y - this.y);

            xc = (other.x - this.x);
            yc = (this.y - other.y);

            var thetaA = add90 - Math.Atan2(yc, xc); // Angle of ball intersection

            var thetaVI = thetaA + thetaV;

            var vParallel = V * Math.Cos(thetaVI);
            var vTangent = V * Math.Sin(thetaVI);

            var thetaOpposite = Math.PI - thetaV;

            yv = (int)Math.Round(-vParallel * Math.Cos(thetaOpposite) + vTangent * Math.Sin(thetaOpposite));
            xv = (int)Math.Round(vParallel * Math.Sin(thetaOpposite) + vTangent * Math.Cos(thetaOpposite));

            state++;
            
            return;

            //var thetaA = Math.Atan2(yc, xc); // Angle of ball intersection

            //var thetaVI = Math.PI - (thetaV + thetaA);

            ////var thetaVI = Math.Acos((xv * Math.Cos(thetaA) + yv * Math.Sin(thetaA)) / V);

            //var mtFac = Math.Abs(Math.Sin(thetaVI)) * .6;  //theta VI?

            //var totV = V + other.V;
            //var totv2 = V2 + other.V2;

            //var myNewV = V - ((V - other.V) * mtFac);

            //xv = (int)Math.Round(oldV * Math.Cos(thetaVI));
            //yv = (int)Math.Round(oldV * Math.Sin(thetaVI));

            //var nv = Math.Sqrt(xv * xv + yv * yv);

            //state++;

            //----------------------------------OLD REM

            //return;

            //var cosThetaVI = (xv * Math.Cos(thetaA) + yv * Math.Sin(thetaA)) / V;

            //var vParallel = cosThetaVI * V;

            //vParallel = -vParallel;  //Reverse it, this gets reflected back

            //var vNormal = Math.Sqrt(V * V - vParallel * vParallel);

            //var newxv = (vParallel * Math.Sin(thetaA) + vNormal * Math.Sin(thetaA - add90));
            //var newyv = (vNormal * Math.Cos(thetaA) + vParallel * Math.Cos(thetaA - add90));

            //var thetaNew = Math.Atan2(newyv, newxv);

            //xv = (int)MathF.Round((float)(V * Math.Cos(thetaNew)));
            //yv = (int)MathF.Round((float)(Math.Sqrt(v2 - xv * xv)));


            //var V1 = (int)Math.Abs(Math.Pow(Math.Floor(newxv), 2) + Math.Pow(Math.Floor(newyv), 2) - v2);
            //var V2 = (int)Math.Abs(Math.Pow(Math.Floor(newxv), 2) + Math.Pow(Math.Ceiling(newyv), 2) - v2);
            //var V3 = (int)Math.Abs(Math.Pow(Math.Ceiling(newxv), 2) + Math.Pow(Math.Floor(newyv), 2) - v2);
            //var V4 = (int)Math.Abs(Math.Pow(Math.Ceiling(newxv), 2) + Math.Pow(Math.Ceiling(newyv), 2) - v2);

            //if (V1 < V2 && V1 < V3 && V1 < V4)
            //{
            //    xv = (int)Math.Floor(newxv);
            //    yv = (int)Math.Floor(newyv);
            //}
            //else if (V2 < V1 && V2 < V3 && V2 < V4)
            //{
            //    xv = (int)Math.Floor(newxv);
            //    yv = (int)Math.Ceiling(newyv);
            //}
            //else if (V3 < V1 && V3 < V2 && V3 < V4)
            //{
            //    xv = (int)Math.Ceiling(newxv);
            //    yv = (int)Math.Floor(newyv);
            //}
            //else 
            //{
            //    xv = (int)Math.Ceiling(newxv);
            //    yv = (int)Math.Ceiling(newyv);
            //}


            //var vNormal = xv * Math.Cos(thetaA) + yv * Math.Cos(thetaA + add90);
            //var vLine = xv * Math.Sin(thetaA) + yv * Math.Sin(thetaA + add90);

            //vNormal -= vNormal;

            //xv = (int)MathF.Round((float)(vLine * Math.Sin(thetaA)));
            //yv = (int)MathF.Round((float)(vNormal * Math.Cos(thetaA)));

            //var ov = V;

            //var nv2 = Math.Sqrt(xv * xv + yv * yv);

            ////xv = -xv;
            ////yv = -yv;
            //state++;
        }


    }
}
