using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTower : Tower
{
   
    [SerializeField]
    private float slowingFactor;

    public float SlowingFactor { get => slowingFactor; }

    private void Start()
    {
        //Sets the tower elements
        ElementType = Element.FREEZE;

        Upgrades = new TowerUpgrades[]
        {
            new TowerUpgrades(2,1,1,2,10),
            new TowerUpgrades(2,1,1,2,20),
        };
    }

    /// <summary>
    /// Gets the tower's debuff
    /// </summary>
    /// <returns>A freeze debuff</returns>
    public override Debuff GetDebuff()
    {
        return new FrostDebuff(SlowingFactor, DebuffDuration, Target);
    }

    public override string GetStats()
    {
        if (NextUpgrade != null)  //If the next is avaliable
        {
            return string.Format("<color=#00ffffff>{0}</color>{1} \nSlowing factor: {2}% <color=#00ff00ff>+{3}</color>", "<size=20><b>Freeze</b></size>", base.GetStats(), SlowingFactor, NextUpgrade.SlowingFactor);
        }

        //Returns the current upgrade
        return string.Format("<color=#00ffffff>{0}</color>{1} \nSlowing factor: {2}%", "<size=20><b>Freeze</b></size>", base.GetStats(), SlowingFactor);
    }

    public override void Upgrade()
    {
        this.slowingFactor = NextUpgrade.SlowingFactor;
        base.Upgrade();
    }

}
