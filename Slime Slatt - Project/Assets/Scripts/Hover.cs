using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover>
{

    private SpriteRenderer spriteRenderer; //creates reference to SpriteRenderer component

    private SpriteRenderer rangeSpriteRenderer; //a reference to the rangecheck on the tower

    public bool isVisible { get; private set; }

    // Start is called before the first frame update
    void Start()
    {

        //Creates the reference to the sprite renderer
        this.spriteRenderer = GetComponent<SpriteRenderer>();

        this.rangeSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        //Makes sure that we follow the mouse
        FollowMouse();
    }


    /// <summary>
    /// Makes the hover icon follow the mouse
    /// </summary>
    private void FollowMouse()
    {
        if(spriteRenderer.enabled) //only follows mouse if spriterender is enabled (tower icon)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0); //z is set to 0 

        }
        
    }
    /// <summary>
    /// Activates the hover icon
    /// </summary>
    /// <param name="sprite"></param>
    public void Activate(Sprite sprite)
    {
        //Sets the correct sprite
        this.spriteRenderer.sprite = sprite;


        //Enables the renderer
        spriteRenderer.enabled = true;

        //Enables the renderer
        rangeSpriteRenderer.enabled = true;

        isVisible = true;

    }


    /// <summary>
    /// Deactivates the hover icon (hides it)
    /// </summary>
    public void Deactivate()
    {

        //Disables the renderer so that we cant see it
        spriteRenderer.enabled = false;


        rangeSpriteRenderer.enabled = false;

        //Unclicks our button
        GameManager.Instance.ClickedBtn = null; //sets clickbtn(selected tower) = null so user cannot place tower

        isVisible = false;

       
    }
}
