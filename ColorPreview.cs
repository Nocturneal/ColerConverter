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

namespace ColorConverter {
    public partial class ColorPreview : Form {

        [DllImport("User32.dll")]
        public static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public const int SW_SHOW = 5;


        public static ColorPreview instance;
        private Control parent;

        public ColorPreview(Color resultColor, Control parent) {
            if (instance == null) {
                instance = this;
                this.parent = parent;
                InitializeComponent();
                StartPosition = FormStartPosition.Manual;

                Load += (snd, evt) => {
                    UpdateLoc();
                    UpdateColor(resultColor);
                };

                Closed += (s, e) => instance = null;
            }
        }

        public void UpdateLoc() {
            if (InvokeRequired) Invoke(new Action(() => UpdateLoc()));
            else {
                var locX = parent.Location.X + parent.Width;
                var locY = parent.Location.Y;
                Location = new Point(locX, locY);

                SetForegroundWindow(Handle);
                ShowWindow(Handle, SW_SHOW);
                Refresh();
            }
        }

        public void UpdateColor(Color newColor) {
            if (InvokeRequired) Invoke(new Action(() => UpdateColor(newColor)));
            else {
                tableLayoutPanel4.BackColor = Color.FromArgb(newColor.A, newColor.R, newColor.G, newColor.B);
                Opacity = int.Parse($"{newColor.A}") / 255D;
                alphaLbl.Text = $"{newColor.A}";
                redLbl.Text = $"{newColor.R}";
                greenLbl.Text = $"{newColor.G}";
                blueLbl.Text = $"{newColor.B}";
                Refresh();
            }
        }
    }
}
