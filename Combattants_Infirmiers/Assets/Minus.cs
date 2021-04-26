using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minus : MonoBehaviour
{
    public TimeZone timeZone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBase>())
        {

            timeZone.playersLeft += 1;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBase>())
        {
            timeZone.playersLeft -= 1;
        }
    }
}
