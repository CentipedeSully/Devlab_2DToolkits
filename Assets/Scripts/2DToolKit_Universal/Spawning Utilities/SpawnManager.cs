using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SullysUniversalToolkit.SpawnUtilities
{
    public class SpawnManager : MonoSingleton<SpawnManager>
    {
        /// Responsiblity: This class is a controller. It spawns objects on command.
        /// Functions:
        /// - Holds a list of spawnPositionManagers. 
        /// - Sends a spawn signal to a specified SPManager, along with an optional ISpawnable Object
        /// -- If no ISpawnable object is passed, the SPManager will determine what is spawned
        /// -- the SPManager determines where spawning occurs
        /// 

        private List<SpawnPositionManager> spawnPositionManagers;
        private SpawnPositionManager currentSpawnManager;


        public void SpawnObject()
        {
            
        }

    }



}
