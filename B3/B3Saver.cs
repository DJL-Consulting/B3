using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace B3
{
    public partial class B3Saver : Form
    {
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        List<Ball> BallList = new List<Ball>();

        int numBalls, radius;

        private Point mouseLocation;

        private bool previewMode = false;

        bool isDrawing = false;

        private DateTime startedTime;

        public B3Saver()
        {
            InitializeComponent();
        }

        public B3Saver(Rectangle Bounds)
        {
            InitializeComponent();
            this.Bounds = Bounds;
        }

        public B3Saver(IntPtr PreviewWndHandle)
        {
            InitializeComponent();

            // Set the preview window as the parent of this window
            SetParent(this.Handle, PreviewWndHandle);

            // Make this a child window so it will close when the parent dialog closes
            // GWL_STYLE = -16, WS_CHILD = 0x40000000
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            // Place our window inside the parent
            Rectangle ParentRect;
            GetClientRect(PreviewWndHandle, out ParentRect);
            Size = ParentRect.Size;
            Location = new Point(0, 0);

            previewMode = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (config.AppSettings.Settings["NumBalls"].Value == null || config.AppSettings.Settings["NumBalls"].Value == "")
                config.AppSettings.Settings["NumBalls"].Value = "20";

            if (config.AppSettings.Settings["Radius"].Value == null || config.AppSettings.Settings["Radius"].Value == "")
                config.AppSettings.Settings["Radius"].Value = "20";

            if (config.AppSettings.Settings["MinVelocity"].Value == null || config.AppSettings.Settings["MinVelocity"].Value == "")
                config.AppSettings.Settings["MinVelocity"].Value = "20";

            if (config.AppSettings.Settings["MaxVelocity"].Value == null || config.AppSettings.Settings["MaxVelocity"].Value == "")
                config.AppSettings.Settings["MaxVelocity"].Value = "20";


            numBalls = int.Parse(config.AppSettings.Settings["NumBalls"].Value);
            radius = int.Parse(config.AppSettings.Settings["Radius"].Value);
            Ball.minVel = int.Parse(config.AppSettings.Settings["MinVelocity"].Value);
            Ball.maxVel = int.Parse(config.AppSettings.Settings["MaxVelocity"].Value);

            //Temp rem below
            //Cursor.Hide();
            //TopMost = true;

            Ball.ScreenHeight = this.Height;
            Ball.ScreenWidth = this.Width;
        }

        private void MakeBalls()
        {
            BallList = new List<Ball>();

            BallList.Add(new Ball { radius = 100, state = BallState.Hit2, x = 200, y = 1001, xv = 20, yv = 0 });

            BallList.Add(new Ball { radius = 100, state = BallState.New, x = 1500, y = 1199, xv = -1, yv = 0 });

            //BallList.Add(new Ball { radius = 100, state = BallState.New, x = 400, y = 1100, xv = 0, yv = 30 });

            //BallList.Add(new Ball { radius = 100, state = BallState.New, x = 400, y = 1200, xv = 0, yv = -5 });

            //for (int x = 0; x < numBalls; x++)
            //{
            //    BallList.Add(new Ball(false, radius));
            //}

            startedTime = DateTime.Now;

            timer1.Interval = 15;
            timer1.Enabled = true;
            //timer1_Tick(null, null);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //MessageBox.Show("Hit");
            //g = e.Graphics;

            MoveBalls(e.Graphics);

            Application.DoEvents();
            //wait(2);
            isDrawing = false;
        }

        public void wait(int milliseconds)
        {
            var waitTimer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;

            // Console.WriteLine("start wait timer");
            waitTimer1.Interval = milliseconds;
            waitTimer1.Enabled = true;
            waitTimer1.Start();

            waitTimer1.Tick += (s, e) =>
            {
                waitTimer1.Enabled = false;
                waitTimer1.Stop();
                // Console.WriteLine("stop wait timer");
            };

            while (waitTimer1.Enabled)
            {
                Application.DoEvents();
            }
        }

        private void MoveBalls(Graphics g)
        {
            var ballsInPlay = BallList.Where(b => b.state != BallState.Dead).ToList();

            if (ballsInPlay.Count < 2)
                MakeBalls();

            foreach (var ball in ballsInPlay)
            {
                ball.Move(g);
            }

            g.DrawCircle(new Pen(Color.Black), 20, 20, 2);

            return;
        }


        private void B3Saver_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseLocation.IsEmpty && !previewMode)
            {
                // Terminate if mouse is moved a significant distance
                if (Math.Abs(mouseLocation.X - e.X) > 5 ||
                    Math.Abs(mouseLocation.Y - e.Y) > 5)
                    Application.Exit();
            }

            // Update current mouse location
            mouseLocation = e.Location;

        }

        private void B3Saver_MouseClick(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }

        private void B3Saver_KeyPress(object sender, KeyEventArgs e)
        {
            if (!previewMode)
                Application.Exit(); 
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isDrawing)
                return;

            if (Math.Abs((DateTime.Now - startedTime).TotalSeconds) > 200)
                MakeBalls();

            isDrawing = true;

            var hitList = new Dictionary<Ball, Ball>();

            var ballsInPlay = BallList.Where(b => b.state != BallState.Dead).ToList();
            foreach (var ball in ballsInPlay)
            {
                hitList.Add(ball, null);
            }
            
            foreach (var ball in ballsInPlay)
            {
                var hitBall = ballsInPlay.FirstOrDefault(b => b.IsTouching(ball) && b != ball);
                if (hitBall != null && hitList[hitBall] == null)
                {
                    ball.Collide(hitBall);
                    hitList[ball] = hitBall;
                }
            }

            this.Refresh();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Ball.ScreenHeight = this.Height;
            Ball.ScreenWidth = this.Width;
        }

        private void B3Saver_Shown(object sender, EventArgs e)
        {
        }

        private void B3Saver_KeyDown(object sender, KeyEventArgs e)
        {
            if (!previewMode)
                Application.Exit();
        }

        private void B3Saver_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!previewMode)
                Application.Exit(); 
        }
    }
}