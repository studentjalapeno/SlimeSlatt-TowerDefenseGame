using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is a node used by the AStar algorith,
/// </summary>
public class Node
{

    /// <summary>
    /// The nodes grid position 
    /// </summary>
    public Point GridPosition { get; private set; }


    /// <summary>
    /// A reference to the tile that this node belongs to
    /// </summary>
    public TileScript TileRef { get; private set; }

    public Vector2 WorldPosition { get; set; }
 


    /// <summary>
    /// A reference to the nodes parent
    /// </summary>
    public Node Parent { get; private set; }

    public int G { get; set; }

    public int H { get; set; }

    public int F { get; set; }




    /// <summary>
    /// The nodes constructor 
    /// </summary>
    /// <param name="tileRef"> A reference to the tilescript </param>
    public Node(TileScript tileRef) //constructor (used evertime we create a node)
    {

        this.TileRef = tileRef;
        this.GridPosition = tileRef.GridPosition; //set gridpostion of the node by setting position of tile
        this.WorldPosition = TileRef.WorldPosition;

    }

    /// <summary>
    /// Calculate all values for the node
    /// </summary>
    /// <param name="parent"> The nodes parent </param>
    /// <param name="gScore"> The nodes g Score</param>
    public void CalcValues(Node parent,Node goal, int gCost)
    {

        this.Parent = parent;
        this.G = parent.G + gCost;
        this.H = ((Math.Abs(GridPosition.X - goal.GridPosition.X)) + Math.Abs((goal.GridPosition.Y - GridPosition.Y))) * 10;
        this.F = G + H;
    }


}
