using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour {
    [SerializeField]
    List<GameObject> levelChunks;
    private float mostRecentSpawnEnd;
    private Camera myCamera;
    [SerializeField]
    private List<GameObject> spawnedLevels;
	// Use this for initialization
	void Start () {
        myCamera = GetComponent<Camera>();

        mostRecentSpawnEnd = 0f;
        if (levelChunks.Count > 0)
        {
            Vector3 newPosition = new Vector3(myCamera.transform.position.x, myCamera.transform.position.y, 0f);
            mostRecentSpawnEnd = myCamera.transform.position.x;
            GameObject newLevel = Instantiate(levelChunks[Random.Range(0, levelChunks.Count - 1)], newPosition, Quaternion.identity);
            spawnedLevels.Add(newLevel);
        }
    }
	
	// Update is called once per frame
	void Update () {
        float width = 18f;// (myCamera.aspect * myCamera.orthographicSize) * 2;
        if (levelChunks.Count > 0)
        {
            float position = myCamera.transform.position.x;
            if (mostRecentSpawnEnd < position)
            {                
                Vector3 newPosition = new Vector3(mostRecentSpawnEnd + width, myCamera.transform.position.y, 0f);
                mostRecentSpawnEnd = newPosition.x;
                GameObject newLevel = Instantiate(levelChunks[Random.Range(0, levelChunks.Count - 1)], newPosition, Quaternion.identity);
                spawnedLevels.Add(newLevel);
            }
        }
        if (spawnedLevels.Count > 1)
        {
            //Only going to need to remove the oldest one, so no point in checking the rest
            if (spawnedLevels[0].transform.position.x < (myCamera.transform.position.x - width))
            {
                Destroy(spawnedLevels[0]);
                spawnedLevels.RemoveAt(0);
            }
        }
    }
}
