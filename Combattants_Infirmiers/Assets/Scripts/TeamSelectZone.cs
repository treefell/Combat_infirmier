using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamSelectZone : MonoBehaviour
{
    

    public ControlledChar perso;
    [HideInInspector]
    public int enteredPlayers = 0;

    public TextMesh myText;
    // Update is called once per frame
    void Update()
    {
        if (enteredPlayers > 1)
        {
            myText.text = "Get outta here!";
        }
        else if (enteredPlayers == 1)
        {
            myText.text = "OK!";
        }
        else
        {
            myText.text = "";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBase>())
        {
            
            GameManagerPersistent.Instance.SetControlledChar(perso, collision.GetComponent<PlayerBase>().port);
            
            enteredPlayers += 1;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBase>())
        {


            enteredPlayers -= 1;

        }
    }
}
