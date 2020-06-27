﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTower : Tower
{
    [SerializeField]
    private float tickTime;

    [SerializeField]
    private float tickDamage;

    public float TickDamage { get => tickDamage; }
    public float TickTime { get => tickTime; }

    private void Start()
    {
        ElementType = Element.CANNON;


        Upgrades = new TowerUpgrades[]
        {

            //Cannon Tower Constructors
            new TowerUpgrades(2,2,.5f,5,-0.1f,1),
            new TowerUpgrades(5,3,.5f,5,-0.1f,1),

        };
    }

    public override Debuff GetDebuff()
    {
        return new CannonDebuff(TickDamage, TickTime, DebuffDuration, Target);
    }

    public override string GetStats()
    {
        if (NextUpgrade != null) //If the next is avaliable
        {
            return string.Format("<color=#ffa500ff>{0}</color>{1} \nTick time: {2} <color=#00ff00ff>{4}</color>\nTick damage: {3} <color=#00ff00ff>+{5}</color>", "<size=20><b>Cannon</b></size> ", base.GetStats(), TickTime, TickDamage, NextUpgrade.TickTime, NextUpgrade.SpecialDamage);
        }

        //Returns the current upgrade
        return string.Format("<color=#ffa500ff>{0}</color>{1} \nTick time: {2}\nTick damage: {3}", "<size=20><b>Cannon</b></size> ", base.GetStats(), TickTime, TickDamage);
    }

    public override void Upgrade()
    {
        this.tickTime -= NextUpgrade.TickTime;
        this.tickDamage += NextUpgrade.SpecialDamage;
        base.Upgrade();
    }
}

