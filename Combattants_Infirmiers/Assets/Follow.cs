using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform other;
    // Update is called once per frame
    void Update()
    {
        this.transform.position = other.position;
    }
}
