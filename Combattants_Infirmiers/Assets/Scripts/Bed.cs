using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBase>())
        {
            if (collision.GetComponent<PlayerBase>().dragging)
            {
                Debug.Log("entered");
                collision.GetComponent<PlayerBase>().bedInRange = this.transform;
                collision.GetComponent<PlayerBase>().canDrop = true;

                collision.GetComponent<PlayerBase>().otherChar.GetComponent<PlayerBase>().thisBed = this.transform;
            }
        
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBase>())
        {
            
             collision.GetComponent<PlayerBase>().bedInRange = null;
             collision.GetComponent<PlayerBase>().canDrop = false;
            

        }
    }
}
