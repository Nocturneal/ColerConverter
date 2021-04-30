using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorConverter {
    internal static class ColorExtensions {

        /// <summary>
        /// Creates color with corrected brightness.
        /// </summary>
        /// <param name="color">Color to correct.</param>
        /// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1. 
        /// Negative values produce darker colors.</param>
        /// <returns>
        /// Corrected <see cref="Color"/> structure.
        /// </returns>
        public static System.Windows.Media.Color ChangeColorBrightness(this System.Windows.Media.Color color, float correctionFactor) => color.ConvertColor().ChangeColorBrightness(correctionFactor).ConvertColor();
        public static Color ChangeColorBrightness(this Color color, float correctionFactor) {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0) {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }

        /// <summary>
        /// Automatically select a light/dark color value based on lumanance. 
        /// (Used mostly for text color.)
        /// </summary>
        public static System.Windows.Media.Color ContrastColor(this System.Windows.Media.Color c) => c.ConvertColor().ContrastColor().ConvertColor();
        public static Color ContrastColor(this Color c) {
            double luma = ((0.299 * c.R) + (0.587 * c.G) + (0.114 * c.B)) / 255;
            return luma > 0.5 ? Color.Black : Color.White;
        }

        /// <summary>
        /// Convert between System.Drawing.Color (WinForms) and Windows.Media.Color (WPF).
        /// </summary>
        public static Color ConvertColor(this System.Windows.Media.Color wpfColor) {
            return Color.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
        }
        public static System.Windows.Media.Color ConvertColor(this Color formsColor) {
            return System.Windows.Media.Color.FromArgb(formsColor.A, formsColor.R, formsColor.G, formsColor.B);
        }
    }
}
