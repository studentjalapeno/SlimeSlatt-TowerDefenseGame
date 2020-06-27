using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{

    /// <summary>
    /// An array of tilePrefabs, these are used for creating the tiles in the game
    /// </summary>
    [SerializeField]
    private GameObject[] tilePrefabs;

    public Portal StartPortal { get; set; }


    /// <summary>
    /// A reference to the camera movement script
    /// </summary>
    [SerializeField]
    private CameraMovement cameraMovement;


    /// <summary>
    /// The maps transform, this is needed for adding new Tiles
    /// </summary>
    [SerializeField]
    private Transform map;


    /// <summary>
    /// Spawn Points for the portals
    /// </summary>
    private Point portalSpawn;

    private Point portalGoal;

    /// <summary>
    /// prefab for spawning the red protal
    /// </summary>
    [SerializeField]
    private GameObject StartPortalPrefab;

    [SerializeField]
    private GameObject ExitPortalPrefab;


    private Point mapSize;

    private Stack<Node> path;

    public Stack<Node> Path
    {

        get
        {
            if(path == null)
            {
                GeneratePath();
            }

            return new Stack<Node>(new Stack<Node>(path));
        }
    }

    /// <summary>
    /// A dictionary that contains all tiles in our game
    /// </summary>
    public Dictionary<Point, TileScript> Tiles { get; set; }
    
   /// <summary>
   /// A property for returing the size of a tile
   /// </summary>
    public float TileSize {  //property
        
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    public Point PortalSpawn { get => portalSpawn; }

    public Point PortalGoal { get => portalSpawn; }



    // Start is called before the first frame update
    void Start(){
        

        CreateLevel();
    }

    // Update is called once per frame
    void Update(){
        
    }

    /// <summary>
    /// Creates level using txt document
    /// </summary>
    private void CreateLevel(){



        Tiles = new Dictionary<Point, TileScript>();

        string[] mapData = ReadLevelText(); //function called and stores .txt doc in mapData

        mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length); //gets mapsize by getting lengh of x and length

        //Calculates the X map size
        int mapXsize = mapData[0].ToCharArray().Length;  // save of "0000" = 4


        //Calculates the Y map size
        int mapYsize = mapData.Length; // length of array is 2 

        Vector3 maxTile = Vector3.zero;


        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        for (int y = 0; y < mapYsize; y++) // The y positions 
        {

            //Gets all tiles we need in order to limit world
            char[] newTiles = mapData[y].ToCharArray(); 

            for (int x = 0; x < mapXsize; x++) // The x positions 
            {

                //Places the tile in the world
                PlaceTile(newTiles[x].ToString(), x, y, worldStart) ; 
            }
        }

        
        //Take last tile in map and store it into max tile
        maxTile = Tiles[new Point(mapXsize - 1, mapYsize - 1)].transform.position;
        
        //Sets the camera limits to the max tile position 
        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize)); // use maxTilesize x/y to limit camera movement 

        SpawnPortal();
   
    }

    /// <summary>
    /// Places a tile in the gameworld
    /// </summary>
    /// <param name="tileType"> The type of tile to place EX : "0 or 1" </param> 
    /// <param name="x"> x position of the tile type </param>
    /// <param name="y"> y position of the tile typ </param>
    /// <param name="worldStart"> The world start position </param> 
    /// <returns></returns>

    private void PlaceTile(string tileType,int x, int y, Vector3 worldStart) {


        //Parses tiletype to an int so that we can use it as an indexer 
        int tileIndex = int.Parse(tileType); 


        //Create a new tile and makes a reference to that tile in the newTile variable
        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();

        // uses the new tile variable to change the position of the tile
        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0), map);
    }

    /// <summary>
    /// Gets text file "Level.txt" and stores data inorder to determine tile placement
    /// </summary>
    /// <returns></returns>
    private string[] ReadLevelText()
    {        
        TextAsset binddata = Resources.Load("Level") as TextAsset;
        string data = binddata.text.Replace(Environment.NewLine, string.Empty); //replace every single new line with a empty string 

        return data.Split('-'); //ignores '-' in text doc and returns whatever is in document
    }

    /// <summary>
    /// Spawns the portals in the game
    /// </summary>
    private void SpawnPortal()
    {   

        //Spawns the starting portal
        portalSpawn = new Point(1, 0);

        GameObject tmp = (GameObject)Instantiate(StartPortalPrefab, Tiles[portalSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        StartPortal = tmp.GetComponent<Portal>();
        StartPortal.name = "StartPortal";



        //Spawns the end portal
        portalGoal = new Point(13, 6);

        Instantiate(ExitPortalPrefab, Tiles[portalGoal].GetComponent<TileScript>().WorldPosition, Quaternion.identity);

    }


    /// <summary>
    /// Returns true if position is in bound of the map
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool InBound(Point position)
    {
        return position.X >= 0 && position.Y >= 0 && position.X < mapSize.X && position.Y < mapSize.Y;
    }


    public void GeneratePath()
    {
        path = AStar.Getpath(portalSpawn, portalGoal);
    }
}
