using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public Item item;
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
        if (collision.GetComponent<Nurse>())
        {
            if (collision.GetComponent<Nurse>().heldItem == null || collision.GetComponent<Nurse>().heldItem.injection == Injection.Heal)
            {
                collision.GetComponent<Nurse>().GetCrate(item);

                Destroy(this.gameObject);
            }
            
        }

        
    }
}
