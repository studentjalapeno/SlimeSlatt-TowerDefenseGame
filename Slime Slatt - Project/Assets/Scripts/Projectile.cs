using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private Monster target;

    private Tower parent;

    private Animator myAnimator;

    private Element elementType;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
    }

    public void Initialize(Tower parent)
    {
        this.target = parent.Target;
        this.parent = parent;
        this.elementType = parent.ElementType;
        
    }

    private void MoveToTarget()
    {
        if ( target != null && target.IsActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * parent.ProjectileSpeed);

        }
        else if (!target.IsActive)
        {

            GameManager.Instance.Pool.ReleaseObject(gameObject); //reused in pool
      
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            if (target.gameObject == collision.gameObject)
            {
                target.TakeDamage(parent.Damage, elementType);

                myAnimator.SetTrigger("Impact");

                ApplyDebuff();
            }
            
        }    
    }

    private void ApplyDebuff()
    {
        if ( target.ElementType != elementType)
        {
            float roll = Random.Range(0, 100);

            if (roll <= parent.Proc)
            {
                target.AddDebuff(parent.GetDebuff());
            }
        }
        

      
    }


}
