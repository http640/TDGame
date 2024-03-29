﻿using UnityEngine;

public class Singletons<T> :  MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<T> ();
            else if (instance != FindObjectOfType<T>())
                Destroy(FindObjectOfType<T>());

            //Do not destroy the target Object when loading a new Scene.
            DontDestroyOnLoad(FindObjectOfType<T>());

            return instance;    
        }
    }

}
