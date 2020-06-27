using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Point // Struct value type not reference
{


    public int X { get; set; }

    public int Y { get; set; }


    /// <summary>
    /// Sets the values of the struct 
    /// </summary>
    /// <param name="x"> Initial x </param>
    /// <param name="y"> Initial y </param>
    public Point(int x, int y) // Constructor
    {

        this.X = x;
        this.Y = y;

    }

    /// <summary>
    /// Called when two points are being compared 
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static bool operator ==(Point first, Point second)
    {
        return first.X == second.X && first.Y == second.Y;
    }

    public static bool operator !=(Point first, Point second)
    {
        return first.X != second.X || first.Y != second.Y;
    }

    public static Point operator -(Point x, Point y)
    {
        return new Point(x.X - y.X, x.Y - y.Y);

    }













}
