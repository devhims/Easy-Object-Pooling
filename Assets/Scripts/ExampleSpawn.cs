using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleSpawn : MonoBehaviour
{
    WaitForSeconds delay0 = new WaitForSeconds(1.0f), delay1 = new WaitForSeconds(2.0f);
    Transform thisTransform;

    void Start()
    {
        thisTransform = transform;
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        // the while loop with "true" condition ensures the code is executed continously without break
        while (true)
        {
            // add a small delay (optional)
            yield return delay0;

            // get gameobject from the pool
            GameObject obj = EasyPool.Instance.GetObj();

            if (obj != null) 
            {
                obj.transform.SetPositionAndRotation(thisTransform.position, Quaternion.identity);
                obj.transform.parent = thisTransform;
                obj.SetActive(true);

                // another delay (optional)
                yield return delay1;

                // return gameobject back the pool
                EasyPool.Instance.ReturnObj(obj);
            }
        }
    }
}
