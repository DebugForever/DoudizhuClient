using System;
using System.Collections.Generic;
using UnityEngine;

public class MyTools
{
    public static void Shuffle<T>(T[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            //swap
            T temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
    }

    public static void DestroyAllChild(Transform transform)
    {
        foreach (Transform t in transform)
        {
            GameObject.Destroy(t.gameObject);
        }
    }
}

