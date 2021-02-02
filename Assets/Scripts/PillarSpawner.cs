//// Imports
using UnityEngine;
using System.Collections;

//// Class
public class PillarSpawner : MonoBehaviour
{
	//// Configurables
    public int fNumberOfPillars = 60;
	public float fRadiusCorrection = 0.7f;
	public GameObject fWallPillarPrefab;

	//// Instance Variables
	private Mesh fTerrainMesh;
	private Transform fTerrainTransform;
	private Vector3 fTerrainBounds;

	//// Events
	void Start()
	{
		// load components
		fTerrainMesh = (GameObject.FindWithTag("Terrain").GetComponent<MeshFilter>()).mesh;
		fTerrainTransform = GameObject.FindWithTag("Terrain").transform;
		// determine primatives
		fTerrainBounds =  Vector3.Scale(fTerrainMesh.bounds.size, fTerrainTransform.localScale);
		// spawn pillars
		spawnPillars();
	}

	void Update ()
	{
		// pass
	}

	//// Methods
	void spawnPillars()
	{
		// initialize
		Vector3 spawnPosition;
		GameObject gPillar;
		// calculate map radius
		float dMapRadius =  Mathf.Pow(Mathf.Pow(fTerrainBounds.x * 0.5f,2) + Mathf.Pow(fTerrainBounds.z * 0.5f,2),0.5f) * fRadiusCorrection;
		// spawn pillars
		for (int i = 0; i < fNumberOfPillars; i++)
		{
			// determine spawn position
			spawnPosition = transform.position;
			spawnPosition += new Vector3 (Mathf.Sin ((Mathf.PI * 2 / fNumberOfPillars) * i), 0f, Mathf.Cos ((Mathf.PI * 2 / fNumberOfPillars) * i)) * dMapRadius;
			// spawn pillar
			gPillar = (GameObject) Instantiate(fWallPillarPrefab, spawnPosition, transform.rotation);
            gPillar.transform.position = new Vector3(gPillar.transform.position.x, gPillar.GetComponent<MeshFilter>().mesh.bounds.size.y * 0.25f, gPillar.transform.position.z);
			// allign pillar
			gPillar.transform.rotation = Quaternion.LookRotation(gPillar.transform.position - fTerrainTransform.position, fTerrainTransform.up);
		}
	}
}
