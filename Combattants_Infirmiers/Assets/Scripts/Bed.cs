using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    List<GameObject> playersInRange;
    // Start is called before the first frame update
    void Start()
    {
        playersInRange = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playersInRange.Count > 0)
        {
            foreach (GameObject g in playersInRange)
            {
                if (g.GetComponent<PlayerBase>().dragging)
                {
                    g.GetComponent<PlayerBase>().bedInRange = this.transform;
                    g.GetComponent<PlayerBase>().canDrop = true;

                    g.GetComponent<PlayerBase>().otherChar.GetComponent<PlayerBase>().thisBed = this.transform;
                }
                else
                {
                    g.GetComponent<PlayerBase>().bedInRange = null;
                    g.GetComponent<PlayerBase>().canDrop = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBase>())
        {
            playersInRange.Add(collision.gameObject);
                 
            
            
        
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBase>())
        {
            playersInRange.Remove(collision.gameObject);
            collision.GetComponent<PlayerBase>().bedInRange = null;
             collision.GetComponent<PlayerBase>().canDrop = false;
            

        }
    }
}
