using System;
using System.Collections.Generic;
//using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VPLab6
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isDrawingSnow = false;
        private bool isDrawingBall = false;
        private bool isDrawingPoints = false;

        private List<Point> pointsList = new List<Point>();

        public MainWindow()
        {
            InitializeComponent();

            DrawSnaw();

            button1.Click += Button1_Click;
            button2.Click += Button2_Click;
            button3.Click += Button3_Click;

            canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            isDrawingBall = false;
            isDrawingPoints = false;
            DrawSnaw();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            isDrawingSnow = false;
            isDrawingPoints = false;
            DrawBall();
        }

        private async void Button3_Click(object sender, RoutedEventArgs e)
        {
            isDrawingSnow = false;
            isDrawingBall = false;

            await Task.Delay(101);

            DrawGrid();
            DrawPointsOnCanvas();
            DrawBoundingRectangle();
        }

        private void DrawPointsOnCanvas()
        {
            foreach (Point point in pointsList)
            {
                Ellipse ellipse = new Ellipse();
                ellipse.Width = 5;
                ellipse.Height = 5;
                ellipse.Fill = Brushes.Red; 
                Canvas.SetLeft(ellipse, point.X - ellipse.Width / 2);
                Canvas.SetTop(ellipse, point.Y - ellipse.Height / 2);

                canvas.Children.Add(ellipse);
            }
        }

        private void DrawBoundingRectangle()
        {
            if (pointsList.Count < 2)
                return;

            Rect boundingRect = CalculateBoundingRectangle(pointsList);

            Rectangle rect = new Rectangle();
            rect.Width = boundingRect.Width;
            rect.Height = boundingRect.Height;
            rect.Stroke = Brushes.Blue;
            rect.StrokeThickness = 2;
            Canvas.SetLeft(rect, boundingRect.Left);
            Canvas.SetTop(rect, boundingRect.Top);

            canvas.Children.Add(rect);
        }

        private Rect CalculateBoundingRectangle(List<Point> points)
        {
            double minX = points.Min(p => p.X);
            double minY = points.Min(p => p.Y);
            double maxX = points.Max(p => p.X);
            double maxY = points.Max(p => p.Y);

            return new Rect(minX, minY, maxX - minX, maxY - minY);
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPoint = e.GetPosition(canvas);
            pointsList.Add(clickPoint);

            dataGridPoints.ItemsSource = pointsList;
            dataGridPoints.Items.Refresh();

            canvas.Children.Clear();
            DrawGrid();
            DrawPointsOnCanvas();
            DrawBoundingRectangle();
        }

        private void DrawSnowman()
        {
            SolidColorBrush blue = new SolidColorBrush(Color.FromRgb(210, 218, 228));
            SolidColorBrush black = new SolidColorBrush(Colors.Black);

            Path path = DrawEllipse(blue, 387, 220, 60, 60);
            path.Stroke = black;
            canvas.Children.Add(path);

            path = DrawEllipse(blue, 387, 130, 40, 40);
            path.Stroke = black;
            canvas.Children.Add(path);

            path = DrawEllipse(blue, 387, 80, 20, 20);
            path.Stroke = black;
            canvas.Children.Add(path);

            canvas.Children.Add(DrawEllipse(black, 380, 75, 3, 3));
            canvas.Children.Add(DrawEllipse(black, 394, 75, 3, 3));

            canvas.Children.Add(DrawEllipse(black, 387, 120, 2, 2));
            canvas.Children.Add(DrawEllipse(black, 387, 130, 2, 2));
            canvas.Children.Add(DrawEllipse(black, 387, 140, 2, 2));

            canvas.Children.Add(DrawEllipse(black, 387, 200, 3, 3));
            canvas.Children.Add(DrawEllipse(black, 387, 220, 3, 3));
            canvas.Children.Add(DrawEllipse(black, 387, 240, 3, 3));

            path = new Path();
            path.Stroke = black;
            path.StrokeThickness = 2;

            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();

            pathFigure.StartPoint = new Point(380, 85);

            BezierSegment bezierSegment = new BezierSegment();
            bezierSegment.Point1 = new Point(384, 90);
            bezierSegment.Point2 = new Point(390, 90);
            bezierSegment.Point3 = new Point(394, 85);

            pathFigure.Segments.Add(bezierSegment);

            pathGeometry.Figures.Add(pathFigure);

            path.Data = pathGeometry;

            canvas.Children.Add(path);
        }

        private Path DrawEllipse(SolidColorBrush solidColorBrush, int x, int y, int width, int height)
        {
            Path path = new Path();
            path.Fill = solidColorBrush;
            path.Data = new EllipseGeometry(new Point(x, y), width, height);

            return path;
        }

        private async void DrawSnaw()
        {
            SolidColorBrush solidColorBrush = new SolidColorBrush(Color.FromRgb(178, 212, 233));
            Random random = new Random();
            
            int rows = 2;
            int cols = 300;

            int[,] array = new int[rows, cols];

            for (int i = 0; i < cols; i++)
            {
                array[0, i] = random.Next(0, 770);
                array[1, i] = random.Next(0, 300);
            }

            isDrawingSnow = true;

            while (isDrawingSnow)
            {
                for (int i = 0; i < cols; i++)
                {
                    canvas.Children.Add(DrawEllipse(solidColorBrush, array[0, i], array[1, i], 5, 5));
                    
                    if (i == 100)
                    {
                        DrawSnowman();
                    }
                }



                for (int i = 0; i < cols; i++)
                {
                    array[0, i] += i % 3;
                    array[0, i] -= i % 2;
                    array[1, i]++;

                    if (array[1, i] > 300)
                    {
                        array[0, i] = random.Next(0, 770);
                        array[1, i] = 0;
                    }    
                }

                await Task.Delay(10);

                canvas.Children.Clear();
            }
        }

        private async void DrawBall()
        {
            Random random = new Random();
            byte r = Convert.ToByte(random.Next(0, 255));
            byte g = Convert.ToByte(random.Next(0, 255));
            byte b = Convert.ToByte(random.Next(0, 255));

            SolidColorBrush color = new SolidColorBrush(Color.FromRgb(r, g, b));

            int size = 50;
            int x = size, y = size;
            int directionX = 1, directionY = 1;
            bool isChangeColor = false;

            isDrawingBall = true;

            while (isDrawingBall)
            {
                canvas.Children.Add(DrawEllipse(color, x, y, size, size));

                if (x > 740 - size)
                {
                    directionX = -1;
                    isChangeColor = true;
                }

                if (y > 260 - size)
                {
                    directionY = -1;
                    isChangeColor = true;
                }

                if (x < size) 
                {
                    directionX = 1;
                    isChangeColor = true;
                }

                if (y < size)
                {
                    directionY = 1;
                    isChangeColor = true;
                }

                if (isChangeColor)
                {
                    isChangeColor = false;

                    r = Convert.ToByte(random.Next(0, 255));
                    g = Convert.ToByte(random.Next(0, 255));
                    b = Convert.ToByte(random.Next(0, 255));

                    color = new SolidColorBrush(Color.FromRgb(r, g, b));
                }

                x += directionX;
                y += directionY;

                await Task.Delay(1);

                canvas.Children.Clear();
            }
        }

        public void DrawGrid()
        {
            for (int i = 0; i < 37; i++)
            {
                DrawLine(10 + i * 20, 0, 10 + i * 20, 370);
            }

            for (int i = 0; i < 13; i++)
            {
                DrawLine(0, 10 + i * 20, 740, 10 + i * 20);
            }

            DrawLine(370, 0, 370, 370, 2);
            DrawLine(0, 130, 740, 130, 2);
        }

        private void DrawLine(int x1, int y1, int x2, int y2, int c = 1)
        {
            Line verticalLine = new Line();

            verticalLine.X1 = x1;
            verticalLine.Y1 = y1;
            verticalLine.X2 = x2;
            verticalLine.Y2 = y2;

            verticalLine.Stroke = Brushes.Gray;
            verticalLine.StrokeThickness = 1;

            if (c == 2)
            {
                verticalLine.Stroke = Brushes.Black;
                verticalLine.StrokeThickness = 2;
            }

            canvas.Children.Add(verticalLine);
        }
    }
}
