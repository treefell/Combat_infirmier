using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class MenuBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    int i;
    Button[] buttons;
    
    void Start()
    {
        int i = 0;
        buttons = this.GetComponentsInChildren<Button>(true);
        buttons[i].Select();
    }
 
    public void Start_Game()
    {
    	SceneManager.LoadScene("TeamSelection");
    }
       
    public void Option()
    {
    		
    }
    
    public void Quit_Game()
    {
    	Application.Quit();
    }
	
    // Update is called once per frame
    void Update()
    {
        buttons[i].Select();
        i = (i +((int)Input.GetAxis("Vertical"))) % 2;


    }
}
