using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3
{
    public enum BallState
    {
        New,
        Hit1,
        Hit2,
        Hit3,
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

        public BallState state;

        private double degreeToRad = 0.0174532927777778;

        public Ball(bool RandomRadius = false, int rad=20)
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
                    return Color.Green;
                case BallState.Hit2:
                    return Color.Yellow;
                case BallState.Hit3:
                    return Color.Red;
                default:
                    return Color.Black;
            }
        }

        public void Move(Graphics g)
        {
            if ((x - radius < pad && xv < 0) || (x + radius > ScreenWidth - pad && xv > 0))
                xv = -xv;

            if ( (y - radius < pad && yv < 0) || (y + radius > ScreenHeight - pad && yv > 0))
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

        public void Collide()
        {
            xv = -xv;
            yv = -yv;
            state++;
        }


    }
}
