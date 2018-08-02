using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using IMG = System.Drawing.Image;

//Access Colors with GUI.Primary, etc
//Use GUI.LoadGUI() to load the ColorMatrix from the games UI

namespace CRYPTIC
{
    public static class GUI
    {
        private static readonly string _GUI = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Frontier Developments\\Elite Dangerous\\Options\\Graphics\\GraphicsConfigurationOverride.xml";
        private static readonly float Multiplier = 2.5f;
        private static readonly double WIDTH = System.Windows.SystemParameters.PrimaryScreenWidth;
        private static readonly double HALF_WIDTH = System.Windows.SystemParameters.PrimaryScreenWidth / 2;

        public static System.Windows.Media.Color Primary = Colors.White;
        public static System.Windows.Media.Color Secondary = Colors.Aqua;
        public static System.Windows.Media.Color Hostile = Colors.Red;
        public static System.Windows.Media.Color Friendly = Colors.Lime;

        private static ColorMatrix colorMatrix;

        private static string DIR()
        {
            string pathToUse = _GUI;
            if (Properties.Settings.Default.GraphicsLocation.Length > 0)
                pathToUse = Properties.Settings.Default.GraphicsLocation;
            return pathToUse;
        }

        public static DropShadowEffect Glow(System.Windows.Media.Color c)
        {
            return new DropShadowEffect
            {
                BlurRadius = 10,
                ShadowDepth = 0,
                Opacity = 1,
                Color = c
            };
        }

        public static void TextBlockSetup(TextBlock t, float f)
        {
            t.Width = 300;
            t.Height = 40;
            t.HorizontalAlignment = HorizontalAlignment.Center;
            t.VerticalAlignment = VerticalAlignment.Top;
        }

        public static void SetupUI(System.Windows.Controls.Image img)
        {
            img.Width = 381;
            img.Height = 69;
            img.Margin = new Thickness(0, 130, 0, 0);
            img.HorizontalAlignment = HorizontalAlignment.Center;
            img.VerticalAlignment = VerticalAlignment.Top;
        }

        public static void Load()
        {
            if (File.Exists(DIR()) == false) { return; }

            XmlDocument graphicsOptions = new XmlDocument();
            try
            {
                graphicsOptions.Load(DIR());
            }
            catch (Exception e)
            {
                return;
            }
            string jsonFromXml = JsonConvert.SerializeXmlNode(graphicsOptions);
            JObject obj = JObject.Parse(jsonFromXml);

            try
            {
                float[] rValues = MatrixValues(obj["GraphicsConfig"]["GUIColour"]["Default"]["MatrixRed"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                float[] gValues = MatrixValues(obj["GraphicsConfig"]["GUIColour"]["Default"]["MatrixGreen"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                float[] bValues = MatrixValues(obj["GraphicsConfig"]["GUIColour"]["Default"]["MatrixBlue"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

                float[][] colorMatrixElements = {
                    new float[] { rValues[0] * Multiplier, rValues[1] * Multiplier, rValues[2] * Multiplier,  0, 0},
                    new float[] { gValues[0] * Multiplier, gValues[1] * Multiplier, gValues[2] * Multiplier,  0, 0},
                    new float[] { bValues[0] * Multiplier, bValues[1] * Multiplier, bValues[2] * Multiplier,  0, 0},
                    new float[] { 0, 0, 0, 1, 0},
                    new float[] { 0, 0, 0, 0, 1 }
                };

                colorMatrix = new ColorMatrix(colorMatrixElements);

                Primary = ApplyColorMatrix(colorMatrix, ColorTranslator.FromHtml("#e94d04"), new Bitmap(1, 1));
                Secondary = ApplyColorMatrix(colorMatrix, ColorTranslator.FromHtml("#5ba2ea"), new Bitmap(1, 1));
            }
            catch (Exception e)
            {
                return;
            }
        }

        private static System.Windows.Media.Color ApplyColorMatrix(ColorMatrix colorMatrix, System.Drawing.Color c, Bitmap bmp)
        {
            ImageAttributes imgAttributes = new ImageAttributes();
            imgAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            Graphics gfx = Graphics.FromImage(bmp);
            SolidBrush brush = new SolidBrush(c);

            gfx.FillRectangle(brush, 0, 0, 1, 1);
            gfx.DrawImage(bmp, new System.Drawing.Rectangle(0, 0, 1, 1), 0, 0, 1, 1, GraphicsUnit.Pixel, imgAttributes);
            return ConvertColor(bmp.GetPixel(0, 0));
        }

        public static void ApplyColorMatrixToImage(System.Windows.Controls.Image gfx)
        {
            Bitmap original = new Bitmap(Properties.Resources.GUI_0);

            ImageAttributes imgAttributes = new ImageAttributes();
            imgAttributes.SetColorMatrix(colorMatrix);

            Graphics g = Graphics.FromImage(original);
            g.DrawImage(original, new System.Drawing.Rectangle(0, 0, original.Width, original.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, imgAttributes);
            g.Dispose();
            gfx.Source = BitmapToImageSource(original);
        }

        private static BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        private static System.Windows.Media.Color ConvertColor(System.Drawing.Color c)
        {
            return System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        private static float[] MatrixValues(string[] values)
        {
            float[] floatArray = new float[3];
            floatArray[0] = float.Parse(values[0]);
            floatArray[1] = float.Parse(values[1]);
            floatArray[2] = float.Parse(values[2]);
            return floatArray;
        }
    }
}
