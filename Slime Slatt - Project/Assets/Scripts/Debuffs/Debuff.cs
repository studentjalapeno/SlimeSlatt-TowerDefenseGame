﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Debuff
{
    protected Monster target;

    private float duration;

    private float elasped;

    public Debuff(Monster target, float duration)
    {
        this.duration = duration;
        this.target = target;
    }

    public virtual void Update()
    {
        elasped += Time.deltaTime;

        if (elasped >= duration)
        {
            Remove();
        }

    }

    public virtual void Remove()
    {
        if ( target != null)
        {
            target.RemoveDebuffs(this);
        }
    }
}
