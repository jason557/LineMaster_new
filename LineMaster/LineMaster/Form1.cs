
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;

namespace LineMaster
{
    public partial class Form1 : Form
    {
        private List<LineList> MyLines = new List<LineList>();
        private List<Point> MyPoints = new List<Point>();
        public Point MouseDownLocation;
        private bool IsMouseDown = false;
        private bool IsMouseDrag = false;
        
        private int StartX;
        private int StartY;
        private int CurX;
        private int CurY;
       
        
        private string DrawAction = @"Drawing";
        private string DrawAction0 = @"Move";
        private string LineType = @"Line";
        int Width1, Height1;
        Point Point1 = new Point();
        Point Point2 = new Point();
      
       

        Point StartDownLocation = new Point();
        Point txtPosition = new Point(0, 0);
        int Select_Line_Index = -1;
        string Select_Line_Type;
        string Select_Point = @"Null";

        int Select_PenWidth = 1;      
        StreamWriter Error000;

        Color Select_Color=Color.Red;   

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Error000 = new StreamWriter(@"000.txt");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Error000.Close();
        }
        private void lineMenu1_Click(object sender, EventArgs e)
        {
            DrawAction = @"Drawing";
            LineType = @"Line";
            this.Text = @"Line";
            if (MyLines.Count >= 1 && Select_Line_Index >= 0 && Select_Line_Index <= (MyLines.Count - 1))
            {
                MyLines[Select_Line_Index].Selected = false;
            }
           
        }

        private void rectangleMenu1_Click(object sender, EventArgs e)
        {
            DrawAction = @"Drawing";
            LineType = @"Rectangle";
            this.Text = @"Rectangle";
            if (MyLines.Count >= 1 && Select_Line_Index >= 0 && Select_Line_Index <= (MyLines.Count - 1))
            {
                MyLines[Select_Line_Index].Selected = false;
            }
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            string TempLineType;          
           
            if (e.Button == MouseButtons.Left && IsMouseDrag == false)
            {
                Select_Line_Index = -1;
                for (int i = 0; i < MyLines.Count; i++)
                {
                    MyLines[i].Selected = false;                 
                }

                for (int i = 0; i < MyLines.Count; i++)
                {
                    TempLineType = MyLines[i].LineType;
                    if (TempLineType == @"Line")
                    {
                        if (MyLines[i].SelectLineTest(e.Location) == true)
                        {
                            MyLines[i].Selected = true;
                            if (DrawAction0 == @"Move")
                            {
                                DrawAction = @"Move";
                            }
                            else if (DrawAction0 == @"Copy")
                            {
                                DrawAction = @"Copy";
                            }
                            MyLines[i].LineColor = Select_Color;
                            MyLines[i].LineWidth = Select_PenWidth;                           
                        }
                        else
                        {
                            MyLines[i].Selected = false;
                        }
                    }
                    else if (TempLineType == @"Rectangle" )
                    {
                        if (MyLines[i].SelectRectangleTest(e.Location) == true)
                        {
                            MyLines[i].Selected = true;
                            MyLines[i].LineColor = Select_Color;
                            MyLines[i].LineWidth = Select_PenWidth;                           

                            if (DrawAction0 == @"Move")
                            {
                                DrawAction = @"Move";
                            }
                            else if (DrawAction0 == @"Copy")
                            {
                                DrawAction = @"Copy";
                            }
                            if (TempLineType == @"Rectangle")
                            {
                                DrawAction = @"Copy";
                                DrawAction0 = @"Copy";
                            }
                        }
                        else
                        {
                            MyLines[i].Selected = false;
                        }
                    }                   
                }

                /////////////////////////////////////////////////////////////////////
                for (int i = 0; i < MyLines.Count; i++)
                {
                    if (MyLines[i].Selected == true)
                    {
                        Select_Line_Type = MyLines[i].LineType;
                        break;
                    }
                }

                ////////////////////////////////////////////////////////////////////
                //// this block to find the start @ end Point of the line
                ///  the Rectange and ellipse do not have the start and end point
                for (int i = 0; i < MyLines.Count; i++)
                {
                    TempLineType = Select_Line_Type;
                    if (TempLineType == @"Line")
                    {
                        if (MyLines[i].SelectPointTest(e.Location) == @"Start")
                        {
                            MyLines[i].SelectPoint = @"Start";
                            MyLines[i].Selected = true;

                        }
                        else if (MyLines[i].SelectPointTest(e.Location) == @"End")
                        {
                            MyLines[i].SelectPoint = @"End";
                            MyLines[i].Selected = true;
                        }
                        else
                        {
                            MyLines[i].SelectPoint = @"Null";
                        }
                    }
                }

                // this block the find select index                
                for (int i = 0; i < MyLines.Count; i++)
                {
                    if (MyLines[i].Selected == true)
                    {
                        Select_Line_Index = i;
                        if (MyLines[i].LineType == @"Line")
                        {
                            Select_Line_Type = MyLines[i].LineType;
                            if (MyLines[i].SelectPoint == @"Start" || MyLines[i].SelectPoint == @"End")
                            {
                                DrawAction = @"ExtendLine";
                               
                            }                           
                            else
                            {
                                if (DrawAction0 == @"Copy")
                                {
                                    DrawAction = @"Copy";
                                }
                                else if (DrawAction0 == @"Move")
                                {
                                    DrawAction = @"Move";
                                }
                            }
                        }                        
                        break;
                    }
                }

                

                for (int i = 0; i <= MyLines.Count - 1; i++)
                {
                    if (i == Select_Line_Index)
                    {
                        MyLines[i].Selected = true;
                    }
                    else
                    {
                        MyLines[i].Selected = false; ;
                    }
                }
                //if (Select_Line_Index >= 0)
                //{
                //    Error000.WriteLine("MyLines.Count - 1={0}", MyLines.Count - 1);
                //    Error000.WriteLine("Select_Line_Index={0}", Select_Line_Index);
                //    Error000.WriteLine("LineType={0}", MyLines[Select_Line_Index].LineType);
                //    for (int i = 0; i <= MyLines.Count - 1; i++)
                //    {
                //        Error000.WriteLine("MyLines[{0}]= X1={1}, Y1={2}, X2={3}, Y2={4}",i, MyLines[i].X1, MyLines[i].Y1, MyLines[i].X2, MyLines[i].Y2);
                //    }
                //    Error000.WriteLine("");
                //}
                pictureBox1.Invalidate();
            }
        }


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IsMouseDown = true;
                StartX = e.X;
                StartY = e.Y;
                CurX = e.X;
                CurY = e.Y;

                StartDownLocation = e.Location;
            }
        }


        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown == true)
            {
                Pen dashed_pen = new Pen(Color.Lime, 1);
                dashed_pen.DashStyle = DashStyle.Dot;
                CurX = e.X;
                CurY = e.Y;

              

                switch (DrawAction)
                {
                    case "Drawing":
                        {
                            break;
                        }
                 
                    case "Copy":
                        {
                            int i;

                            i = Select_Line_Index;
                            if (i >= 0 && MyLines.Count >= 1)
                            {
                                int dx = e.X - StartDownLocation.X;
                                int dy = e.Y - StartDownLocation.Y;
                                if (Select_Line_Type == @"Line")
                                {
                                    Point1.X = MyLines[i].X1 + dx;
                                    Point1.Y = MyLines[i].Y1 + dy;
                                    Point2.X = MyLines[i].X2 + dx;
                                    Point2.Y = MyLines[i].Y2 + dy;

                                }
                                else if (Select_Line_Type == @"Rectangle")
                                {
                                    Point1.X = MyLines[i].X1 + dx;
                                    Point1.Y = MyLines[i].Y1 + dy;
                                    Width1 = MyLines[i].Width;
                                    Height1 = MyLines[i].Height;
                                    Point2.X = Point1.X + Width1;
                                    Point2.Y = Point1.Y + Height1;
                                }                                                                                              
                            }
                            break;
                        }
                    case "Move":
                        {
                            int i;
                            i = Select_Line_Index;

                            if (i >= 0 && MyLines.Count >= 1)
                            {
                                int dx = e.X - StartDownLocation.X;
                                int dy = e.Y - StartDownLocation.Y;
                                if (Select_Line_Type == @"Line")
                                {
                                    Point1.X = MyLines[i].X1 + dx;
                                    Point1.Y = MyLines[i].Y1 + dy;
                                    Point2.X = MyLines[i].X2 + dx;
                                    Point2.Y = MyLines[i].Y2 + dy;
                                }
                                else if (Select_Line_Type == @"Rectangle")
                                {
                                    Point1.X = MyLines[i].X1 + dx;
                                    Point1.Y = MyLines[i].Y1 + dy;
                                    Width1 = MyLines[i].Width;
                                    Height1 = MyLines[i].Height;
                                    Point2.X = Point1.X + Width1;
                                    Point2.Y = Point1.Y + Height1;
                                }                              
                            }
                            break;
                        }
                 
                }
                DragTest();
                pictureBox1.Invalidate();
            }
        }
        private bool DragTest()
        {
            double length = (StartX - CurX) * (StartX - CurX) + (StartY - CurY) * (StartY - CurY);
            length = Math.Sqrt(length);
         
            if (length >= 10)
            {
                IsMouseDrag = true;
                return true;
            }
            else
            {
                IsMouseDrag = false;
                return false;
            }
        }


        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && IsMouseDrag == true)
            {
               
                switch (DrawAction)
                {
                    case "Drawing":
                        {
                            LineList DrawLine = new LineList();
                            switch (LineType)
                            {
                                case "Line":
                                    {
                                        DrawLine.X1 = StartX;
                                        DrawLine.Y1 = StartY;
                                        DrawLine.X2 = CurX;
                                        DrawLine.Y2 = CurY;
                                        DrawLine.LineType = @"Line";
                                        DrawLine.LineColor = Select_Color;
                                        DrawLine.LineWidth = Select_PenWidth;
                                        break;
                                    }                                
                                case "Rectangle":
                                    {
                                        DrawLine.X1 = Math.Min(StartX, CurX);
                                        DrawLine.Y1 = Math.Min(StartY, CurY);
                                        DrawLine.Width = Math.Abs(StartX - CurX);
                                        DrawLine.Height = Math.Abs(StartY - CurY);
                                        DrawLine.X2 = DrawLine.GetX2(DrawLine.X1, DrawLine.Width);
                                        DrawLine.Y2 = DrawLine.GetY2(DrawLine.Y1, DrawLine.Height);
                                        DrawLine.LineType = @"Rectangle";
                                        DrawLine.LineColor = Select_Color;
                                        DrawLine.LineWidth = Select_PenWidth;
                                        break;
                                    }
                             
                            }

                            if (DrawLine.DragTest() == true)
                            {
                                MyLines.Add(DrawLine);
                            }
                            pictureBox1.Invalidate();
                            break;
                        }
                    case "Copy":
                        {
                            int i = Select_Line_Index;
                            if (i >= 0 && MyLines.Count >= 1)
                            {
                                LineList DrawLine = new LineList();
                                DrawLine.X1 = Point1.X;
                                DrawLine.Y1 = Point1.Y;
                                DrawLine.X2 = Point2.X;
                                DrawLine.Y2 = Point2.Y;
                                DrawLine.Width = DrawLine.GetWidth(DrawLine.X1, DrawLine.X2);
                                DrawLine.Height = DrawLine.GetHeight(DrawLine.Y1, DrawLine.Y2);
                                DrawLine.LineColor = MyLines[i].LineColor;
                                DrawLine.LineType = MyLines[i].LineType;
                                DrawLine.LineWidth = MyLines[i].LineWidth;
                                switch (Select_Line_Type)
                                {
                                    case "Line":
                                        {
                                            MyLines.Add(DrawLine);
                                            Select_Line_Index = MyLines.Count - 1;
                                            MyLines[i].Selected = false;
                                            MyLines[Select_Line_Index].Selected = true;
                                            break;
                                        }
                                    
                                    case "Rectangle":
                                        {
                                            int X1 = Math.Min(Point1.X, Point2.X); int Y1 = Math.Min(Point1.Y, Point2.Y);
                                            int X2 = Math.Max(Point1.X, Point2.X); int Y2 = Math.Max(Point1.Y, Point2.Y);
                                            DrawLine.X1 = X1; DrawLine.Y1 = Y1;
                                            DrawLine.X2 = X2; DrawLine.Y2 = Y2;
                                            DrawLine.Width = DrawLine.GetWidth(DrawLine.X1, DrawLine.X2);
                                            DrawLine.Height = DrawLine.GetHeight(DrawLine.Y1, DrawLine.Y2);
                                            MyLines.Add(DrawLine);
                                            Select_Line_Index = MyLines.Count - 1;
                                            MyLines[i].Selected = false;
                                            MyLines[Select_Line_Index].Selected = true;
                                            break;
                                        }                                
                                }
                            }                          
                            break;
                        }
                    case "Move":
                        {
                            int i = Select_Line_Index;
                            if (i >= 0 && MyLines.Count >= 1)
                            {
                                LineList DrawLine = new LineList();
                                DrawLine.X1 = Point1.X;
                                DrawLine.Y1 = Point1.Y;
                                DrawLine.X2 = Point2.X;
                                DrawLine.Y2 = Point2.Y;
                                DrawLine.Width = DrawLine.GetWidth(DrawLine.X1, DrawLine.X2);
                                DrawLine.Height = DrawLine.GetHeight(DrawLine.Y1, DrawLine.Y2);
                                DrawLine.LineColor = MyLines[i].LineColor;
                                DrawLine.LineType = MyLines[i].LineType;
                                DrawLine.LineWidth = MyLines[i].LineWidth;
                                switch (Select_Line_Type)
                                {
                                    case "Line":
                                        {
                                            MyLines[i] = DrawLine;
                                            break;
                                        }                                                                          
                                    case "Rectangle":
                                        {
                                            int X1 = Math.Min(Point1.X, Point2.X); int Y1 = Math.Min(Point1.Y, Point2.Y);
                                            int X2 = Math.Max(Point1.X, Point2.X); int Y2 = Math.Max(Point1.Y, Point2.Y);
                                            DrawLine.X1 = X1; DrawLine.Y1 = Y1;
                                            DrawLine.X2 = X2; DrawLine.Y2 = Y2;
                                            DrawLine.Width = DrawLine.GetWidth(DrawLine.X1, DrawLine.X2);
                                            DrawLine.Height = DrawLine.GetHeight(DrawLine.Y1, DrawLine.Y2);
                                            MyLines[i] = DrawLine;
                                            break;
                                        }                               
                                }
                            }                           
                            break;
                        }
                    case "ExtendLine":
                        {
                            int i;
                            i = Select_Line_Index;
                            if (i >= 0)
                            {
                                LineList DrawLine = new LineList();
                                if (Select_Point == @"Start")
                                {
                                    if (LineType == @"Line")
                                    {
                                        DrawLine.X1 = CurX;
                                        DrawLine.Y1 = CurY;
                                        DrawLine.X2 = MyLines[i].X2;
                                        DrawLine.Y2 = MyLines[i].Y2;
                                    }                                   
                                }
                                else if (Select_Point == @"End")
                                {
                                    if (LineType == @"Line")
                                    {
                                        DrawLine.X1 = MyLines[i].X1;
                                        DrawLine.Y1 = MyLines[i].Y1;
                                        DrawLine.X2 = CurX;
                                        DrawLine.Y2 = CurY;
                                    }                                  
                                }
                                if (DrawLine.DragTest() == true)
                                {
                                    MyLines[i].X1 = DrawLine.X1;
                                    MyLines[i].Y1 = DrawLine.Y1;
                                    MyLines[i].X2 = DrawLine.X2;
                                    MyLines[i].Y2 = DrawLine.Y2;
                                }
                                DrawAction = "Null";
                            }                           
                            break;
                        }                  
                }                
            }
            IsMouseDrag = false;
            IsMouseDown = false;
            pictureBox1.Invalidate();
        }
        private void copyMenu_Click(object sender, EventArgs e)
        {
           
            DrawAction = "Copy";
            DrawAction0 = "Copy";
            this.Text = @"Copy";
            int j = -1;
            for (int i = 0; i < MyLines.Count; i++)
            {
                if (MyLines[i].Selected == true)
                {
                    j = i;
                    break;
                }
            }
            if (j >= 0)
            {
                Select_Line_Index = j;
                Select_Line_Type = MyLines[j].LineType;
            }
        }

        private void moveMenu_Click(object sender, EventArgs e)
        {           
            DrawAction = @"Move";
            DrawAction0 = @"Move";
            this.Text = @"Move";
            int j = -1;
            for (int i = 0; i < MyLines.Count; i++)
            {
                if (MyLines[i].Selected == true)
                {
                    j = i;
                    break;
                }
            }
            if (j >= 0)
            {
                Select_Line_Index = j;
                Select_Line_Type = MyLines[j].LineType;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int i;

            Graphics g = e.Graphics;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Pen FreeHandPen = new Pen(Color.FromArgb(255, Select_Color), Select_PenWidth);
            

            for (i = 0; i <= MyLines.Count - 1; i++)
            {
            
                string TempLineType = MyLines[i].LineType;

                switch (TempLineType)
                {
                    case "Line":
                        {
                            if (MyLines[i].Selected == true)
                            {
                                Pen dashed_pen = new Pen(Color.Cyan, MyLines[i].LineWidth);
                                g.DrawLine(dashed_pen, MyLines[i].X1, MyLines[i].Y1, MyLines[i].X2, MyLines[i].Y2);
                            }
                            else
                            {
                                Pen dashed_pen = new Pen(MyLines[i].LineColor, MyLines[i].LineWidth);
                                g.DrawLine(dashed_pen, MyLines[i].X1, MyLines[i].Y1, MyLines[i].X2, MyLines[i].Y2);
                            }
                            break;
                        }                    
                    case "Rectangle":
                        {
                            if (MyLines[i].Selected == true)
                            {
                                Pen LinePen = new Pen(Color.Cyan, MyLines[i].LineWidth);
                                int x1 = MyLines[i].X1;
                                int y1 = MyLines[i].Y1;
                                int Width1 = MyLines[i].Width;
                                int Height1 = MyLines[i].Height;
                                e.Graphics.DrawRectangle(LinePen, x1, y1, Width1, Height1);
                            }
                            else
                            {
                                Pen LinePen = new Pen(MyLines[i].LineColor, MyLines[i].LineWidth);
                                int x1 = MyLines[i].X1;
                                int y1 = MyLines[i].Y1;
                                int Width1 = MyLines[i].Width;
                                int Height1 = MyLines[i].Height;
                                e.Graphics.DrawRectangle(LinePen, x1, y1, Width1, Height1);
                            }
                            break;
                        }                  
                }
            }

            for (i = 0; i <= MyLines.Count - 1; i++)
            {
                if (MyLines[i].Selected == true)
                {
                    if (MyLines[i].LineType == @"Line")
                    {
                        Select_Line_Index = i;

                        MyPoints.Clear();
                        MyPoints.Add(new Point(MyLines[i].X1, MyLines[i].Y1));
                        MyPoints.Add(new Point(MyLines[i].X2, MyLines[i].Y2));
                        int X1 = MyPoints[0].X - 5;
                        int Y1 = MyPoints[0].Y - 5;
                        Rectangle rect1 = new Rectangle(X1, Y1, 10, 10);

                        int X2 = MyPoints[1].X - 5;
                        int Y2 = MyPoints[1].Y - 5;
                        Rectangle rect2 = new Rectangle(X2, Y2, 10, 10);
                        Pen blackPen = new Pen(Color.Cyan, MyLines[i].LineWidth);
                        e.Graphics.DrawEllipse(blackPen, rect1);
                        e.Graphics.DrawEllipse(blackPen, rect2);
                        break;
                    }
                }
            }
            /////////////////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////
            if (IsMouseDown == true)
            {
                switch (DrawAction)
                {
                    case "Drawing":
                        {
                            Pen dashed_pen = new Pen(Select_Color, 1); ;
                            switch (LineType)
                            {
                                case "Line":
                                    g.DrawLine(dashed_pen, StartX, StartY, CurX, CurY);
                                    break;                                                                                              
                                case "Rectangle":
                                    {
                                        int x1 = Math.Min(StartX, CurX);
                                        int y1 = Math.Min(StartY, CurY);
                                        int Width1 = Math.Abs(StartX - CurX);
                                        int Height1 = Math.Abs(StartY - CurY);
                                        e.Graphics.DrawRectangle(dashed_pen, x1, y1, Width1, Height1);
                                        break;
                                    }                               
                            }
                            break;
                        }
                    case "Copy":
                        {
                            Pen dashed_pen = new Pen(Select_Color, 1);
                            i = Select_Line_Index;
                            if (i >= 0 && MyLines.Count >= 1)
                            {
                                if (Select_Line_Type == @"Line")
                                {
                                    g.DrawLine(dashed_pen, Point1.X, Point1.Y, Point2.X, Point2.Y);
                                }
                                else if (Select_Line_Type == @"Rectangle")
                                {
                                    int x1 = Point1.X;
                                    int y1 = Point1.Y;
                                    e.Graphics.DrawRectangle(dashed_pen, x1, y1, Width1, Height1);
                                }                             
                            }
                            break;
                        }
                    case "Move":
                        {
                            Pen dashed_pen = new Pen(Select_Color, 1);
                            i = Select_Line_Index;
                            if (i >= 0 && MyLines.Count >= 1)
                            {
                                if (Select_Line_Type == @"Line")
                                {
                                    g.DrawLine(dashed_pen, Point1.X, Point1.Y, Point2.X, Point2.Y);
                                }
                                else if (Select_Line_Type == @"Rectangle")
                                {
                                    int x1 = Point1.X;
                                    int y1 = Point1.Y;
                                    e.Graphics.DrawRectangle(dashed_pen, x1, y1, Width1, Height1);
                                }                                
                            }
                            break;
                        }
                    case "ExtendLine":
                        {
                            if (MyLines.Count - 1 >= 0)
                            {
                                i = Select_Line_Index;

                                if (i >= 0)
                                {

                                    if (MyLines[i].SelectPoint == @"Start")
                                    {
                                        StartX = MyLines[i].X2;
                                        StartY = MyLines[i].Y2;
                                        Select_Line_Index = i;
                                        Select_Point = @"Start";
                                    }
                                    else if (MyLines[i].SelectPoint == @"End")
                                    {
                                        StartX = MyLines[i].X1;
                                        StartY = MyLines[i].Y1;
                                        Select_Line_Index = i;
                                        Select_Point = @"End";
                                    }
                                    Pen dashed_pen = new Pen(Color.Cyan, 1);
                                    string TempLineType = MyLines[i].LineType;
                                    switch (TempLineType)
                                    {
                                        case "Line":
                                            g.DrawLine(dashed_pen, StartX, StartY, CurX, CurY);
                                            break;                                        
                                    }
                                }
                            }
                            break;
                        }                   
                }
            }
        }
    }
}
