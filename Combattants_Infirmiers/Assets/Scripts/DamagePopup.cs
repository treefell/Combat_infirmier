using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public float speed = 1;
    public float timeTillDisappears = 1;
    public float currentTime = 0;

    public Vector2 offset;
    TextMesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh = this.GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * speed / 100;
        currentTime += Time.deltaTime;
        //fade out somehow
        if (currentTime >= timeTillDisappears)
        {
            Destroy(this.gameObject);
        }
    }


    public void SetColor(Color color)
    {
        this.GetComponent<TextMeshPro>().color = color;
    }
    public void SetTxt(string txt)
    {
        this.GetComponent<TextMeshPro>().text = txt;
    }

  
}
