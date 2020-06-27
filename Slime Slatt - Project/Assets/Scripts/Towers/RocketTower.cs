using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTower : Tower
{
    private void Start()
    {
        ElementType = Element.ROCKET;


        Upgrades = new TowerUpgrades[]
        {

            //Flame Tower Constructors
            new TowerUpgrades(2,2,1,2),
            new TowerUpgrades(5,3,1,2),

        };
    }

    public override Debuff GetDebuff()
    {
        return new RocketDebuff(Target, DebuffDuration);
    }

    public override string GetStats()
    {
        return string.Format("<color=white>{0}</color>{1}", "<size=20>Rocket</size>", base.GetStats());

    }
}
