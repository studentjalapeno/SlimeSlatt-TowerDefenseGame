using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Delegate for the currency changed event
/// </summary>
public delegate void CurrencyChanged();


/// <summary>
/// GameManager handles time, countdown, nextwave, currency
/// </summary>

public class GameManager : Singleton<GameManager> //Singleton
{
    /// <summary>
    ///An event that is triggered when the currency is changed 
    /// </summary>
    public event CurrencyChanged Changed;

    //property (enables a class to expose a public way of getting and setting values
    public TowerBtn ClickedBtn { get; set; }

    private int currency; //money in game
    private int wave = 0; //starting wave number
    private int lives; //lives player has
    private int health = 15; //the health monsters have (this will be changed later
    
    private bool gameOver;

    private Tower selectedTower; // the current selected tower

    [SerializeField]
    private Text waveText;

    [SerializeField]
    private Text currencyText; //reference for currencyText

    [SerializeField]
    private Text livesText; //refrence for the livesText

    [SerializeField]
    private GameObject waveBtn;

    [SerializeField]
    private GameObject gameOverMenu;
    
    [SerializeField]
    private GameObject upgradePanel;

    [SerializeField]
    private GameObject statsPanel;

    [SerializeField]
    private Text sellText;

    [SerializeField]
    private Text statText;

    [SerializeField]
    private Text upgradePrice;

    [SerializeField]
    private GameObject inGameMenu;

    [SerializeField]
    private GameObject optionsMenu;


    private List<Monster> activeMonsters = new List<Monster>(); // a list of monsters active on the screen 

    public ObjectPool Pool { get; set; } //holds and resuses inactive objects

    //property enables access to set and get currency
    public int Currency
    {
        get => currency;
        set
        {
            this.currency = value;
            this.currencyText.text = value.ToString() + " <color=lime>Dollars</color>";


            //Everytime currency is changed function is triggered
            OnCurrencyChanged();

        }

    }

    //property enables accesst to set and get lived
    public int Lives
    {
        get => lives;
        set
        {
            this.lives = value;
            

            //If player loses all lives
            if (lives <= 0) 
            {
                this.lives = 0;
                GameOver();
            }

            this.livesText.text = lives.ToString();

        }
        
    }

    public bool WaveActive
    {
        get { return activeMonsters.Count > 0; } //returns true of activeMonsters > 0
    }

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }

    // Start is called before the first frame update

    void Start()
    {
        //starting currency
        Currency = 650;

        //starting lives;
        Lives = 10;

        gameOver = false;

    }

    // Update is called once per frame
    void Update()
    {
        HandleEscape(); //checks if esc is being pressed
    }


    /// <summary>
    /// Pick a tower then buy button is pressed
    /// </summary>
    /// <param name="towerBtn"></param>
    public void PickTower(TowerBtn towerBtn)
    {

        //if player has enough currency
        if (Currency >= towerBtn.Price && !WaveActive)
        {
            this.ClickedBtn = towerBtn; //makes refence to button that is clicked


            //Activates the hover icon
            Hover.Instance.Activate(towerBtn.Sprite);
        }

    }

    /// <summary>
    /// Select the tower
    /// </summary>
    /// <param name="tower"> The clicked tower</param>
    public void SelectTower(Tower tower)
    {
        
        if ( selectedTower != null)
        {

            //Selects the tower
            selectedTower.Select();
        }

        //sets the selected tower     
        selectedTower = tower;

        //Selects the tower
        selectedTower.Select();


        sellText.text = (selectedTower.Price / 2).ToString();

        upgradePanel.SetActive(true);
    }

    /// <summary>
    /// Deselect the tower
    /// </summary>
    public void DeselectTower()
    {

        //if we have a selected tower
        if (selectedTower !=null)
        {
            //calls select to deselect it 
            selectedTower.Select();
        }

        upgradePanel.SetActive(false);

        //Remove the reference to the tower
        selectedTower = null;


    }

    /// <summary>
    /// Buys a tower
    /// </summary>
    public void BuyTower()
    {
        if (Currency >= ClickedBtn.Price)
        {
            //reduce amount of currency player has when tower is bought
            Currency -= ClickedBtn.Price;

            //Calls method Deactivate from Hover class. Deactiates sprite renderer so hover icon disappears when tower is placed
            Hover.Instance.Deactivate();
        }
    }

    /// <summary>
    /// Sells a tower
    /// </summary>
    public void SellTower()
    {
        if( selectedTower != null)
        {
            Currency += selectedTower.Price / 2;


            selectedTower.GetComponentInParent<TileScript>().IsEmpty = true;


            Destroy(selectedTower.transform.parent.gameObject);

            DeselectTower();
        }

    }

    public void OnCurrencyChanged()
    {
        if ( Changed != null)
        {
            Changed();
         
        }
    }

    /// <summary>
    /// Handles escape presses
    /// </summary>
    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            Hover.Instance.Deactivate(); //deactivates hover icon
            if (selectedTower == null && !Hover.Instance.isVisible)
            {
                ShowInGameMenu();
            }

            else if ( Hover.Instance.isVisible) //if range is showing  
            {
                DropTower(); 
            }
            else if (selectedTower != null)
            {
                DeselectTower();
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0;
        }
    }

    public void StartWave()
    {
        wave++;

        waveText.text = string.Format("Wave: <color=lime>{0}</color>", wave);
        StartCoroutine(SpawnWave());

        waveBtn.SetActive(false);
    }


   /// <summary>
   /// Spawns a wave of monsters
   /// </summary>
   /// <returns></returns>
    private IEnumerator SpawnWave()
    {

        LevelManager.Instance.GeneratePath();

        for (int i = 0; i < wave; i++)
        {
            int monsterIndex = Random.Range(0, 2);

            string type = string.Empty;

            switch (monsterIndex)
            {
                case 0:
                    type = "Slime_1";
                    break;
                case 1:
                    type = "Slime_2";
                    break;
                    // case 2:
                    //     type = "Slime_2";
                    //     break;
            }


            //Requests the monster from the pool
            Monster monster = Pool.GetObject(type).GetComponent<Monster>();


            monster.Spawn(health);

            if (wave % 3 == 0)
            {
                health += 5;
            }

            //Adds the monster to the active monster list
            activeMonsters.Add(monster);

            yield return new WaitForSeconds(2.5f);

        }

    }

    public void RemoveMonster(Monster monster)
    {
        //remove monster from active list
        activeMonsters.Remove(monster);


        //if activeMonsters < 0
        if(!WaveActive & !gameOver)
        {
            //Shows the wave button
            waveBtn.SetActive(true);

        }

    }

    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            gameOverMenu.SetActive(true);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {

        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ShowStats()
    {

        //sets the panel active if not active (vice versa)
        statsPanel.SetActive(!statsPanel.activeSelf);

    }

    public void ShowSelectedTowerStats()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
        UpdateUpgradeTip();
    }

    public void SetToolTipText(string txt)
    {
        statText.text = txt;

    }

    public void UpdateUpgradeTip()
    {
        if (selectedTower != null)
        {
            sellText.text = (selectedTower.Price / 2).ToString(); // updates sell text
            SetToolTipText(selectedTower.GetStats());

            if(selectedTower.NextUpgrade != null)
            {
                upgradePrice.text = selectedTower.NextUpgrade.Price.ToString();
            }
       
        }
    }

    public void UpgradeTower()
    {
        if (selectedTower != null)
        {
            if (selectedTower.Level <= selectedTower.Upgrades.Length && Currency >= selectedTower.NextUpgrade.Price)
            {
                selectedTower.Upgrade();
            }
        }
    }


    public void ShowInGameMenu()
    {
        if (optionsMenu.activeSelf) //if options menu is active
        {
            ShowMainManu();
        }
        else
        {

            inGameMenu.SetActive(!inGameMenu.activeSelf);

            if (!inGameMenu.activeSelf)
            {
                //game resumes
                Time.timeScale = 1;
            }

            else
            {
                //game is stopped
                Time.timeScale = 0;
            }

        }
    }

    private void DropTower()
    {
        ClickedBtn = null;
        Hover.Instance.Deactivate();
    }


    public void ShowMainManu()
    {
        inGameMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void ShowOptions()
    {
        inGameMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
 
}
