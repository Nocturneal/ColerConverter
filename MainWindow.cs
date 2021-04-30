using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColorConverter {
    public partial class MainWindow : Form {
        
        //public const int WM_NCLBUTTONDOWN = 0xA1;
        //public const int HT_CAPTION = 0x2;
        //[DllImport("user32.dll")]
        //public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        //[DllImport("user32.dll")]
        //public static extern bool ReleaseCapture();

        private bool isDragging = false;
        private int cursorOffsetX, cursorOffsetY;

        public MainWindow() {
            InitializeComponent();

            codeLbl.MouseDown += MouseMover; 
            codeLbl.MouseMove += WindowMover;
            codeLbl.MouseUp += OnMouseUp;

            label7.MouseDown += MouseMover;
            label7.MouseMove += WindowMover;
            label7.MouseUp += OnMouseUp;

            Opacity = 0;

            Load += (s, e) => {
                new ClrPreview(Color.White, this).Show();
            };

            Shown += (s, e) => FadeWindow();

            Closing += (s, e) => {
                ClrPreview.instance?.Close();
                FadeWindow(true);
            };
        }

        private void FadeWindow(bool fadeOut = false) {
            if (fadeOut) {
                while (Opacity > 0) {
                    Thread.Sleep(20);
                    Opacity -= 0.05D;
                }
            }
            else {
                Task.Run(() => {
                    while (Opacity < 1) {
                        Thread.Sleep(20);
                        Invoke(new Action(() => Opacity += 0.05D));
                    }
                });
            }
        }

        private void WindowMover(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left && isDragging) {
                Location = new Point(Cursor.Position.X - cursorOffsetX, Cursor.Position.Y - cursorOffsetY);
                ClrPreview.instance?.UpdateLoc();
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e) => isDragging = false;
        private void MouseMover(object sender, MouseEventArgs e) {
            cursorOffsetX = Cursor.Position.X - Location.X;
            cursorOffsetY = Cursor.Position.Y - Location.Y;
            isDragging = true;
            //if (e.Button == MouseButtons.Left) {
            //    ReleaseCapture();
            //    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            //    ClrPreview.instance?.UpdateLoc();
            //}
        }

        private void quitBtn_Click(object sender, EventArgs e) {
            Close();
        }

        private void convertBtn_Clicked(object sender, EventArgs e) {
            var input = hexTextBox.Text;
            if (Regex.IsMatch(input, "^#?([A-Fa-f0-9]{8}|[A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")) {
                if (!input.StartsWith("#")) input = $"#{input}";
                var res = ColorTranslator.FromHtml(input);
                codeOutputBox.Text = $"Color.FromArgb({res.A}, {res.R}, {res.G}, {res.B})";
                ClrPreview.instance?.UpdateColor(res.ConvertColor());
                Refresh();
            }
        }
    }
}
