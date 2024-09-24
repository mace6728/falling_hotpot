using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class floorManager : MonoBehaviour
{
    [SerializeField] GameObject[] floorPrefabs;
    public void spawnFloor() {
        int r = Random.Range(0, floorPrefabs.Length);
        GameObject floor = Instantiate(floorPrefabs[r], transform);
        floor.transform.position = new Vector3(Random.Range(-7f, 7f), -11f, 0f);
    } 
}
