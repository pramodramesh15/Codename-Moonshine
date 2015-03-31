using UnityEngine;
using System.Collections;

public class ToxicDripper : MonoBehaviour {

	public float spawnDelay;
	public float dripInterval;
	public bool waitToSpawn;
	
	//Player Shoot Projectiles
	public Transform dripPoint;
	public GameObject toxicDrip;
	
	
	// Use this for initialization
	void Start () 
	{
		waitToSpawn = true;
		StartCoroutine("SpawnDelay");
		//SpawnDelay ();

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!waitToSpawn)
		{
			Debug.Log ("Spawning Drip");
			waitToSpawn = true;

			StartCoroutine("SpawnDrips");
			//SpawnDrips ();


		}                                 
	}
	
	
	public IEnumerator SpawnDrips()
	{	
		Instantiate (toxicDrip, dripPoint.position, dripPoint.rotation); 
		yield return new WaitForSeconds (dripInterval);
		waitToSpawn = false;
	}
	
	
	public IEnumerator SpawnDelay()
	{	
		yield return new WaitForSeconds (spawnDelay);
		waitToSpawn = false;
	}
	/*
	void OnTriggerEnter2D(Collider2D other)
	{
		Destroy (gameObject);
	}
	*/
	
}
