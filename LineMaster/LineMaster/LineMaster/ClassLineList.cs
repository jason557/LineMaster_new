using System;
using System.Drawing;

namespace LineMaster
{
    class LineList
    {
        public static long TotalCount;
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public string LineType { get; set; }
        public Color LineColor { get; set; }
        public float LineWidth { get; set; }
        public string Text { get; set; }
        public bool Selected { get; set; }
        public string SelectPoint { get; set; }


        public int GetX2(int _X, int _Width)
        {
            return X2 = X1 + _Width;
        }

        public int GetY2(int _Y, int _Height)
        {
            return Y1 + _Height;
        }


        public int GetWidth(int _X1, int _X2)
        {
            return Math.Abs(_X1 - _X2);
        }
        public int GetHeight(int _Y1, int _Y2)
        {
            return Math.Abs(_Y1 - _Y2);
        }

        public void Draw(Graphics G)
        { using (Pen pen = new Pen(LineColor, LineWidth)) G.DrawLine(pen, X1, Y1, X2, Y2); }



        public bool SelectLineTest(Point Pt)
        {
            double tolerance = 5;
            bool inBoundary;
            if (Math.Abs(X1 - X2) <= tolerance && Math.Abs(Y1 - Y2) > tolerance)
            {
                return Math.Abs(Pt.X - X1) <= tolerance && Math.Min(Y1, Y2) <= Pt.Y && Pt.Y <= Math.Max(Y1, Y2);
            }
            if (Math.Abs(Y1 - Y2) <= tolerance && Math.Abs(X1 - X2) > tolerance)
            {
                return Math.Abs(Pt.Y - Y1) <= tolerance && Math.Min(X1, X2) <= Pt.X && Pt.X <= Math.Max(X1, X2);
            }
            inBoundary = Math.Min(X1, X2) <= Pt.X && Pt.X <= Math.Max(X1, X2) && Math.Min(Y1, Y2) <= Pt.Y && Pt.Y <= Math.Max(Y1, Y2);
            if (!inBoundary) return false;
            double slope = ((double)(Y2 - Y1)) / (X2 - X1); // X2 != X1
            double intercept = Y1 - (slope * X1);
            double distance = Math.Abs(slope * Pt.X - Pt.Y + intercept) / Math.Sqrt(slope * slope + 1);
            return (distance <= tolerance);
        }

        public bool SelectRectangleTest(Point Pt)
        {
            Boolean inBoundary;
            inBoundary = Math.Min(X1, X2) <= Pt.X && Pt.X <= Math.Max(X1, X2) && Math.Min(Y1, Y2) <= Pt.Y && Pt.Y <= Math.Max(Y1, Y2);
            if (inBoundary)
            {
                Selected = true;
            }
            else
            {
                Selected = false;
            }
            return Selected;
        }


        public string SelectPointTest(Point Pt)
        {

            double d1 = (Pt.X - X1) * (Pt.X - X1) + (Pt.Y - Y1) * (Pt.Y - Y1);
            double d2 = (Pt.X - X2) * (Pt.X - X2) + (Pt.Y - Y2) * (Pt.Y - Y2);

            d1 = Math.Sqrt(d1);
            d2 = Math.Sqrt(d2);
            if (d1 <= 30)
            {
                SelectPoint = @"Start";
            }
            else if (d2 <= 30)
            {
                SelectPoint = @"End";
            }
            else
            {
                SelectPoint = @"Null";
            }
            return SelectPoint;
        }


        public bool DragTest()
        {
            double L = (X1 - X2) * (X1 - X2) + (Y1 - Y2) * (Y1 - Y2);
            L = Math.Sqrt(L);
            if (L <= 25)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public LineList()
        {
            X1 = 0;
            Y1 = 0;
            X2 = 0;
            Y2 = 0;
            Width = 0;
            Height = 0;

            LineType = @"Null";
            LineColor = Color.Lime;
            LineWidth = 1;
            Text = @"";
            Selected = false;
            SelectPoint = @"Null";

            TotalCount += 1;
        }

        ~LineList()
        {
            TotalCount -= 1;
        }

    }

}
