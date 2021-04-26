using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RezZone : MonoBehaviour
{

    public GameObject rezMes;
    private bool canRez = false;
    private PlayerBase otherPlayer;
    
    
    //au cas ou pour faire un check au debut s'il est deja dedans
    private ContactFilter2D filter = new ContactFilter2D();
    private List<Collider2D> colliders = new List<Collider2D>();
    // Start is called before the first frame update
    void Start()
    {
        //bar = rezMes.GetComponentInChildren<Slider>();

        //check au debut
        Physics2D.OverlapCollider(this.GetComponent<Collider2D>(), filter, colliders);
        foreach (Collider2D col in colliders)
        {
            if (col.GetComponent<PlayerBase>() && col.gameObject.layer == transform.parent.gameObject.layer)
            {
                PlayerEnters(col);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBase>() && collision.gameObject.layer == transform.parent.gameObject.layer)
        {
            PlayerEnters(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBase>() && collision.gameObject.layer == transform.parent.gameObject.layer)
        {
            rezMes.SetActive(false);
            canRez = false;

            if (otherPlayer == null) { return; }
            otherPlayer.otherRezZone = null;
            otherPlayer.canRez = false;
            otherPlayer = null;
        }
    }

    public void Deactivate()
    {
        rezMes.SetActive(false);
        otherPlayer = null;
        this.gameObject.SetActive(false);
    }

    private void PlayerEnters(Collider2D colli)
    {
        rezMes.SetActive(true);
        canRez = true;
        otherPlayer = colli.GetComponent<PlayerBase>();
        otherPlayer.otherRezZone = this;
        otherPlayer.canRez = true;
    }
}
