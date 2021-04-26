using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public float currentSeconds = 0f;
    public int currentMinutes = 5;

    private Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        currentSeconds -= Time.deltaTime;
        if (currentSeconds <= 0)
        {
            if (currentMinutes > 0)
            {
                currentMinutes -= 1;
                currentSeconds += 60;
            }
            else
            {
                if (currentSeconds < 10)
                {
                    timerText.text = currentMinutes + ":0" + Mathf.Floor(currentSeconds);
                }
                else
                {
                    timerText.text = currentMinutes + ":" + Mathf.Floor(currentSeconds);
                }
                GameManagerPersistent.Instance.GameOver();
            }

        }

        if (currentSeconds < 10)
        {
            timerText.text = currentMinutes + ":0" + Mathf.Floor(currentSeconds);
        }
        else
        {
            timerText.text = currentMinutes + ":" + Mathf.Floor(currentSeconds);
        }
               
    }


}
