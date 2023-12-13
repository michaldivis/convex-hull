﻿namespace Domain;

/// <summary>
/// This class implements the Graham Scan (<see href="https://en.wikipedia.org/wiki/Graham_scan"/>) algorithm for finding the convex hull of a finite set of points on a plane.
/// </summary>
public static class GrahamScan
{
    public static Point[] ConvexHull(Point[] points)
    {
        ArgumentNullException.ThrowIfNull(points);

        // if there are 3 or less points, all of the make up the convex hull
        if (points.Length <= 3)
        {
            return points;
        }

        var reference = FindPointWithTheLowestY(points);

        // sort the points in increasing order of the angle they and the reference point make with the x-axis
        var angleComparison = new Comparison<Point>((a, b) => GetAngle(a, reference).CompareTo(GetAngle(b, reference)));
        var convexHull = new List<Point>(points);
        convexHull.Sort(angleComparison);

        var aIndex = 0;
        var bIndex = 1;
        var cIndex = 2;

        while (true)
        {
            var a = convexHull[aIndex];
            var b = convexHull[bIndex];
            var c = convexHull[cIndex];
            
            if (GetDirection(a, b, c) == Direction.Right)
            {
                // If moving from B to C makes a right turn relative to moving from A to B, remove B as it's inside the hull
                convexHull.Remove(b);

                // If possible, re-evaluate the previous 3 points
                if (aIndex > 0)
                {
                    aIndex--;
                    bIndex--;
                    cIndex--;
                }
            }
            else
            {
                // Otherwise, move on to evaluate the next 3 points
                aIndex++;
                bIndex++;
                cIndex++;
            }

            // If the last point is reached, break the cycle
            if (cIndex >= convexHull.Count - 1)
            {
                break;
            }
        }

        // evaluate the last 2 points against the first point
        if (GetDirection(convexHull[^2], convexHull[^1], convexHull[0]) == Direction.Right)
        {
            convexHull.Remove(convexHull[^1]);
        }

        return convexHull.ToArray();
    }

    internal static Direction GetDirection(Point a, Point b, Point c)
    {
        var crossProduct = ((b.X - a.X) * (c.Y - a.Y)) - ((c.X - a.X) * (b.Y - a.Y));

        // return the direction of moving from B to C relative to moving from A to B
        return crossProduct.CompareTo(0) switch
        {
            -1 => Direction.Right,
            1 => Direction.Left,
            _ => Direction.Collinear,
        };
    }

    internal static Point FindPointWithTheLowestY(Point[] points)
    {
        var candidate = points[0];

        for (var i = 1; i < points.Length; i++)
        {
            var point = points[i];

            if (point.Y < candidate.Y)
            {
                candidate = point;
                continue;
            }

            if (point.Y == candidate.Y && point.X < candidate.X)
            {
                candidate = point;
                continue;
            }
        }

        return candidate;
    }

    internal static double GetAngle(Point a, Point b) => Math.Atan2(b.Y - a.Y, b.X - a.X);
}
