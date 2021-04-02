using System;
using System.Drawing;

namespace Kagochan_DC_Bot.BitmapDraw
{
    public class Draw
    {
        public PointF[] DrawBorder(Bitmap bitmap, int penThicknes)
        {
            PointF[] borderPoints = null;

            if (penThicknes % 2 == 0)
            {
                borderPoints = new PointF[]
                {
                    // Left
                    new Point(0 + penThicknes / 2, 0),
                    new Point(0 + penThicknes / 2, bitmap.Height),
                    // Top
                    new Point(0, 0 + penThicknes / 2),
                    new Point(bitmap.Width, 0 + penThicknes / 2),
                    // Right
                    new Point(bitmap.Width - penThicknes / 2, 0),
                    new Point(bitmap.Width - penThicknes / 2, bitmap.Height - penThicknes / 2),
                    // Bottom
                    new Point(0, bitmap.Height - penThicknes / 2),
                    new Point(bitmap.Width - penThicknes / 2, bitmap.Height - penThicknes / 2)
                };
            }
            else
            {
                borderPoints = new PointF[]
                {
                    // Left
                    new Point(0 + penThicknes / 2, 0),
                    new Point(0 + penThicknes / 2, bitmap.Height),
                    // Top
                    new Point(0, 0 + penThicknes / 2),
                    new Point(bitmap.Width, 0 + penThicknes / 2),
                    // Right
                    new Point(bitmap.Width - penThicknes / 2 - 1, 0),
                    new Point(bitmap.Width - penThicknes / 2 - 1, bitmap.Height - penThicknes / 2 - 1),
                    // Bottom
                    new Point(0, bitmap.Height - penThicknes / 2 - 1),
                    new Point(bitmap.Width - penThicknes / 2 - 1, bitmap.Height - penThicknes / 2 - 1)
                };
            }
            return borderPoints;
        }
    }
}
