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

        public MainWindow()
        {
            InitializeComponent();

            button1.Click += Button1_Click;
            button2.Click += Button2_Click;
            button3.Click += Button3_Click;
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            DrawSnaw(canvas);
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            isDrawingSnow = false;
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            isDrawingSnow = false;
        }

        private void DrawSnowman(Canvas canvas)
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

        private async void DrawSnaw(Canvas canvas)
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
                        DrawSnowman(canvas);
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
    }
}
