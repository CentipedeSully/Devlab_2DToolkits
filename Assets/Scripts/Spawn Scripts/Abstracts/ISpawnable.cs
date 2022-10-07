using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnable
{
    GameObject Spawn(SpawnPosition position);
    void Despawn();
    float GetSpawnChance();
}
