using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour {
    [SerializeField]
    private List<GameObject> enemyPrefabs;
    [SerializeField]
    private List<GameObject> pickupPrefabs;
    [SerializeField]
    private List<Transform> pickupLocations;
    [SerializeField]
    private List<Transform> enemyLocations;
    // Use this for initialization
    void Start () {
        float enemiesToSpawn = Random.Range(1, enemyLocations.Count+1);
        float pickupsToSpawn = Random.Range(1, pickupLocations.Count+1);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int enemyToSpawn = Random.Range(0, enemyPrefabs.Count);
            int locationToSpawn = Random.Range(0, enemyLocations.Count);
            Instantiate(enemyPrefabs[enemyToSpawn], enemyLocations[locationToSpawn].transform.position, Quaternion.identity);
            enemyLocations.RemoveAt(locationToSpawn);
        }

        for (int i = 0; i < pickupsToSpawn; i++)
        {
            int pickupToSpawn = Random.Range(0, pickupPrefabs.Count);
            int locationToSpawn = Random.Range(0, pickupLocations.Count);
            Instantiate(pickupPrefabs[pickupToSpawn], pickupLocations[locationToSpawn].transform.position, Quaternion.identity);
            pickupLocations.RemoveAt(locationToSpawn);
        }
    }
}
