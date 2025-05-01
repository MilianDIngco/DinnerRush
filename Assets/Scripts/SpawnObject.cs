using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject ItemPrefab;

    public void Spawn()
    {
        GameObject item = Instantiate(ItemPrefab);
        item.transform.parent = transform;
        item.transform.position += new Vector3(0, 0.5f, 0);
    }

}
