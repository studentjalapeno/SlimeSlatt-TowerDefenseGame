using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objectPrefabs;


    private List<GameObject> pooledObjects = new List<GameObject>();


    /// <summary>
    /// Gets an object from the pool
    /// </summary>
    /// <param name="type">The type of object that we request</param>
    /// <returns>A GameObject of specific type</returns>
    public GameObject GetObject(string type)
    {
        
        
        //This code only runs if the object pool has objects in it
        foreach ( GameObject gameObject in pooledObjects)
        {
            if (gameObject.name == type && !gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
                return gameObject;

            }

        }




        //If the pool didnt contain the object, that we needed then we need to create a new object
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            //if we have a prefab for creating the object
            if (objectPrefabs[i].name == type)
            {
                //Instantiate the prefab of the correct type
                GameObject newObject = Instantiate(objectPrefabs[i]);
                pooledObjects.Add(newObject);
                newObject.name = type;
                return newObject;

            }


        }


        return null;
    }

    public void ReleaseObject(GameObject gameObject)
    {
        gameObject.SetActive(false);



    }

}
