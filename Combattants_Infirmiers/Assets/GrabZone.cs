using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabZone : MonoBehaviour
{
    private bool full = true;
    private float currentTime = 0f;
    public Item[] items;
    public Sprite fullSprite;
    public Sprite emptySprite;
    public Item healItem;
    private SpriteRenderer mySprite;

    public GameObject message;
    // Start is called before the first frame update
    void Start()
    {
        mySprite = transform.parent.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!full)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= GameManagerPersistent.Instance.shelfRefillTime)
            {
                full = true;
                currentTime = 0f;
                mySprite.sprite = fullSprite;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Nurse>() != null)
        {
            if (collision.GetComponent<Nurse>().heldItem != null )
            {
                if (collision.GetComponent<Nurse>().heldItem.injection == Injection.Crate)
                {
                    message.SetActive(true);
                    collision.GetComponent<Nurse>().canGrab = true;
                    collision.GetComponent<Nurse>().armoire = this;
                }
            }
            else if(full)
            {
                message.SetActive(true);
                collision.GetComponent<Nurse>().canGrab = true;
                collision.GetComponent<Nurse>().armoire = this;
            }
            

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Nurse>() != null)
        {
            message.SetActive(false);
            collision.GetComponent<Nurse>().canGrab = false;
            collision.GetComponent<Nurse>().armoire = null;
        }
    }

    public void EmptyShelf()
    {
        full = false;
        currentTime = 0f;
        mySprite.sprite = emptySprite;

    }
}
