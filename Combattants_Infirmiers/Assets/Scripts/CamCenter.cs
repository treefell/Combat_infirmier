using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamCenter : MonoBehaviour
{

    private List<Collider2D> colliders = new List<Collider2D>();

    ContactFilter2D filter = new ContactFilter2D();

    private int charsInBox = 0;
    // Start is called before the first frame update
    void Start()
    {

        Physics2D.OverlapCollider(this.GetComponent<Collider2D>(), filter, colliders);
        foreach (Collider2D col in colliders)
        {
            if (col.GetComponent<PlayerBase>())
            {
                charsInBox += 1;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (charsInBox < 2 && collision.GetComponent<PlayerBase>())
        {
            charsInBox += 1;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBase>())
        {
            charsInBox -= 1;
        }
    }


    public int GetCharsInBox()
    {

        return charsInBox;
    }


}
