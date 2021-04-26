using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpDisplay : MonoBehaviour
{

    private Slider bar;
    private Text text;
    public Fighter player;

    // Start is called before the first frame update
    void Start()
    {
        
        
        bar = GetComponentInChildren<Slider>();
        text = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            if (gameObject.name == "hpdisplay1")
            {
                player = GameManagerPersistent.Instance.BoxerA.GetComponent<Fighter>();
            }
            else
            {
                player = GameManagerPersistent.Instance.BoxerB.GetComponent<Fighter>();
            }
        }
        
        bar.value = (float)player.currentHp / (float)player.hp;
        text.text = "HP: " + player.currentHp + " / " + player.hp;
    }
}
