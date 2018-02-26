using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipPool : MonoBehaviour {

    public GameObject Prefab;

    private Stack<GameObject> inActiveobject = new Stack<GameObject>();

    public GameObject GetObject()
    {
        GameObject objectSpawned;

        if (inActiveobject.Count > 0)
        {
            objectSpawned = inActiveobject.Pop();
        }
        else
        {
            objectSpawned = Instantiate(Prefab);

            PooledObject pooledObject = objectSpawned.AddComponent<PooledObject>();
            pooledObject.myPool = this;
        }

        objectSpawned.transform.SetParent(null);
        objectSpawned.SetActive(true);

        return objectSpawned;
    }

    public void ReturnObject(GameObject toReturn)
    {
        PooledObject pooledObject = toReturn.GetComponent<PooledObject>();

        if (pooledObject != null && pooledObject.myPool == this)
        {
            toReturn.transform.SetParent(transform);
            toReturn.SetActive(false);

            inActiveobject.Push(toReturn);
        }
        else
        {
            Destroy(toReturn);
        }
    }
}

public class PooledObject : MonoBehaviour
{
    public ClipPool myPool;
}
