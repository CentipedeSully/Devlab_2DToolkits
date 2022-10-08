using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableObject : MonoBehaviour
{
    [Header("Object Spawn Utilities")]
    [Tooltip("Keep this value to the nearest Hundreth (0.xx). Also beware that the sum of all spawnable objects' spawn chances must equal 1 ")]
    [Range(.01f, 1f)] [SerializeField] float _spawnChance = .15f;
    [SerializeField] bool _showDebug = false;



    public GameObject Spawn(SpawnPosition spawnPosition)
    {
        if (spawnPosition != null)
            return Instantiate(gameObject, spawnPosition.transform.position, Quaternion.identity);
        else
        {
            if (_showDebug)
                Debug.Log("No Position is available. Spawn Request Ignored");
            return null;
        }
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }

    public float GetSpawnChance()
    {
        return Mathf.Floor(_spawnChance * 100) / 100;
    }
}
