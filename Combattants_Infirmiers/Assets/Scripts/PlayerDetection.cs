using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{

    //mostly a copy of room manager, some of the code doesnt make sense

    private List<GameObject> playersInside = new List<GameObject>();

    private ContactFilter2D filter = new ContactFilter2D();
    private List<Collider2D> colliders = new List<Collider2D>();
    private void Start()
    {
        Physics2D.OverlapCollider(this.GetComponent<Collider2D>(), filter, colliders);
        foreach (Collider2D col in colliders)
        {
            if (col.GetComponent<PlayerBase>())
            {
                playersInside.Add(col.gameObject);
            }
        }
        CheckPlayerCount();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBase>() && !playersInside.Contains(collision.gameObject))
        {
            playersInside.Add(collision.gameObject);
            CheckPlayerCount();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBase>() && playersInside.Contains(collision.gameObject))
        {
            playersInside.Remove(collision.gameObject);
            CheckPlayerCount();
        }
    }

    private void Update()
    {

        

    }


    private void CheckPlayerCount()
    {
        if (playersInside.Count >= 1)
        {
            PlayerDetected();
        }
        
    }
    private void PlayerDetected()
    {
        this.transform.parent.GetComponent<AIController>().enabled = true;

    }
}
