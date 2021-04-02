using System.Drawing;

namespace Kagochan_DC_Bot.BitmapDraw
{
    public static class AppendBitmap
    {
        public static Bitmap AppendBottom(Bitmap bitmap1, Bitmap bitmap2)
        {
            Bitmap bitmap3 = new Bitmap(bitmap1.Width, bitmap1.Height + bitmap2.Height);
            for (int x = 0; x < bitmap1.Width; x++)
            {
                for (int y = 0; y < bitmap1.Height; y++)
                {
                    bitmap3.SetPixel(x, y, bitmap1.GetPixel(x, y));
                }
            }
            for (int x = 0; x < bitmap2.Width; x++)
            {
                for (int y = 0; y < bitmap2.Height; y++)
                {
                    bitmap3.SetPixel(x, y + bitmap1.Height, bitmap2.GetPixel(x, y));
                }
            }
            return bitmap3;
        }
    }
}
