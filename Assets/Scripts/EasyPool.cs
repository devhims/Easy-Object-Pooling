using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EasyPool : MonoBehaviour
{
    [SerializeField] GameObject[] prefabsToPool;
    [SerializeField] int poolSize = 25;
    [SerializeField] bool randomise = false;

    Transform thisTransform;
    int currentIndex;
    int prefabIndex;

    // array to store pooled objects
    GameObject[] pooledObjects;

    // singleton 
    public static EasyPool Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;

        thisTransform = transform;
        pooledObjects = new GameObject[poolSize];
        AddObjectsToPool();
    }

    // instantiate and add objects to pool
    void AddObjectsToPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            prefabIndex %= prefabsToPool.Length;
            GameObject go = Instantiate(prefabsToPool[prefabIndex]);
            go.SetActive(false);
            go.transform.parent = thisTransform;
            pooledObjects[i] = go;
            prefabIndex++;
        }
    }

    public GameObject GetObj()
    {
        // when randomise is checked in the inspector, it'll pick a random index,
        // check if the gameobject stored at that index is not null. If yes, return it.
        if (randomise)
        {
            int randomIndex = Random.Range(0, pooledObjects.Length);

            if (!pooledObjects[randomIndex].activeSelf)
            {
                GameObject go = pooledObjects[randomIndex];
                return go;
            }
        }

        // returns the first inactive pooled object and moves the index down by 1.
        for (int i = currentIndex; i < pooledObjects.Length; i++)
        {
            if (!pooledObjects[i].activeSelf)
            {
                GameObject go = pooledObjects[i];
                ShiftIndexDown();
                return go;
            }
        }

        Debug.LogWarning("Pool is empty");
        return null;
    }

    public void ReturnObj(GameObject obj)
    {
        // deactivate and reset the parent of the gameobject passed in as parameter
        if (obj.activeSelf)
            obj.SetActive(false);
        if (obj.transform.parent != thisTransform)
            obj.transform.parent = thisTransform;
    }

    // moves the current currentIndex in the pool down by 1
    void ShiftIndexDown()
    {
        currentIndex = currentIndex < pooledObjects.Length - 1 ? currentIndex + 1 : 0;
    }
}
