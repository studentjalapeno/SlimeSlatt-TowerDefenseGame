using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AStarDebugger : MonoBehaviour
{
    
    private TileScript start, goal;

    [SerializeField]
    private Sprite blankTile;

    [SerializeField]
    private GameObject arrowPrefab;

    [SerializeField]
    private GameObject debugTilePrefab;



    public Sprite BlankTile { get => blankTile; set => blankTile = value; }


    // Start is called before the first frame update
    void Start(){
        

    }

    // Update is called once per frame
    //void Update(){

    //    ClickTile();

        

    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        AStar.Getpath(start.GridPosition, goal.GridPosition);

    //    }
    //}

    private void ClickTile(){


        if (Input.GetKeyDown(KeyCode.Z)) //z key
        {
            UnityEngine.Debug.Log("Space is pressed"); 
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); //used to detect objects along a path

            if (hit.collider != null)
            {
                TileScript tmp = hit.collider.GetComponent<TileScript>(); //if tile is hit tmp = TileScript
                
                if(tmp != null) //if tmp is a tile (checks that it doenst hit a tower)
                {

                    //first time mouse is clicked 
                    if (start == null){

                        //start =  tile tmp
                        start = tmp;
                        CreateDebugTile(start.WorldPosition, new Color(255, 135, 0, 255));



                    }
                    //second mouse click
                    else if (goal == null){

                        //goal = tile tmp
                        goal = tmp;

                        CreateDebugTile(goal.WorldPosition, new Color(255, 0, 0, 255));
                       
                    }

                }

            }


        }

    }
    /// <summary>
    /// Debugs the path so that we can we whats going on 
    /// </summary>
    /// <param name="openList"></param>
    public void DebugPath(HashSet<Node> openList, HashSet<Node> closedList, Stack<Node> path) 
    {
        foreach (Node node in openList) //Colors all tiles cyan so that we can see which nodes are in list
        {
            if (node.TileRef != start && node.TileRef != goal) //if tile is not starting position && not goal
            {
                CreateDebugTile(node.TileRef.WorldPosition, Color.cyan, node);
            }

            //Points at the parent node
            PointToParent(node, node.TileRef.WorldPosition);
        
        }

        foreach (Node node in closedList) //Colors all tiles blue so that we can see which nodes are in list
        {
            if (node.TileRef != start && node.TileRef  != goal && path.Contains(node)) //if tile is not starting position 
            {
                CreateDebugTile(node.TileRef.WorldPosition, Color.blue, node);
            }


            //Points at the parent node
            PointToParent(node, node.TileRef.WorldPosition); //closed list arrows 
        }

        foreach (Node node in path)
        {
            if (node.TileRef != start && node.TileRef != goal)
            {

                CreateDebugTile(node.TileRef.WorldPosition, Color.green, node);

            }


        }

    }

    /// <summary>
    /// Creates an arrow that points at the parent
    /// </summary>
    /// <param name="node">Parent</param>
    /// <param name="position">World pos</param>
    private void PointToParent(Node node, Vector2 position)
    {
        if (node.Parent != null)
        {
            GameObject arrow = (GameObject)Instantiate(arrowPrefab, position, Quaternion.identity);
            arrow.GetComponent<SpriteRenderer>().sortingOrder = 3;

            //Right
            if ((node.GridPosition.X < node.Parent.GridPosition.X) && (node.GridPosition.Y == node.Parent.GridPosition.Y)) 
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 0); //point arrow

            }
            //Top right
            else if ((node.GridPosition.X < node.Parent.GridPosition.X) && (node.GridPosition.Y > node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 45); //point arrow

            }
            //Up
            else if ((node.GridPosition.X == node.Parent.GridPosition.X) && (node.GridPosition.Y > node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 90); //point arrow

            }
            //Top left
            else if ((node.GridPosition.X > node.Parent.GridPosition.X) && (node.GridPosition.Y > node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 135); //point arrow

            }

            //Left
            else if ((node.GridPosition.X > node.Parent.GridPosition.X) && (node.GridPosition.Y == node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 180); //point arrow

            }

            //Bottom left
            else if ((node.GridPosition.X > node.Parent.GridPosition.X) && (node.GridPosition.Y < node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 225); //point arrow

            }

            //Down
            else if ((node.GridPosition.X == node.Parent.GridPosition.X) && (node.GridPosition.Y < node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 270); //point arrow

            }

            //Bottom Right
            else if ((node.GridPosition.X < node.Parent.GridPosition.X) && (node.GridPosition.Y < node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 315); //point arrow

            }



        }


    }


    private void CreateDebugTile(Vector3 worldPos, Color32 color, Node node = null)
    {
        GameObject debugTile = (GameObject)Instantiate(debugTilePrefab, worldPos, Quaternion.identity);

        if(node != null)
        {
            DebugTile tmp = debugTile.GetComponent<DebugTile>(); //so we dont have to get component multiple times


            tmp.G.text += node.G; //G: + node g score
            tmp.H.text += node.H; //H: + node g score
            tmp.F.text += node.F; //F: + node g score

        }


        debugTile.GetComponent<SpriteRenderer>().color = color; //set tile color based on given

    }

}
