using System.Drawing;

namespace Kagochan_DC_Bot.BitmapDraw
{
    public class AppendBitmap
    {
        private Bitmap bitmap;
        public Bitmap AppendBottom(Bitmap bitmap1, Bitmap bitmap2)
        {
            this.bitmap = new Bitmap(bitmap1.Width, bitmap1.Height + bitmap2.Height);
            for (int x = 0; x < bitmap1.Width; x++)
            {
                for (int y = 0; y < bitmap1.Height; y++)
                {
                    this.bitmap.SetPixel(x, y, bitmap1.GetPixel(x, y));
                }
            }
            for (int x = 0; x < bitmap2.Width; x++)
            {
                for (int y = 0; y < bitmap2.Height; y++)
                {
                    this.bitmap.SetPixel(x, y + bitmap1.Height, bitmap2.GetPixel(x, y));
                }
            }
            return this.bitmap;
        }
    }
}
