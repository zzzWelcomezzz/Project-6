using UnityEngine;
using System.Collections;

public class Point {
    public int x { get; private set; }
    public int y { get; private set; }

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Point operator +(Point left, Point right)
    {
        return new Point(left.x + right.x, left.y + right.y);
    }

    public static Point operator -(Point left, Point right)
    {
        return new Point(left.x - right.x, left.y - right.y);
    }

    public static bool operator ==(Point left, Point right)
    {
        return left.x == right.x && left.y == right.y;
    }

    public static bool operator !=(Point left, Point right)
    {
        return !(left == right);
    }

    public override bool Equals(object obj)
    {
        Point point = obj as Point;
        if (obj == null)
            return false;

        return this == point;      
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return "(" + x + ", " + y + ")";
    }
}
