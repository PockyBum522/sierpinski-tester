using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

// ReSharper disable InconsistentNaming

namespace TriangleTester;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public int WindowWidth => 700;
    public int WindowHeight => 700;

    public int FirstPointX { get; set; }
    public int FirstPointY { get; set; }
    
    public bool FirstPointChosen;

    public List<Point> points = new ();
    
    public Random rand = new ();

    DispatcherTimer dispatcherTimer = new();   
    
    public MainWindow()
    {
        InitializeComponent();
        
        dispatcherTimer.Tick += DispatcherTimerOnTick;
        dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1);
        
        DataContext = this;
    }

    private void DispatcherTimerOnTick(object? sender, EventArgs e)
    {
        DrawTheRestOfThePoints();
    }


    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        // Draw triangle points
        InitializeWindowAndCanvas();
    }

    private void InitializeWindowAndCanvas()
    {
        TriangleCanvas.Width = WindowWidth - 30;
        TriangleCanvas.Height = WindowHeight - 30;

        // Top point
        var topPoint = new Point((double)WindowWidth / 2, 50);
        DrawPoint(topPoint.X, topPoint.Y);
    
        // Bottom left point
        var bottomLeftPoint = new Point(70, WindowHeight - 70);
        DrawPoint(bottomLeftPoint.X, bottomLeftPoint.Y);
        
        // Bottom right point
        var bottomRightPoint = new Point(WindowWidth - 70, WindowHeight - 70);
        DrawPoint(bottomRightPoint.X, bottomRightPoint.Y);
        
        DrawLine(topPoint.X, topPoint.Y, bottomLeftPoint.X, bottomLeftPoint.Y);
        DrawLine(topPoint.X, topPoint.Y, bottomRightPoint.X, bottomRightPoint.Y);
        DrawLine(bottomLeftPoint.X, bottomLeftPoint.Y, bottomRightPoint.X, bottomRightPoint.Y);
        
    }

    private void DrawPoint(double x, double y, Brush? color = null, bool b = true)
    {
        var ln = new Line
        {
            Stroke = color ?? Brushes.Black,
            X1 = x,
            Y1 = y,
            X2 = x - 1,
            Y2 = y - 1
        };

        TriangleCanvas.Children.Add(ln);
        if (b)
        {
            points.Add(
                new Point(x, y));    
        }
    }    
    
    private void DrawLine(double x1, double y1, double x2, double y2, Brush? color = null)
    {
        var ln = new Line
        {
            Stroke = color ?? Brushes.Black,
            X1 = x1,
            Y1 = y1,
            X2 = x2,
            Y2 = y2
        };

        TriangleCanvas.Children.Add(ln);
    }

    private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (FirstPointChosen)
        {
            dispatcherTimer.Start();
            return;
        }
        
        FirstPointChosen = true;
        
        FirstPointX = (int)e.GetPosition(TriangleCanvas).X;
        FirstPointY = (int)e.GetPosition(TriangleCanvas).Y;
        
        DrawPoint(FirstPointX, FirstPointY);
        
        dispatcherTimer.Start();
    }

    private void DrawTheRestOfThePoints()
    {
        DrawPoint(FirstPointX, FirstPointY, Brushes.Red, false);

        DrawNewPointHalfwayBetweenPoints();
    }

    private void DrawNewPointHalfwayBetweenPoints()
    {
        var numberOfPointsSoFar = points.Count;

        // Randomly select one of the original 3 points
        var randomPoint01 = rand.NextInt64(3);
        
        // Draw new point halfway between one of the original 3 and the point we just drew
        var point02 = points.Last();
        
        var newPoint = GetMidpoint(points[(int)randomPoint01], point02);
        
        DrawPoint((int)newPoint.X, (int)newPoint.Y);
    }
    
    Point GetMidpoint(Point a, Point b) 
    {
        Point ret = new();
        
        ret.X = (a.X + b.X) / 2;
        ret.Y = (a.Y + b.Y) / 2;
        
        return ret;
    }
}