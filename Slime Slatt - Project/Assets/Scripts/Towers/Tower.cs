using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Element { CANNON , FREEZE , ROCKET, FIRE, NONE }



public abstract class Tower : MonoBehaviour
{

    [SerializeField]
    private string projectileType;

    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    private int damage; // the damage the projectile will deal

    [SerializeField]
    private float attackCooldown;

    [SerializeField]
    private float debuffDuration;

    [SerializeField]
    private float proc;

    private bool canAttack = true;

    private float attackTimer;

    //private Monster target;

    private SpriteRenderer mySpriteRenderer; //the towers sprite renderer

    private Queue<Monster> monsters = new Queue<Monster>(); //a queue of monsters

    public int Level { get; protected set; }

    public Element ElementType { get; protected set; } //the element type of the projectile 

    public int Price { get; set; } //the projectiles price

    public float ProjectileSpeed { get => projectileSpeed; } //projectile speed

    public Monster Target { get; private set; } //the towers current target

    public int Damage { get => damage;  }
    public float DebuffDuration { get => debuffDuration; set => debuffDuration = value; }
    public float Proc { get => proc; set => proc = value; }

    public TowerUpgrades NextUpgrade 
    {
        get
        {
            if (Upgrades.Length > Level-1)
            {
                return Upgrades[Level - 1];
            }

            return null;
        }         
    }

    public TowerUpgrades[] Upgrades { get; protected set; }




    // Start is called before the first frame update
    void Awake()
    {
        
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        Level = 1;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Target);
        Attack();


    }

    /// <summary>
    /// Selects the tower
    /// </summary>
    public void Select()
    {
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
        
        //everytime tower is selected it updates tool tip
        GameManager.Instance.UpdateUpgradeTip();
    }

    /// <summary>
    /// Makes the tower attack a target
    /// </summary>

    private void Attack()
    {
        if (!canAttack) //if we cant attack
        {
            //Count how much time has passed since last attack
            attackTimer += Time.deltaTime;

            if ( attackTimer >= attackCooldown)
            {
                canAttack = true;
                attackTimer = 0; //reset timer
            }
        }

        if (Target == null && monsters.Count > 0 && monsters.Peek().IsActive) //if list has something in it
        {
            Target = monsters.Dequeue(); //removes first item from queue and set equal to target
        }

        if (Target != null && Target.IsActive) //if we a target that is active
        {

            if (canAttack) //if we can attack then we shoot at target
            {
                Shoot();

                canAttack = false;
            }
            

        }



        if ( Target !=null && !Target.IsAlive || Target != null && !Target.IsActive)
        {
            Target = null;
        }

    }

    public virtual string GetStats()
    {
        if (NextUpgrade != null)
        {
            return string.Format("\nLevel: {0} \nDamage: {1} <color=#00ff00ff> +{4}</color>\nProc: {2}% <color=#00ff00ff>+{5}%</color>\nDebuff: {3}sec <color=#00ff00ff>+{6}</color>", Level, damage, proc, DebuffDuration, NextUpgrade.Damage, NextUpgrade.ProcChance, NextUpgrade.DebuffDuration);
        }

        return string.Format("\nLevel: {0} \nDamage: {1} \nProc: {2}% \nDebuff: {3}sec", Level, damage, proc, DebuffDuration);
    }

    /// <summary>
    /// Makes the tower shoot
    /// </summary>
    private void Shoot()
    {
        //Gets a projectile from the object pool
        Projectile projectile = GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();

        //Makes sure that the projectile is instantiated on the towers position
        projectile.transform.position = transform.position;

        projectile.Initialize(this); //this == target
    }

    public virtual void Upgrade()
    {
        GameManager.Instance.Currency -= NextUpgrade.Price;
        Price += NextUpgrade.Price;
        this.damage += NextUpgrade.Damage;
        this.proc += NextUpgrade.ProcChance;
        this.DebuffDuration += NextUpgrade.DebuffDuration;
        Level++;
        GameManager.Instance.UpdateUpgradeTip();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster" ) //Adds new monsters to the queue when they enter the range
        {
            
            monsters.Enqueue(collision.GetComponent<Monster>());
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(Target.tag == "Monster")
        {
            Target = null;
        }
    }

    public abstract Debuff GetDebuff();


}
