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
            new(65, 397),
            new(430, 130),
            new(179, 400),
            new(372, 400),
            new(453, 160),
            new(207, 76),
            new(279, 101),
            new(311, 303),
            new(426, 150),
            new(353, 45),
        };

        var expected = new Point[]
        {
            new(430, 130),
            new(453, 160),
            new(372, 400),
            new(179, 400),
            new(65, 397),
            new(207, 76),
            new(353, 45),
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
        var result = GrahamScan.FindPointWithTheLowestY(points);

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
        var result = GrahamScan.GetAngle(a, b);

        // Assert
        result.Should().BeApproximately(Math.PI / 4, 0.0001);
    }
}
