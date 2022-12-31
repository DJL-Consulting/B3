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


        int hatchWidht = 20;

        List<Ball> BallList = new List<Ball>();

        int x = 200;
        int y = 200;

        private Point mouseLocation;

        private bool previewMode = false;

        bool isDrawing = false;

        Ball b;

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
            //Temp rem below
            Cursor.Hide(); 
            TopMost = true;

            Ball.ScreenHeight = this.Height;
            Ball.ScreenWidth = this.Width;
        }

        private void MakeBalls()
        {
            BallList = new List<Ball>();

            for (int x = 0; x < 25; x++)
            {
                BallList.Add(new Ball(false));
            }

            timer1.Interval = 10;
            timer1.Enabled = true;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            var ballsInPlay = BallList.Where(b => b.state != BallState.Dead).ToList();

            if (ballsInPlay.Count < 2)
                MakeBalls();

            foreach (var ball in ballsInPlay)
            {
                ball.Move(e.Graphics);
            }

            e.Graphics.Dispose();
            isDrawing = false;
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

            isDrawing = true;
            var ballsInPlay = BallList.Where(b => b.state != BallState.Dead).ToList();
            foreach (var ball in ballsInPlay)
            {
                if (ballsInPlay.Any(b => b.IsTouching(ball) && b != ball))
                    ball.Collide();
            }

            this.Refresh();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Ball.ScreenHeight = this.Height;
            Ball.ScreenWidth = this.Width;
        }

        private void B3Saver_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!previewMode)
                Application.Exit(); 
        }
    }
}