using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Stat
{

    [SerializeField]
    private HealthBar healthBar;

    [SerializeField]
    private float maxVal;

    [SerializeField]
    private float currentVal;

    public float CurrentVal
    { 
        get => currentVal; 
        
        set
        {
            this.currentVal = Mathf.Clamp(value,0,MaxVal);
            HealthBar.Value = currentVal;
        }

    }
    
    public float MaxVal 
    { 
        get => maxVal; 
        
        set
        {
            this.maxVal = value;
            HealthBar.MaxValue = value;
         
        }
    
    }

    public HealthBar HealthBar { get => healthBar;  }

    public void Initialize()
    {
        this.MaxVal = maxVal;
        this.CurrentVal = currentVal;
        

    }
}
