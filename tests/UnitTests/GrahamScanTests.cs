using Domain;
using FluentAssertions;
using Xunit;

namespace UnitTests;

public class GrahamScanTests
{
    [Fact]
    public void ConvexHull_NullInput_ThrowsArgumentNullException()
    {
        // Arrange
        Point[] points = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => GrahamScan.ConvexHull(points));
    }

    [Fact]
    public void ConvexHull_LessThanOrEqual3Points_ReturnsOriginalPoints()
    {
        // Arrange
        var points = new Point[]
        {
            new(0, 0),
            new(1, 1),
            new(2, 2)
        };

        // Act
        var result = GrahamScan.ConvexHull(points);

        // Assert
        result.Should().Equal(points);
    }

    [Fact]
    public void ConvexHull_MoreThan3Points_ReturnsCorrectConvexHull()
    {
        // Arrange
        var points = new Point[]
        {
            new(1, 0),
            new(2, 1),
            new(3, 1),
            new(3, 3),
            new(2, 2),
            new(1, 4),
            new(1, 2),
            new(0, 1),
        };

        var expected = new Point[]
        {
            new(1, 0),
            new(3, 1),
            new(3, 3),
            new(1, 4),
            new(0, 1),
        };

        // Act
        var result = GrahamScan.ConvexHull(points);

        // Assert
        result.Should().HaveCount(expected.Length);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ConvexHull_MoreThan3Points_IgnoresPointsWithCollinearDirection()
    {
        // Arrange
        var points = new Point[]
        {
            new(0, 0),
            new(1, 1),
            new(2, 2),
            new(1, 3),
            new(1, 2),
            new(0, 2),
        };

        var expected = new Point[]
        {
            new(0, 0),
            new(2, 2),
            new(1, 3),
            new(0, 2),
        };

        // Act
        var result = GrahamScan.ConvexHull(points);

        // Assert
        result.Should().HaveCount(expected.Length);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GetDirection_CollinearPoints_ReturnsDirectionNone()
    {
        // Arrange
        var a = new Point(0, 0);
        var b = new Point(1, 1);
        var c = new Point(2, 2);

        // Act
        var result = GrahamScan.GetDirection(a, b, c);

        // Assert
        result.Should().Be(Direction.Collinear);
    }

    [Fact]
    public void GetDirection_LeftTurn_ReturnsDirectionLeft()
    {
        // Arrange
        var a = new Point(0, 0);
        var b = new Point(1, 1);
        var c = new Point(0, 2);

        // Act
        var result = GrahamScan.GetDirection(a, b, c);

        // Assert
        result.Should().Be(Direction.Left);
    }

    [Fact]
    public void GetDirection_RightTurn_ReturnsDirectionRight()
    {
        // Arrange
        var a = new Point(0, 0);
        var b = new Point(1, 1);
        var c = new Point(2, 0);

        // Act
        var result = GrahamScan.GetDirection(a, b, c);

        // Assert
        result.Should().Be(Direction.Right);
    }

    [Fact]
    public void FindPointWithTheLowestY_ReturnsCorrectPoint()
    {
        // Arrange
        var points = new Point[]
        {
            new(0, 0),
            new(1, 1),
            new(2, -1),
            new(1, -1),
        };

        // Act
        var result = GrahamScan.FindPointWithTheLowestYCoordinate(points);

        // Assert
        result.Should().Be(new Point(1, -1));
    }

    [Fact]
    public void GetAngle_ReturnsCorrectAngle()
    {
        // Arrange
        var a = new Point(0, 0);
        var b = new Point(1, 1);

        // Act
        var result = GrahamScan.GetAngleAgainstXAxis(a, b);

        // Assert
        result.Should().BeApproximately(Math.PI / 4, 0.0001);
    }
}
