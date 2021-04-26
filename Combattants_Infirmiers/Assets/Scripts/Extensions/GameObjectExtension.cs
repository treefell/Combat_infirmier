using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtension
{
    public static GameObject FindClosest(GameObject initialObject, string tag)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;


        foreach (GameObject obj in objs)
        {
            float distance = Vector3.Distance(obj.transform.position, initialObject.transform.position);
            if (distance < closestDistance)
            {
                closestEnemy = obj;
                closestDistance = distance;
            }

        }
        return closestEnemy;
    }

}
