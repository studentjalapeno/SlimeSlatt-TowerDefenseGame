using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameDebuff : Debuff
{
    private float timeSinceTick;

    private float tickTime;

    private FlameSplash splashPrefab;

    private int splashDamage;

    public FlameDebuff(int splashDamage, float tickTime, float duration , FlameSplash splashPrefab, Monster target) : base(target,duration)
    {


        this.splashDamage = splashDamage;
        this.tickTime = tickTime;
        this.splashPrefab = splashPrefab;



    }

    public override void Update()
    {
        if (target != null)
        {
            timeSinceTick += Time.deltaTime;

            if(timeSinceTick >= tickTime)
            {
                timeSinceTick = 0;
                Splash();
            }

        }    

        base.Update();
    }

    private void Splash()
    {
        FlameSplash tmp = GameObject.Instantiate(splashPrefab, target.transform.position, Quaternion.identity);

        tmp.Damage = splashDamage;

        Physics2D.IgnoreCollision(target.GetComponent<Collider2D>(), tmp.GetComponent<Collider2D>());

    }


}
