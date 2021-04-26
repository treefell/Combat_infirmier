using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlinkArrow : MonoBehaviour
{
    float Timer = 0.0f;
    public GameObject arrow; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void SlowBlink()
    {
        Timer += Time.deltaTime;
        if (Timer % 1 > 0.5)
            arrow.GetComponent<SpriteRenderer>().enabled = true;
        else
            arrow.GetComponent<SpriteRenderer>().enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        SlowBlink();
    }
}
