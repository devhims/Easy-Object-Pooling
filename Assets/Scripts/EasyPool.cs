using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hims.Arsenal
{
    public class EasyPool : MonoBehaviour
    {
        [SerializeField] GameObject[] prefabsToPool;
        [SerializeField] bool randomise = false;

        //Singleton 
        public static EasyPool Instance;

        Transform thisTransform;
        List<GameObject> pooledObjects = new List<GameObject>();

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }

            Instance = this;

            thisTransform = transform;
            AddObjectsToPool();
        }

        void AddObjectsToPool()
        {
            // Loop through all the prefabs selected by the user, instantiate and deactivate them
            foreach (var prefab in prefabsToPool)
            {
                GameObject go = Instantiate(prefab);
                go.SetActive(false);
                go.transform.parent = thisTransform;
                pooledObjects.Add(go);
            }
        }

        public GameObject GetObj()
        {
            // when randomise is checked in the inspector, it'll pick a random index,
            // check if the gameobject stored at that index is not null. If yes, return it and also remove it from the list
            if (randomise)
            {
                int randomIndex = Random.Range(0, pooledObjects.Count);

                if (pooledObjects[randomIndex] != null)
                {
                    GameObject go = pooledObjects[randomIndex];
                    pooledObjects.RemoveAt(randomIndex);
                    return go;
                }
            }

            // returns and remove the first inactive pooled object stored in the list
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeSelf)
                {
                    GameObject go = pooledObjects[i];
                    pooledObjects.RemoveAt(i);
                    return go;
                }
            }

            Debug.Log("Objects in the pool are over!");
            return null;
        }

        public void ReturnObj(GameObject obj)
        {
            // deactivate and reset the parent of the gameobject passed in as parameter
            if (obj.activeSelf)
                obj.SetActive(false);
            if (obj.transform.parent != thisTransform)
                obj.transform.parent = thisTransform;

            // adds the gameobject back to the pooled objects list
            pooledObjects.Add(obj);
        }
    }
}
