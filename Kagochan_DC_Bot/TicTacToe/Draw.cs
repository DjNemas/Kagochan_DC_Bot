using System.Drawing;

namespace Kagochan_DC_Bot.TicTacToe
{
    class Draw
    {
        private int abstand = 6;
        private int gridLengthWidth;
        private int gridLengthHeight;
        Bitmap bitmap;
        Graphics graphics;

        public Draw(Bitmap bitmap, Graphics graphics)
        {
            this.gridLengthWidth = (bitmap.Width / 3) / abstand;
            this.gridLengthHeight = (bitmap.Height / 3) / abstand;
            this.bitmap = bitmap;
            this.graphics = graphics;
        }


        public void DrawTicTacToeField(Pen pen)
        {
            int thirdsWidth = bitmap.Width / 3;
            int thirdsHeight = bitmap.Height / 3;

            // Vertical Left Line
            graphics.DrawLine(pen, new Point(thirdsWidth, 0), new Point(thirdsWidth, bitmap.Height));
            // Vertical Right Line
            graphics.DrawLine(pen, new Point(thirdsWidth * 2, 0), new Point(thirdsWidth * 2, bitmap.Height));
            //Horizontal Top Line
            graphics.DrawLine(pen, new Point(0, thirdsHeight), new Point(bitmap.Width, thirdsHeight));
            //Horizontal Bottom Line
            graphics.DrawLine(pen, new Point(0, thirdsHeight * 2), new Point(bitmap.Width, thirdsHeight * 2));
        }

        /// <summary>
        /// Field 1 | Field 2 | Field 3
        /// Field 4 | Field 5 | Field 6
        /// Field 7 | Field 8 | Field 9
        /// </summary>
        /// <param name="fieldNumber"></param>
        public void DrawCircle(int fieldNumber, Pen pen)
        {
            if (fieldNumber < 1 || fieldNumber > 9) return;

            if (fieldNumber == 1)
            {
                graphics.DrawEllipse(pen,
                gridLengthWidth * 1, gridLengthHeight * 1,
                gridLengthWidth * 4, gridLengthHeight * 4);
            }
            else if (fieldNumber == 2)
            {
                graphics.DrawEllipse(pen,
                gridLengthWidth * 1 + bitmap.Width / 3, gridLengthHeight * 1,
                gridLengthWidth * 4, gridLengthHeight * 4);
            }
            else if (fieldNumber == 3)
            {
                graphics.DrawEllipse(pen,
                gridLengthWidth * 1 + bitmap.Width / 3 * 2, gridLengthHeight * 1,
                gridLengthWidth * 4, gridLengthHeight * 4);
            }
            else if (fieldNumber == 4)
            {
                graphics.DrawEllipse(pen,
                gridLengthWidth * 1, gridLengthHeight * 1 + bitmap.Height / 3,
                gridLengthWidth * 4, gridLengthHeight * 4);
            }
            else if (fieldNumber == 5)
            {
                graphics.DrawEllipse(pen,
                gridLengthWidth * 1 + bitmap.Width / 3, gridLengthHeight * 1 + bitmap.Height / 3,
                gridLengthWidth * 4, gridLengthHeight * 4);
            }
            else if (fieldNumber == 6)
            {
                graphics.DrawEllipse(pen,
                gridLengthWidth * 1 + bitmap.Width / 3 * 2, gridLengthHeight * 1 + bitmap.Height / 3,
                gridLengthWidth * 4, gridLengthHeight * 4);
            }
            else if (fieldNumber == 7)
            {
                graphics.DrawEllipse(pen,
                gridLengthWidth * 1, gridLengthHeight * 1 + bitmap.Height / 3 * 2,
                gridLengthWidth * 4, gridLengthHeight * 4);
            }
            else if (fieldNumber == 8)
            {
                graphics.DrawEllipse(pen,
                gridLengthWidth * 1 + bitmap.Width / 3, gridLengthHeight * 1 + bitmap.Height / 3 * 2,
                gridLengthWidth * 4, gridLengthHeight * 4);
            }
            else if (fieldNumber == 9)
            {
                graphics.DrawEllipse(pen,
                gridLengthWidth * 1 + bitmap.Width / 3 * 2, gridLengthHeight * 1 + bitmap.Height / 3 * 2,
                gridLengthWidth * 4, gridLengthHeight * 4);
            }
        }

        /// <summary>
        /// Field 1 | Field 2 | Field 3
        /// Field 4 | Field 5 | Field 6
        /// Field 7 | Field 8 | Field 9
        /// </summary>
        /// <param name="fieldNumber"></param>
        public void DrawCross(int fieldNumber, Pen pen)
        {
            if (fieldNumber == 1)
            {
                graphics.DrawLine(pen, new Point(gridLengthWidth * 1, gridLengthHeight * 1), new Point(gridLengthWidth * 5, gridLengthHeight * 5));
                graphics.DrawLine(pen, new Point(gridLengthWidth * 5, gridLengthHeight * 1), new Point(gridLengthWidth * 1, gridLengthHeight * 5));
            }
            else if (fieldNumber == 2)
            {
                graphics.DrawLine(pen, new Point(gridLengthWidth * 1 + bitmap.Width / 3, gridLengthHeight * 1), new Point(gridLengthWidth * 5 + bitmap.Width / 3, gridLengthHeight * 5));
                graphics.DrawLine(pen, new Point(gridLengthWidth * 5 + bitmap.Width / 3, gridLengthHeight * 1), new Point(gridLengthWidth * 1 + bitmap.Width / 3, gridLengthHeight * 5));
            }
            else if (fieldNumber == 3)
            {
                graphics.DrawLine(pen, new Point(gridLengthWidth * 1 + bitmap.Width / 3 * 2, gridLengthHeight * 1), new Point(gridLengthWidth * 5 + bitmap.Width / 3 * 2, gridLengthHeight * 5));
                graphics.DrawLine(pen, new Point(gridLengthWidth * 5 + bitmap.Width / 3 * 2, gridLengthHeight * 1), new Point(gridLengthWidth * 1 + bitmap.Width / 3 * 2, gridLengthHeight * 5));
            }
            else if (fieldNumber == 4)
            {
                graphics.DrawLine(pen, new Point(gridLengthWidth * 1, gridLengthHeight * 1 + bitmap.Height / 3), new Point(gridLengthWidth * 5, gridLengthHeight * 5 + bitmap.Height / 3));
                graphics.DrawLine(pen, new Point(gridLengthWidth * 5, gridLengthHeight * 1 + bitmap.Height / 3), new Point(gridLengthWidth * 1, gridLengthHeight * 5 + bitmap.Height / 3));
            }
            else if (fieldNumber == 5)
            {
                graphics.DrawLine(pen, new Point(gridLengthWidth * 1 + bitmap.Width / 3, gridLengthHeight * 1 + bitmap.Height / 3), new Point(gridLengthWidth * 5 + bitmap.Width / 3, gridLengthHeight * 5 + bitmap.Height / 3));
                graphics.DrawLine(pen, new Point(gridLengthWidth * 5 + bitmap.Width / 3, gridLengthHeight * 1 + bitmap.Height / 3), new Point(gridLengthWidth * 1 + bitmap.Width / 3, gridLengthHeight * 5 + bitmap.Height / 3));
            }
            else if (fieldNumber == 6)
            {
                graphics.DrawLine(pen, new Point(gridLengthWidth * 1 + bitmap.Width / 3 * 2, gridLengthHeight * 1 + bitmap.Height / 3), new Point(gridLengthWidth * 5 + bitmap.Width / 3 * 2, gridLengthHeight * 5 + bitmap.Height / 3));
                graphics.DrawLine(pen, new Point(gridLengthWidth * 5 + bitmap.Width / 3 * 2, gridLengthHeight * 1 + bitmap.Height / 3), new Point(gridLengthWidth * 1 + bitmap.Width / 3 * 2, gridLengthHeight * 5 + bitmap.Height / 3));
            }
            else if (fieldNumber == 7)
            {
                graphics.DrawLine(pen, new Point(gridLengthWidth * 1, gridLengthHeight * 1 + bitmap.Height / 3 * 2), new Point(gridLengthWidth * 5, gridLengthHeight * 5 + bitmap.Height / 3 * 2));
                graphics.DrawLine(pen, new Point(gridLengthWidth * 5, gridLengthHeight * 1 + bitmap.Height / 3 * 2), new Point(gridLengthWidth * 1, gridLengthHeight * 5 + bitmap.Height / 3 * 2));
            }
            else if (fieldNumber == 8)
            {
                graphics.DrawLine(pen, new Point(gridLengthWidth * 1 + bitmap.Width / 3, gridLengthHeight * 1 + bitmap.Height / 3 * 2), new Point(gridLengthWidth * 5 + bitmap.Width / 3, gridLengthHeight * 5 + bitmap.Height / 3 * 2));
                graphics.DrawLine(pen, new Point(gridLengthWidth * 5 + bitmap.Width / 3, gridLengthHeight * 1 + bitmap.Height / 3 * 2), new Point(gridLengthWidth * 1 + bitmap.Width / 3, gridLengthHeight * 5 + bitmap.Height / 3 * 2));
            }
            else if (fieldNumber == 9)
            {
                graphics.DrawLine(pen, new Point(gridLengthWidth * 1 + bitmap.Width / 3 * 2, gridLengthHeight * 1 + bitmap.Height / 3 * 2), new Point(gridLengthWidth * 5 + bitmap.Width / 3 * 2, gridLengthHeight * 5 + bitmap.Height / 3 * 2));
                graphics.DrawLine(pen, new Point(gridLengthWidth * 5 + bitmap.Width / 3 * 2, gridLengthHeight * 1 + bitmap.Height / 3 * 2), new Point(gridLengthWidth * 1 + bitmap.Width / 3 * 2, gridLengthHeight * 5 + bitmap.Height / 3 * 2));
            }
        }

        public void DrawHelpLines(Pen pen)
        {
            
            for (int i = 0; i < abstand; i++)
            {
                if (i == 0) continue;
                // Horizontal Field 1
                graphics.DrawLine(pen, new Point(gridLengthWidth * i, 0), new Point(gridLengthWidth * i, bitmap.Height));
                graphics.DrawString(i.ToString(), new Font(new FontFamily(System.Drawing.Text.GenericFontFamilies.SansSerif), 10f), Brushes.AliceBlue, new Point(gridLengthWidth * i, 0));
                // Horizontal Field 2
                graphics.DrawLine(pen, new Point(gridLengthWidth * i + (bitmap.Width / 3), 0), new Point(gridLengthWidth * i + (bitmap.Width / 3), bitmap.Height));
                graphics.DrawString(i.ToString(), new Font(new FontFamily(System.Drawing.Text.GenericFontFamilies.SansSerif), 10f), Brushes.AliceBlue, new Point(gridLengthWidth * i + (bitmap.Width / 3), 0));
                // Horizontal Field 3
                graphics.DrawLine(pen, new Point(gridLengthWidth * i + ((bitmap.Width / 3 * 2)), 0), new Point(gridLengthWidth * i + (bitmap.Width / 3 * 2), bitmap.Height));
                graphics.DrawString(i.ToString(), new Font(new FontFamily(System.Drawing.Text.GenericFontFamilies.SansSerif), 10f), Brushes.AliceBlue, new Point(gridLengthWidth * i + (bitmap.Width / 3 * 2), 0));
                // Vertical Field 1
                graphics.DrawLine(pen, new Point(0, gridLengthHeight * i), new Point(bitmap.Width, (bitmap.Height / 3) / abstand * i));
                graphics.DrawString(i.ToString(), new Font(new FontFamily(System.Drawing.Text.GenericFontFamilies.SansSerif), 10f), Brushes.AliceBlue, new Point(0, (bitmap.Height / 3) / abstand * i));
                // Vertical Field 2
                graphics.DrawLine(pen, new Point(0, gridLengthHeight * i + ((bitmap.Height / 3))), new Point(bitmap.Width, gridLengthHeight * i + (bitmap.Height / 3)));
                graphics.DrawString(i.ToString(), new Font(new FontFamily(System.Drawing.Text.GenericFontFamilies.SansSerif), 10f), Brushes.AliceBlue, new Point(0, gridLengthHeight * i + (bitmap.Height / 3)));
                // Vertical Field 3
                graphics.DrawLine(pen, new Point(0, gridLengthHeight * i + ((bitmap.Height / 3 * 2))), new Point(bitmap.Width, gridLengthHeight * i + (bitmap.Height / 3 * 2)));
                graphics.DrawString(i.ToString(), new Font(new FontFamily(System.Drawing.Text.GenericFontFamilies.SansSerif), 10f), Brushes.AliceBlue, new Point(0, gridLengthHeight * i + (bitmap.Height / 3 * 2)));
            }
        }

    }
}
