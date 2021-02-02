//// Imports
using UnityEngine;
using System.Collections;

//// Class
public class ItemSpawner : MonoBehaviour
{
    //// Configurables
    public int fMaxCollidables = 30;
    public float fRadiusCorrection = 0.6f;
    public GameObject fCollidable1Prefab;
    public GameObject fCollidable2Prefab;
    public GameObject fCollidable3Prefab;
    public int fInitialSpawnCount = 10;
    public int fSpawnCycle = 30;

    //// Instance Variables
    private Mesh fTerrainMesh;
    private Transform fTerrainTransform;
    private Vector3 fTerrainBounds;
    private int fSpawnCycleCounter = 0;

    //// Events
    void Start()
    {
        // load components
        fTerrainMesh = (GameObject.FindWithTag("Terrain").GetComponent<MeshFilter>()).mesh;
        fTerrainTransform = GameObject.FindWithTag("Terrain").transform;
        // determine primatives
        fTerrainBounds =  Vector3.Scale(fTerrainMesh.bounds.size, fTerrainTransform.localScale);
        // spawn collectibles
        spawnCollidables(fInitialSpawnCount);
    }

    void FixedUpdate ()
    {
        // increment counter
        fSpawnCycleCounter++;
        // check counter
        if (fSpawnCycleCounter >= fSpawnCycle)
        {
            // spawn
            spawnCollidables(1);
            // reset
            fSpawnCycleCounter = 0;
        }
    }

    //// Methods
    void spawnCollidables(int iSpawnCount)
    {
        // check limit
        if (GameObject.FindGameObjectsWithTag("Collidable").Length == fMaxCollidables)
        {
            return;
        }
        // initialize
        Vector3 spawnPosition;
        float dRadius = Mathf.Pow(Mathf.Pow(fTerrainBounds.x * 0.5f, 2) + Mathf.Pow(fTerrainBounds.z * 0.5f, 2), 0.5f) * fRadiusCorrection;
        // spawn collectibles
        for (int i = 0; i < iSpawnCount; ++i)
        {
            // determine spawn position
            spawnPosition = new Vector3(0,0,0);
            float dXPos = Random.Range(dRadius * -1f, dRadius);
            float dZPos = Random.Range(dRadius * -1f, dRadius);
            // check if valid
            while (Mathf.Pow(Mathf.Pow(dXPos, 2) + Mathf.Pow(dZPos, 2), 0.5f) > dRadius)
            {
                dXPos = Random.Range(dRadius * -1f, dRadius);
                dZPos = Random.Range(dRadius * -1f, dRadius);
            }
            // make into position vector
            spawnPosition += new Vector3 (dXPos, Random.Range(4,7), dZPos);
            // spawn collectible
            switch (Random.Range(0, 3))
            {
                case 0:
                    Instantiate(fCollidable1Prefab, spawnPosition, Quaternion.Euler(new Vector3(0, Random.rotation.eulerAngles.y, 0)));
                    break;
                case 1:
                    Instantiate(fCollidable2Prefab, spawnPosition, Quaternion.Euler(new Vector3(0, Random.rotation.eulerAngles.y, 0)));
                    break;
                case 2:
                    Instantiate(fCollidable3Prefab, spawnPosition, Quaternion.Euler(new Vector3(0, Random.rotation.eulerAngles.y, 0)));
                    break;
            }
        }
    }
}
