using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    /// <summary>
    /// actually this should be called logging manager
    /// </summary>
    
    public static TreeManager instance = null;

    public float treeSpawnTime = 10;

    [SerializeField]
    GameObject treeSpawner; // it's a prefab!
    [SerializeField]
    GameObject logPrefab;

    public bool playerHasAxe = false;
    
    // system messages /////////////////////////////////////////////////
    void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        }
        else {
            instance = this;
        }
    }


    // tree instance message ///////////////////////////////////////////
    // when a tree gets chopped, this method will be called
    public void TreeGotChopped(GameObject poorTree) {
        Debug.Log("treeGotChopped() called by");
        Debug.Log(poorTree.transform.position);
        Instantiate(treeSpawner, poorTree.transform.position + new Vector3(0.2f, 0, 0.2f), Quaternion.identity);
        Instantiate(logPrefab, poorTree.transform.position, Quaternion.identity);
        Destroy(poorTree);
    }
}
