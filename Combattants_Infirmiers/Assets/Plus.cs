using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plus : MonoBehaviour
{
    public TimeZone timeZone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBase>())
        {

            timeZone.playersRight += 1;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBase>())
        {
            timeZone.playersRight -= 1;
        }
    }
}
