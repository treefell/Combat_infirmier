using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawn : MonoBehaviour
{
    float currentTime = 0f;
    private List<Vector4> exceptions;
    public GameObject crate;
    private Vector4 thisArea;

    private void Start()
    {
        thisArea = GetMultipliedBounds(this.gameObject, 1);
        exceptions = new List<Vector4>();
        currentTime = GameManagerPersistent.Instance.crateSpawnTime - 1f;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= GameManagerPersistent.Instance.crateSpawnTime)
        {
            SpawnCrate();
        }
    }

    private void SpawnCrate()
    {
        exceptions.Clear();
        exceptions.Add(GetMultipliedBounds(GameManagerPersistent.Instance.NurseA, 2));
        exceptions.Add(GetMultipliedBounds(GameManagerPersistent.Instance.NurseB, 2));
        exceptions.Add(GetMultipliedBounds(GameManagerPersistent.Instance.BoxerA, 2));
        exceptions.Add(GetMultipliedBounds(GameManagerPersistent.Instance.BoxerB, 2));

        GameObject[] levelObjects = GameObject.FindGameObjectsWithTag("LevelObjects");
        foreach (GameObject g in levelObjects)
        {
            exceptions.Add(GetMultipliedBounds(g, 1));
        }

        GameObject clone = Instantiate(crate);
        clone.transform.position = FindPosition(clone);
        currentTime = 0f;
    }

    

    private Vector3 FindPosition(GameObject enemy)
    {
        float posX = 0;
        float posY = 0;
        bool foundPos = false;
        int iterations = 0;

        while (foundPos == false) // archi sale il recommence sil tombe sur une exception
        {
            iterations += 1;
            if (iterations >= 100) // securite pour pas qu'il boucle à l'infini
            {
                break;
            }
            foundPos = true;
            posX = Random.Range(thisArea.x, thisArea.y);
            posY = Random.Range(thisArea.z, thisArea.w);
            foreach (Vector4 e in exceptions)
            {
                if (posX >= e.x && posX <= e.y && posY >= e.z && posY <= e.w)
                {
                    foundPos = false;
                    break;
                }
            }

        }


        return new Vector3(posX, posY, 0);
    }

    //fonction pour pas surcharger de texte plus haut. renvoient les valeurs exception pour pas faire spawn les ennemis sur les joueurs, ni les uns sur les autres. ORDRE : Xa Xb Ya Yb
    private Vector4 GetMultipliedBounds(GameObject obj, float factor)
    {
        return new Vector4(obj.GetComponent<Collider2D>().bounds.center.x - factor * obj.GetComponent<Collider2D>().bounds.extents.x, obj.GetComponent<Collider2D>().bounds.center.x + factor * obj.GetComponent<Collider2D>().bounds.extents.x,
            obj.GetComponent<Collider2D>().bounds.center.y - factor * obj.GetComponent<Collider2D>().bounds.extents.y, obj.GetComponent<Collider2D>().bounds.center.y + factor * obj.GetComponent<Collider2D>().bounds.extents.y);
    }


    
}
