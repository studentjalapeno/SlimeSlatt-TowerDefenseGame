using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



/// <summary>
/// This script is used for all tiles in the game
/// </summary>
public class TileScript : MonoBehaviour //judges where you can place towers /check if tower position is already being used / 
{


    public Point GridPosition  { get; set; } //The tiles grid position 

    public bool IsEmpty { get; set; }


    /// <summary>
    /// The color of the tile when it is full, this is used when mouse is hovered over tile
    /// </summary>
    private Color32 fullColor = new Color32(255, 118, 118, 255);


    /// <summary>
    /// The color of the tile when it is empty, this is used when mouse is hovered over tile
    /// </summary>
    private Color32 emptyColor = new Color32(96, 255, 90, 255);

    private SpriteRenderer spriteRenderer;


    public bool Debugging { get; set; } //property
    public bool WalkAble { get; set; } //property

    private Tower myTower;



    public Vector2 WorldPosition //The tiles world position
    {
        get
        {
            //take top left of corner of tile and add half the size of x, then take y position and subract half its size to get the center of the tile
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y/2));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); //creates a reference for SpriteRenderer component
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    /// <summary>
    /// Sets up the tile, this is an alternative to a constructor 
    /// </summary>
    /// <param name="gridpos">The tiles grid position</param>
    /// <param name="worldPosition">The tiles world position</param>
    
    public void Setup(Point gridpos, Vector3 worldPosition, Transform parent)
    {
        WalkAble = true; //set all nodes walkable
        IsEmpty = true;
        this.GridPosition = gridpos;
        transform.position = worldPosition;
        transform.SetParent(parent);


        //Add that new tile point in a dictionary with the key (grid pos)
        LevelManager.Instance.Tiles.Add(gridpos, this);

    }
    /// <summary>
    /// Checks if mouse if over tile and returns the position of the tile thats being hovered 
    /// </summary>
    private void OnMouseOver()
    {

        ColorTile(fullColor);

        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null) // only trys to place tower on ground if mouse is not hovering over a button on ui
        {
            if (IsEmpty && !Debugging) // if tile is empty
            {
                ColorTile(emptyColor); //sets tile color to green (indicating tower can be placed)
            }
            if (!IsEmpty && !Debugging)//if tile is not empty
            {
                ColorTile(fullColor); //sets tile color to red (indicating tower cannot be placed)
            }
            //Debug.Log(GridPosition.X + ", " + GridPosition.Y); //displays (X,Y) to console
            else if (Input.GetMouseButtonDown(0)) //if user clicks mouse (only excecuting once user cannot hold down mouse)
            {
                PlaceTower();
            }
         }
         else if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn == null && Input.GetMouseButtonDown(0)) 
         {
                if(myTower !=null)
                {
                    GameManager.Instance.SelectTower(myTower);
                }
                else
                {
                    GameManager.Instance.DeselectTower();
                }

          }
    }
    


    /// <summary>
    /// Called whenever mouse exits tile (changes a tile back to orginal color)
    /// </summary>
    private void OnMouseExit()
    {
        if (!Debugging)
        {
            ColorTile(Color.white);
        }

    
    }

    /// <summary>
    /// Places a tower on the tile
    /// </summary>

    private void PlaceTower()
    {


        GameObject tower = (GameObject)Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity); //spawns tower

        //sets sorting order of tower thats being placed based on Y position of grid
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;

        //tower becomes child object of tile thats it's placed on
        tower.transform.SetParent(transform);

        this.myTower = tower.transform.GetChild(0).GetComponent<Tower>();

        //sets tile to default color
        ColorTile(Color.white);

        myTower.Price = GameManager.Instance.ClickedBtn.Price;

        //tile is not empty once tower is placed
        IsEmpty = false;

        //Calls method BuyTower
        GameManager.Instance.BuyTower();

        WalkAble = false;

    }

    /// <summary>
    /// Sets the color on the tile
    /// </summary>
    /// <param name="newColor"></param>
    private void ColorTile(Color newColor)
    {
        spriteRenderer.color = newColor;
    }
}
