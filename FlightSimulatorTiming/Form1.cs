using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace FlightSimulatorTiming
{
    public partial class Form1 : Form
    {

        private Color tr_color = Color.Transparent;
        System.DateTime TimeNow = DateTime.Now;
        TimeSpan TimeCount = new TimeSpan();

        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(SystemInformation.WorkingArea.Width - this.Width, SystemInformation.WorkingArea.Height - this.Height);
        }      

        private void Form1_Load(object sender, EventArgs e)
        {
            

            this.BackColor = Color.Black;
            this.TransparencyKey = Color.Black;

            tb_timing.Text = "";
            timer1.Enabled = true;
            timer1.Interval = 1000;
            timer1.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {           

            TimeCount = DateTime.Now - TimeNow;
            tb_timing.Text = string.Format("{0:00}:{1:00}:{2:00}", TimeCount.Hours, TimeCount.Minutes, TimeCount.Seconds);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Stop();
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int Left;
            public int Right;
            public int Top;
            public int Bottom;
        }
        [DllImport("dwmapi.dll", PreserveSig = false)]
        static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);
        [DllImport("dwmapi.dll", PreserveSig = false)]
        static extern bool DwmIsCompositionEnabled();

        protected override void OnLoad(EventArgs e)
        {
            if (DwmIsCompositionEnabled())
            {
                MARGINS m = new MARGINS();
                m.Right = m.Left = m.Top = this.Width + this.Height;
                DwmExtendFrameIntoClientArea(this.Handle, ref m);
            }
            base.OnLoad(e);
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            if (DwmIsCompositionEnabled())
            {
                e.Graphics.Clear(Color.Black);
            }
        }



        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int wndproc);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        public const int GWL_STYLE = -16;
        public const int WS_DISABLED = 0x8000000;

        public static void SetControlEnabled(Control c, bool enabled)
        {
            if (enabled)
            { SetWindowLong(c.Handle, GWL_STYLE, (~WS_DISABLED) & GetWindowLong(c.Handle, GWL_STYLE)); }
            else
            { SetWindowLong(c.Handle, GWL_STYLE, WS_DISABLED | GetWindowLong(c.Handle, GWL_STYLE)); }
        }

    }
}
