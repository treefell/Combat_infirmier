using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
       
    }
}
