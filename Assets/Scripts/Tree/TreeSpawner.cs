using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    private float timer = 0;

    [SerializeField]
    GameObject treePrefab;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= TreeManager.instance.treeSpawnTime) {
            Instantiate(treePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
