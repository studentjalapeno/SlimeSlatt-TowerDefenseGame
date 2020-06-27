﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameTower : Tower
{

    [SerializeField]
    private float tickTime;

    [SerializeField]
    private FlameSplash splashPrefab;

    [SerializeField]
    private int splashDamage;

    public int SplashDamage { get => splashDamage;  }
    public float TickTime { get => tickTime;  }

    private void Start()
    {
        ElementType = Element.FIRE;


        Upgrades = new TowerUpgrades[]
        {

            //Flame Tower Constructors
            new TowerUpgrades(2,1,0.5f,5,-0.1f,1),
            new TowerUpgrades(5,1,0.5f,5,-0.1f,1),
            

        };


    }

    public override Debuff GetDebuff()
    {
        return new FlameDebuff(splashDamage, tickTime, DebuffDuration, splashPrefab, Target);
    }

    public override string GetStats()
    {
        if (NextUpgrade != null)
        {
            return string.Format("<color=#00ff00ff>{0}</color>{1} \nTick time: {2} <color=#00ff00ff>{4}</color>\nSplash damage: {3} <color=#00ff00ff>+{5}</color>", "<size=20><b>Flame</b></size>", base.GetStats(), TickTime, SplashDamage, NextUpgrade.TickTime, NextUpgrade.SpecialDamage);
        }

        return string.Format("<color=#00ff00ff>{0}</color>{1} \nTick time: {2}\nSplash damage: {3}", "<size=20><b>Flame</b></size>", base.GetStats(), TickTime, SplashDamage);

    }

    public override void Upgrade()
    {

        this.splashDamage += NextUpgrade.SpecialDamage;
        this.tickTime -= NextUpgrade.TickTime;
        base.Upgrade();
    }

}

