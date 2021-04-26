using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabZone : MonoBehaviour
{

    public Item[] items;

    public GameObject message;
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
        if (collision.GetComponent<Nurse>() != null && collision.GetComponent<Nurse>().heldItem == null)
        {
            message.SetActive(true);
            collision.GetComponent<Nurse>().canGrab = true;
            collision.GetComponent<Nurse>().armoire = this;

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
}
