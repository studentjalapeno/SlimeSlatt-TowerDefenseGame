using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AStar // static class 
{
    private static Dictionary<Point, Node> nodes;  //key = point value = Node

    /// <summary>
    /// Create nodes for each tile in the game
    /// </summary>
    private static void CreateNodes() 
    {
        nodes = new Dictionary<Point, Node>(); //instantiate the dictionary


        foreach(TileScript tile in LevelManager.Instance.Tiles.Values)   //run through each tile in Tiles Dictionary in LevelManager
        {
            nodes.Add(tile.GridPosition, new Node(tile));

        }

    }

    /// <summary>
    /// Generates a path with the A* algorithm
    /// </summary>
    /// <param name="start"></param>
    public static Stack<Node> Getpath(Point start, Point goal)
    {
        if (nodes == null) //If we dont have nodes thne we need to create them
        {
            CreateNodes();

        }


        //Creates an open list to be used with the A* algorith
        HashSet<Node> openList = new HashSet<Node>();

        //Creates an closed list to be used with the A* algorith
        HashSet<Node> closedList = new HashSet<Node>();

        Stack<Node> finalPath = new Stack<Node>();
        

        //set starting point in nodes dictionary and set it equal to currentNode
        Node currentNode = nodes[start]; 


        //1. Adds the start node to the openList
        openList.Add(currentNode);


        while (openList.Count > 0) //10.
        {
            //2. Runs through all neighbors
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Point neighbourPos = new Point(currentNode.GridPosition.X - x, currentNode.GridPosition.Y - y);


                    if (LevelManager.Instance.InBound(neighbourPos) && LevelManager.Instance.Tiles[neighbourPos].WalkAble && neighbourPos != currentNode.GridPosition) //if neighbour is not equal to self and if tile is walkable
                    {

                        int gCost = 0;

                        // [14 ][10][14]
                        // [10][S ][10]
                        // [14][10][14 ]
                        if (Math.Abs(x - y) == 1) //math.abs returns absoulute values 
                        {
                            gCost = 10;
                        }
                        else //score 14 is a diagonial move
                        {
                            if (!ConnectedDiagonally(currentNode, nodes[neighbourPos]))
                            {
                                continue;
                            }
                            gCost = 14;
                        }

                        //3. Adds the neighbor to the open list
                        Node neighbour = nodes[neighbourPos];



                        if (openList.Contains(neighbour))
                        {
                            if (currentNode.G + gCost < neighbour.G) //9.4
                            {
                                neighbour.CalcValues(currentNode, nodes[goal], gCost);
                            }



                        }


                        else if (!closedList.Contains(neighbour)) //9.1
                        {

                            openList.Add(neighbour); //9.2

                            neighbour.CalcValues(currentNode, nodes[goal], gCost); //9.3

                        }
                        //ONLY FOR DEBUGGING
                        // neighbour.TileRef.SpriteRenderer.color = Color.black;
                    }

                }


            }


            //5. & 8. remove current node from open list and add to closed list
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (openList.Count > 0) //7.
            {
                //Sorts the list by F value, and selects the first on the list
                currentNode = openList.OrderBy(n => n.F).First();

            }

            if (currentNode == nodes[goal])
            {
                while (currentNode.GridPosition != start)
                {
                    finalPath.Push(currentNode);
                    currentNode = currentNode.Parent;

                }
                
                return finalPath;
            }
        }

        return null;

       
     

        //****This is only for debugging needs to be removed later ******
        //GameObject.Find("AStarDebugger").GetComponent<AStarDebugger>().DebugPath(openList, closedList, finalPath); //DebugPath found in AStar script

    }


    private static bool  ConnectedDiagonally(Node currentNode, Node neighbor)
    {

        Point direction = neighbor.GridPosition - currentNode.GridPosition;

        Point first = new Point(currentNode.GridPosition.X + direction.X, currentNode.GridPosition.Y);

        Point second = new Point(currentNode.GridPosition.X, currentNode.GridPosition.Y + direction.Y);
        
        if(LevelManager.Instance.InBound(first) && !LevelManager.Instance.Tiles[first].WalkAble)
        {

            return false;
        }

        if(LevelManager.Instance.InBound(second) && !LevelManager.Instance.Tiles[second].WalkAble)
        {

            return false;
        }

        return true;


    }
}
