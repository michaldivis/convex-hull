using Domain;
using SkiaSharp;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Visualizer;

public partial class MainWindow : Window
{
    private readonly SKPaint _insidePointPaint = new()
    {
        Color = SKColor.Parse("#d1d5db"),
        IsAntialias = true,
    };

    private readonly SKPaint _convexHullPointPaint = new()
    {
        Color = SKColor.Parse("#67e8f9"),
        IsAntialias = true,
    };

    private readonly SKPaint _originPointPaint = new()
    {
        Color = SKColor.Parse("#22c55e"),
        IsAntialias = true,
    };

    private readonly SKPaint _linePaint = new()
    {
        Color = SKColor.Parse("#64748b"),
        IsAntialias = true,
        StrokeWidth = 1,
    };

    private const float _pointRadius = 4;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnPaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
    {
        var surface = e.Surface;
        var canvas = surface.Canvas;

        canvas.Clear();

        canvas.RotateDegrees(180, 250, 250);

        canvas.DrawColor(SKColor.Parse("#030712"));

        var points = Enumerable
            .Range(1, (int)sliderAmountOfPoints.Value)
            .Select(_ => new Domain.Point(Random.Shared.Next(20, 481), Random.Shared.Next(20, 481)))
            .ToArray();

        foreach (var point in points)
        {
            canvas.DrawCircle(new SKPoint(point.X, point.Y), _pointRadius, _insidePointPaint);
        }

        var convexHullPoints = GrahamScan.ConvexHull(points);

        for (var i = 0; i < convexHullPoints.Length - 1; i++)
        {
            var current = convexHullPoints[i];
            var next = convexHullPoints[i + 1];

            canvas.DrawLine(new SKPoint(current.X, current.Y), new SKPoint(next.X, next.Y), _linePaint);
        }

        canvas.DrawLine(new SKPoint(convexHullPoints[^1].X, convexHullPoints[^1].Y), new SKPoint(convexHullPoints[0].X, convexHullPoints[0].Y), _linePaint);

        foreach (var point in convexHullPoints)
        {
            canvas.DrawCircle(new SKPoint(point.X, point.Y), _pointRadius, _convexHullPointPaint);
        }

        canvas.DrawCircle(new SKPoint(convexHullPoints[0].X, convexHullPoints[0].Y), _pointRadius, _originPointPaint);
    }

    private void Generate(object sender, RoutedEventArgs e) => skElement.InvalidateVisual();
}