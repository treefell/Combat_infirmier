using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZone : MonoBehaviour
{
    
    [HideInInspector]
    public int enteredPlayers = 0;

    public TextMesh myText;
    // Update is called once per frame
    void Update()
    {
        Debug.Log(enteredPlayers);
        if (enteredPlayers < 1)
        {
            myText.text = "";
        }
        else
        {
            myText.text = "Bring all 4!";
        }
        if (enteredPlayers >= 4)
        {
            Application.Quit();

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBase>())
        {

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
