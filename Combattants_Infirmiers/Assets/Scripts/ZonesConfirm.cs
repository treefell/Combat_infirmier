using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ZonesConfirm : MonoBehaviour
{
    private int okZones = 0;
    public TeamSelectZone[] zones;

    // Update is called once per frame
    void Update()
    {
        okZones = 0;
        foreach (TeamSelectZone z in zones)
        {
            if (z.enteredPlayers == 1)
            {
                okZones += 1;
            }
        }
        if (okZones >= zones.Length)
        {
            GameManagerPersistent.Instance.ActivateHud();
            
            SceneManager.LoadScene("Level");
            GameManagerPersistent.Instance.InitializeVar();
        }
    }
}
