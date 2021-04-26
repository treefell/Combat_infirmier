using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxManager : MonoBehaviour
{
    public int destroyAfter;
    private int currentFrame;

    // Start is called before the first frame update
    void Start()
    {
        currentFrame = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentFrame += 1;
        if (currentFrame >= destroyAfter)
        {
            Destroy(this.gameObject);
        }
    }
}
