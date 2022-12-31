using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace B3
{
    public partial class B3Config : Form
    {
        public B3Config()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void NumBalls_TextChanged(object sender, EventArgs e)
        {

        }

        private void B3Config_Load(object sender, EventArgs e)
        {
            LoadConfig();
        }

        private void LoadConfig()
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


            NumBalls.Text = config.AppSettings.Settings["NumBalls"].Value;
            Radius.Text = config.AppSettings.Settings["Radius"].Value; 
            MinVel.Text = config.AppSettings.Settings["MinVelocity"].Value; 
            MaxVel.Text = config.AppSettings.Settings["MaxVelocity"].Value; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int numBalls;
            if (!int.TryParse(NumBalls.Text, out numBalls))
                numBalls = 20;

            int rad;
            if (!int.TryParse(Radius.Text, out rad))
                rad = 20;

            int minV;
            if (!int.TryParse(MinVel.Text, out minV))
                minV = 2;

            int maxV;
            if (!int.TryParse(MaxVel.Text, out maxV))
                maxV = 20;

            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["NumBalls"].Value = numBalls.ToString();
            config.AppSettings.Settings["Radius"].Value = rad.ToString();
            config.AppSettings.Settings["MinVelocity"].Value = minV.ToString();
            config.AppSettings.Settings["MaxVelocity"].Value = maxV.ToString();

            config.Save(ConfigurationSaveMode.Modified);

            LoadConfig();

            var res = MessageBox.Show("Changes Saved!");

            Application.Exit();
        }
    }
}
