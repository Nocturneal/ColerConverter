using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VirtexCommonLib.Extensions;

namespace ColorConverter {
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ClrPreview : Window {
        [DllImport("User32.dll")]
        public static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public const int SW_SHOW = 5;

        private bool isAttached = true;
        private volatile bool isBeingDragged = false;
        private Point cursorOffset;

        public static ClrPreview instance;
        private System.Windows.Forms.Control parent;

        public ClrPreview(System.Drawing.Color startColor, System.Windows.Forms.Control parent) {
            if (instance == null) {
                instance = this;
                this.parent = parent;
                InitializeComponent();

                WindowStartupLocation = WindowStartupLocation.Manual;
                PreviewMouseDoubleClick += ClrPreview_PreviewMouseDoubleClick;
                PreviewMouseLeftButtonDown += OnMouseDown;
                PreviewMouseUp += OnMouseUp;

                Loaded += (snd, evt) => {
                    UpdateLoc();
                    UpdateColor(startColor.ConvertColor());
                };

                Closed += (s, e) => instance = null;                
            }
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e) {
            var pos = System.Windows.Forms.Cursor.Position;
            cursorOffset = new Point(pos.X - Left, pos.Y - Top);
            isBeingDragged = true;
            Task.Run(() => {
                while (isBeingDragged) {
                    Thread.Sleep(5);
                    UpdateLoc(new Point(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y));
                }
            });
        }
        private void OnMouseUp(object sender, MouseButtonEventArgs e) => isBeingDragged = false;
        private void ClrPreview_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e) => isAttached = !isAttached;

        public void UpdateLoc(Point cursorPos = default, bool isInvoke = false) {
            if (!isInvoke) Dispatcher?.Invoke(new Action(() => UpdateLoc(cursorPos, true)));
            else {
                if (isAttached) {
                    Left = parent.Location.X + parent.Width + 5;
                    Top = parent.Location.Y;
                }
                else if (isBeingDragged) {
                    Left = (cursorPos.X - cursorOffset.X);
                    Top  = (cursorPos.Y - cursorOffset.Y);
                }
                InvalidateVisual();
            }
        }

        public void UpdateColor(Color newColor, bool isInvoke = false) {
            if (!isInvoke) Dispatcher?.Invoke(new Action(() => UpdateColor(newColor, true)));
            else {
                if (Resources["windowBgColor"] != null) {
                    var windowBgColor = (SolidColorBrush)Resources["windowBgColor"];
                    windowBgColor.Color = Color.FromRgb(newColor.R, newColor.G, newColor.B);
                    windowBgColor.Opacity = int.Parse($"{newColor.A}") / 255D;
                    if (Resources["textColor"] != null) {
                        var textColor = (SolidColorBrush)Resources["textColor"];
                        textColor.Color = windowBgColor.Color.ContrastColor();
                    }
                }
                alphaLbl.Content = $"{newColor.A}";
                redLbl.Content   = $"{newColor.R}";
                greenLbl.Content = $"{newColor.G}";
                blueLbl.Content  = $"{newColor.B}";
                InvalidateVisual();
            }
        }
    }
}
