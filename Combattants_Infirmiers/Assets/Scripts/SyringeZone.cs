using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyringeZone : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> playersInZone = new List<GameObject> ();
    public GameObject message;
    public Nurse myPlayer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerBase>())
        {
            if (collision.gameObject.GetComponent<PlayerBase>() is Nurse)
            { 
            }
            else
            {
                 playersInZone.Add(collision.gameObject);
                if (playersInZone.Count > 0)
                {
                    myPlayer.canInject = true;
                    message.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerBase>())
        {
            if (collision.gameObject.GetComponent<PlayerBase>() is Nurse)
            {
            }
            else
            {
                playersInZone.Remove(collision.gameObject);
                if (playersInZone.Count <= 0)
                {
                    myPlayer.canInject = false;
                    message.SetActive(false);
                }
            }
        }
    }

    public GameObject GetTarget()
    {
        if (playersInZone.Count > 1)
        {
            float closestDist = Mathf.Infinity;
            GameObject closest = new GameObject();
            foreach (GameObject g in playersInZone)
            {
                if (Vector2.Distance(this.transform.position, g.transform.position) <= closestDist)
                {
                    closestDist = Vector2.Distance(this.transform.position, g.transform.position);
                    closest = g;
                }
            }
            return closest;
        }
        else
        {
            return playersInZone[0];
        }
    }
}
