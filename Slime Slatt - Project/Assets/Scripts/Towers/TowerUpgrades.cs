using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrades : MonoBehaviour
{


    /// <summary>
    /// Reason these propertities are private set, is because we dont want to be able to set them from the outside
    /// </summary>
    public int Price { get; private set; }

    public int Damage { get; private set; }

    public float DebuffDuration { get; private set; }

    public float ProcChance { get; private set; }

    public float SlowingFactor { get; private set; } //exclusivley for Freeze tower

    public float TickTime { get; private set; }

    public int SpecialDamage { get; private set; } //Flame tower


    //Constructor cannon
    public TowerUpgrades(int price, int damage, float debuffduaration, float procChance)
    {
        this.Price = price;
        this.Damage = damage;
        this.DebuffDuration = debuffduaration;
        this.ProcChance = procChance;
    }

        //Constructor freeze
    public TowerUpgrades(int price, int damage, float debuffduaration, float procChance, float slowingFactor)
    {
        this.Price = price;
        this.Damage = damage;
        this.DebuffDuration = debuffduaration;
        this.ProcChance = procChance;
        this.SlowingFactor = slowingFactor;
    }

    //Constructor flame
    public TowerUpgrades(int price, int damage, float debuffduaration, float procChance, float tickTime, int specialDamage)
    {
        this.Price = price;
        this.Damage = damage;
        this.DebuffDuration = debuffduaration;
        this.ProcChance = procChance;
        this.TickTime = tickTime;
        this.SpecialDamage = specialDamage;
    }




}
