using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Restricts the instantiation of a class to one "single" instance
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;

    public static T Instance
    {

        get
        {

            if (instance == null) {
                instance = FindObjectOfType<T>();
                }
            
            return instance;
        }

    }

}
