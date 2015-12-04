using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gabriel.cat
{
	public class Imagen
	{

		/// <summary>
		/// Recorta una imagen en formato Bitmap
		/// </summary>
		/// <param name="localizacion">localizacion de la esquina izquierda de arriba</param>
		/// <param name="tamaño">tamaño del rectangulo</param>
		/// <param name="bitmapARecortar">bitmap para recortar</param>
		/// <returns>bitmap resultado del recorte</returns>
		public static Bitmap Recortar(Point localizacion, Size tamaño, Bitmap bitmapARecortar)
		{

			Rectangle rect = new Rectangle(localizacion.X, localizacion.Y, tamaño.Width, tamaño.Height);
			Bitmap cropped = bitmapARecortar.Clone(rect, bitmapARecortar.PixelFormat);
			return cropped;

		}
		public static Bitmap Escala(Bitmap imgAEscalar, decimal escala)
		{
			return Resize(imgAEscalar,new Size(Convert.ToInt32(imgAEscalar.Size.Width*escala),Convert.ToInt32(imgAEscalar.Size.Height*escala)));
		}
		public static Bitmap Resize(Bitmap imgToResize, Size size)
		{
			try
			{
				Bitmap b = new Bitmap(size.Width, size.Height);
				using (Graphics g = Graphics.FromImage((Image)b))
				{
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
					g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
				}
				return b;
			}
			catch { }
			return imgToResize;
		}
		public static Bitmap Convierte(byte[] img)
		{
			if (img == null)
			{
				return null;
			}
			else
			{
				try
				{
					MemoryStream ms = new MemoryStream(img);
					return new Bitmap(ms);
				}
				catch { return null; }

			}
		}

		public static byte[] Convierte(Bitmap img)
		{
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
			byte[] buffer = new Byte[ms.Length];
			return ms.ToArray();
		}
		//Tratar Pixeles
		#region Pixels
		public static Color ToRed(Color pixel)
		{
			return Color.FromArgb(pixel.R, 0, 0);
		}
		public static Color ToBlue(Color pixel)
		{
			return Color.FromArgb(0, 0, pixel.B);
		}
		public static Color ToGreen(Color pixel)
		{
			return Color.FromArgb(0, pixel.G, 0);
		}
		public static Color ToEscalaGrises(Color pixel)
		{
			int v = (int)(0.2126 * pixel.R + 0.7152 * pixel.G + 0.0722 * pixel.B);
			return Color.FromArgb(v, v, v);
		}
		public static Color ToInvertit(Color pixel)
		{
			return Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
		}
		public static Color ToSepia(Color pixel)
		{
			int r = (int)(pixel.R * 0.393 + pixel.G * 0.769 + pixel.B * 0.189);
			int g = (int)(pixel.R * 0.349 + pixel.G * 0.686 + pixel.B * 0.168);
			int b = (int)(pixel.R * 0.272 + pixel.G * 0.534 + pixel.B * 0.131);
			if (r > 255) r = 255;
			if (g > 255) g = 255;
			if (b > 255) b = 255;
			return Color.FromArgb(r, g, b);
		}
		#endregion




		
	}
}
