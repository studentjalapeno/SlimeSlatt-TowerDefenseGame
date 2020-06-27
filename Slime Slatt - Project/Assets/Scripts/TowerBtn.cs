using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour
{


    /// <summary>
    /// The prefab that this button will spawn
    /// </summary>
    [SerializeField]
    private GameObject towerPrefab;

    /// <summary>
    /// A reference to the towers sprite
    /// </summary>
    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private int price;

    [SerializeField]
    private Text priceTxt;



    //property for accessing the buttons prefab (enables a class to expose a public way of getting and setting values)
    public GameObject TowerPrefab { get => towerPrefab; }
    
    //get sprite 
    public Sprite Sprite { get => sprite; }

    //property for accessing the tower buttons price
    public int Price { get => price; set => price = value; }

    private void Start()
    {
        priceTxt.text = " " + Price;


        //Listener event ( everytime currency is changed this is called )
        GameManager.Instance.Changed += new CurrencyChanged(PriceCheck);
    }

    private void PriceCheck()
    {

        //if player can afford the tower
        if (price <= GameManager.Instance.Currency)
        {
            GetComponent<Image>().color = Color.white;
            priceTxt.color = Color.white;

        }
        else //if  player cannot afford tower
        {
            GetComponent<Image>().color = Color.grey; //change color of button
            priceTxt.color = Color.grey;
        }


    }    

    public void ShowInfo(string type)
    {
        string tooltip = string.Empty;

        switch (type)
        {
            case "Cannon":
                CannonTower cannon = towerPrefab.GetComponentInChildren<CannonTower>();
                tooltip = string.Format("<color=#ffa500ff><size=20><b>Cannon</b></size></color>\nDamage: {0} \nProc: {1}%\nDebuff duration: {2}sec \nTick time: {3} sec \nTick damage: {4}\nHas a chance to apply a damage over time debuff", cannon.Damage, cannon.Proc, cannon.DebuffDuration, cannon.TickTime, cannon.TickDamage);
                break;

            case "Flame":
                FlameTower flame = towerPrefab.GetComponentInChildren<FlameTower>();
                tooltip = string.Format("<color=#00ff00ff><size=20><b>Flame</b></size></color>\nDamage: {0} \nProc: {1}%\nDebuff duration: {2}sec \nTick time: {3} sec \nSplash damage: {4}\nCan apply flame damage", flame.Damage, flame.Proc, flame.DebuffDuration, flame.TickTime, flame.SplashDamage);
                break;


            case "Freeze":

                FreezeTower freeze = towerPrefab.GetComponentInChildren<FreezeTower>();
                tooltip = string.Format("<color=#00fffff><size=20><b>Freeze</b></size></color>\nDamage: {0} \nProc: {1}% \nDebuff duration: {2}sec \nSlowing factor: {3}% \n Has a chance at slowing down slimes",
                                         freeze.Damage, freeze.Proc, freeze.DebuffDuration, freeze.SlowingFactor);
                break;

            case "Rocket":

                RocketTower rocket = towerPrefab.GetComponentInChildren<RocketTower>();
                tooltip = string.Format("<color=#add8e6ff><size=20><b>Rocket</b></size></color>\nDamage: {0} \nProc: {1}%\nDebuff duration: {2}sec\n Has a chance to stun slimes", rocket.Damage, rocket.Proc, rocket.DebuffDuration);
                break;
           
      
        }


        GameManager.Instance.SetToolTipText(tooltip);
        GameManager.Instance.ShowStats();
    }

}
