using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketDebuff : Debuff
{
    public RocketDebuff(Monster target, float duration) : base(target, duration)
    {
       if ( target !=  null)
        {
            target.Speed = 0;

        }
    }

    public override void Update()
    {
        if (target != null)
        {
            target.Speed = 0;
        }

        base.Update();
    }

    public override void Remove()
    {
        if ( target != null)
        {
            target.Speed = target.MaxSpeed;
            base.Remove();
        }

    }


}

