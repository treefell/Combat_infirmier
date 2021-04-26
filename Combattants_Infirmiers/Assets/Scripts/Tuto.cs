using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuto : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
    }
    private void Update()
    {
        if (Input.GetButtonDown("A"))
        {
            Time.timeScale = 1f;
            Destroy(this.gameObject);
        }
    }

}
