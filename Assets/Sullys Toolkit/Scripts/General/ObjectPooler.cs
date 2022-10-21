using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SullysToolkit
{
    public class ObjectPooler : MonoSingleton<ObjectPooler>
    {
        private static int _defaultPopulationValue = 10;

        private static List<GameObject> _pooledObjects;

        private static GameObject _objectPoolerGameObject;


        protected override void InitializeAdditionalFields()
        {
            _objectPoolerGameObject = gameObject;
            _pooledObjects = new List<GameObject>();
        }


        public static void PoolObject(GameObject existingObject)
        {
            if (existingObject.activeSelf == true)
                existingObject.SetActive(false);

            ParentPoolerToGameObject(existingObject);

            _pooledObjects.Add(existingObject);
        }

        private static void ParentPoolerToGameObject(GameObject pooledObject)
        {
            ParentObjectToNewTransform(pooledObject, _objectPoolerGameObject.transform);

            pooledObject.transform.position = _objectPoolerGameObject.transform.position;
        }


        public static void ParentObjectToNewTransform(GameObject childObject, Transform parentTransform)
        {
            childObject.transform.SetParent(parentTransform);
        }

        public static GameObject TakePooledGameObject(GameObject requestedPrefab)
        {
            if (DoesObjectExistInPool(requestedPrefab) == false)
                AddPopulationToPool(requestedPrefab, _defaultPopulationValue);

            GameObject recycledGameObject = null;
            foreach (GameObject pooledObject in _pooledObjects)
            {
                if (requestedPrefab.tag == pooledObject.tag)
                {
                    recycledGameObject = pooledObject;
                    _pooledObjects.Remove(pooledObject);
                    break;
                }
            }

            if (recycledGameObject != null)
            {
                recycledGameObject.SetActive(true);
                return recycledGameObject;
            }

            else
            {
                Debug.LogError("Failed to return requested object from ObjectPooler: (" + requestedPrefab.name + "). Failed To Populate Pooler with requested object prfab.");
                return null;
            }

        }

        public static bool DoesObjectExistInPool(GameObject objectInQuestion)
        {
            foreach (GameObject pooledObject in _pooledObjects)
            {
                if (objectInQuestion.tag == pooledObject.tag)
                    return true;
            }

            return false;
        }

        private static void AddPopulationToPool(GameObject prefab, int amountToAdd)
        {
            int count = 0;
            while (count < amountToAdd)
            {
                GameObject newObject = Instantiate(prefab, _objectPoolerGameObject.transform);
                newObject.SetActive(false);
                PoolObject(newObject);
                count++;
            }
        }


    }

}
