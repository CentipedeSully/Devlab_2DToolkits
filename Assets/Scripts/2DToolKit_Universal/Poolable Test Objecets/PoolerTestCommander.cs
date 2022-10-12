using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolerTestCommander : MonoBehaviour
{
    [SerializeField]
    List<GameObject> prefabs;



    private void Update()
    {
        //Spawn a random prefab and then add it to the objectPooler (which will despawn it)
        if (Input.GetKey(KeyCode.A))
        {
            GameObject newObject = Instantiate(ReturnRandomPrefab());


            ObjectPooler.PoolObject(newObject);
        }

        //Spawn A random Prefab fromt he object pooler
        if (Input.GetKey(KeyCode.S))
        {
            ObjectPooler.GetPooledGameObject(ReturnRandomPrefab());
        }
    }

    private GameObject ReturnRandomPrefab()
    {
        return prefabs[Random.Range(0, 1)];
    }
}
