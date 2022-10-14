using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolerTestCommander : MonoBehaviour
{
    [SerializeField]
    List<GameObject> prefabs;

    List<GameObject> activeObjects = new List<GameObject>();
    private float inputCooldownDelay = 0.5f;
    private bool _inputEnabled = true;



    private void Update()
    {
        //Spawn a random prefab and then add it to the objectPooler (which will despawn it)
        if (Input.GetKey(KeyCode.A) && _inputEnabled)
        {
            _inputEnabled = false;
            ResetInputAfterCooldownDelay();

            if (activeObjects.Count < 1)
            {
                GameObject newObject = Instantiate(SelectRandomPrefab());
                ObjectPooler.PoolObject(newObject);
            }
            else
            {
                ObjectPooler.PoolObject(TakeRandomObjectFromList());
            }

        }

        //Spawn A random Prefab from the object pooler. Stats should be reset
        if (Input.GetKey(KeyCode.S) && _inputEnabled)
        {
            _inputEnabled = false;
            ResetInputAfterCooldownDelay();

            GameObject recycledObject = ObjectPooler.TakePooledGameObject(SelectRandomPrefab());
            recycledObject.transform.position = transform.position;
            recycledObject.transform.SetParent(transform);

            activeObjects.Add(recycledObject);

        }
    }

    private GameObject TakeRandomObjectFromList()
    {
        string randomObjectTag = (activeObjects[Random.Range(0, activeObjects.Count)]).tag;
        Debug.Log("Tag Selected: "+ randomObjectTag);

        foreach (GameObject selectedObject in activeObjects)
        {
            if (randomObjectTag == selectedObject.tag)
            {
                var foundObject = selectedObject;
                activeObjects.Remove(selectedObject);
                return foundObject;
            }
        }

        Debug.LogError("Error in poolTestCommander: GetRandomObjectFromList Failed due to not finding matching tag. Returned null instead.");
        return null;
    }

    private GameObject SelectRandomPrefab()
    {
        GameObject randomPrefab = prefabs[Random.Range(0, 2)];
        Debug.Log("Random Prefab Selection: "+ randomPrefab.name);
        return randomPrefab;
    }

    private void ResetInputAfterCooldownDelay()
    {
        Invoke("EnableCooldown", inputCooldownDelay);
    }

    private void EnableCooldown()
    {
        _inputEnabled = true;
    }
}
