using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    private Camera cam;
    public Transform player1;
    public Transform player2;
    private Vector2 targetPosition;
    private float camSpeed = 6f;

    public CamCenter camCenter;
    public CamCenter smallCenter;

    public int allChars = 4;

    private float minsize;

    // Start is called before the first frame update
    void Start()
    {
        cam = this.GetComponent<Camera>();
        minsize = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (camCenter.GetCharsInBox() < allChars)
        {
            Zoom(1);
            
        }
        else if (smallCenter.GetCharsInBox() >= allChars && cam.orthographicSize > minsize)
        {
            Zoom(-1);
        }


        
        targetPosition = (player1.position + player2.position)/2;
        this.transform.position = Vector3.Lerp(transform.position,new Vector3(targetPosition.x, targetPosition.y, transform.position.z), Time.deltaTime*camSpeed);
    }

    void Zoom(int zoomFactor)
    {
        float oldsize = cam.orthographicSize;

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, cam.orthographicSize+zoomFactor, Time.deltaTime*camSpeed);
        camCenter.GetComponent<BoxCollider2D>().size *= cam.orthographicSize / oldsize;
        smallCenter.GetComponent<BoxCollider2D>().size *= cam.orthographicSize / oldsize;

    }
}
