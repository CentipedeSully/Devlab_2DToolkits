using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoSingleton<ObjectPooler>
{
    private static List<GameObject> _pooledObjects;

    private static GameObject objectPoolerGameObject;

    public static void CleanAndPoolObject(GameObject poolableObject)
    {
        poolableObject.GetComponent<IPoolable>();
        poolableObject.GetComponent<IPoolable>();
        poolableObject.GetComponent<IPoolable>();
        _pooledObjects.Add(poolableObject);
    }


}
