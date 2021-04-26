using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZone : MonoBehaviour
{
    [HideInInspector]
    public int enteredPlayers = 0;
    [HideInInspector]
    public int playersLeft = 0;
    [HideInInspector]
    public int playersRight = 0;

    private float increment;
    public float speed = 1f;
    public TextMesh myText;
    public GameObject plusText;

    public GameObject minusText;
    // Update is called once per frame
    void Update()
    {
        if (enteredPlayers > 0)
        {
            increment += (playersRight - playersLeft) * Time.deltaTime * speed;
            if (Mathf.Abs(increment) >= 1f)
            {
                if (increment < 0)
                {
                    GameManagerPersistent.Instance.gameTimeSeconds -= Mathf.FloorToInt(Mathf.Abs(increment));
                }
                else
                {
                    GameManagerPersistent.Instance.gameTimeSeconds += Mathf.FloorToInt(Mathf.Abs(increment));
                }
                
                if (GameManagerPersistent.Instance.gameTimeSeconds >= 60)
                {
                    GameManagerPersistent.Instance.gameTimeSeconds -= 60;
                    GameManagerPersistent.Instance.gameTimeMinutes += 1;
                }
                if (GameManagerPersistent.Instance.gameTimeSeconds < 0)
                {
                    GameManagerPersistent.Instance.gameTimeSeconds = 59;
                    GameManagerPersistent.Instance.gameTimeMinutes -= 1;
                }

                if (increment < 0)
                {
                    increment += Mathf.FloorToInt(Mathf.Abs(increment));
                }
                else
                {
                    increment -= Mathf.FloorToInt(increment);
                }
                
            }

        }

        if (GameManagerPersistent.Instance.gameTimeSeconds <= 9)
        {
            myText.text = GameManagerPersistent.Instance.gameTimeMinutes.ToString() + ":0" + GameManagerPersistent.Instance.gameTimeSeconds.ToString();
        }
        else
        {
            myText.text = GameManagerPersistent.Instance.gameTimeMinutes.ToString() + ":" + GameManagerPersistent.Instance.gameTimeSeconds.ToString();
        }
        
        
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBase>())
        {

            enteredPlayers += 1;
            plusText.SetActive(true);
            minusText.SetActive(true);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBase>())
        {
            enteredPlayers -= 1;
            if (enteredPlayers <= 0)
            {

                plusText.SetActive(false);
                minusText.SetActive(false);
            }

        }
    }
}
