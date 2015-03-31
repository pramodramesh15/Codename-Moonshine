using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {


	public GameObject currentCheckpoint;
	public GameObject deathParticle;
	public GameObject respawnParticle;
	public float respawnDelay;
	public int pointsRemovedOnDeath;

	//private float gravityStore;
	private PlayerController player;
	private Animator anim;

	// Use this for initialization
	void Start () {
	
		player = FindObjectOfType<PlayerController>();
		anim = player.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RespawnPlayer()
	{
		StartCoroutine ("RespawnPlayerCo");
	}

	public IEnumerator RespawnPlayerCo()
	{
		PlayerController.alive = false;
		Instantiate (deathParticle, player.transform.position, player.transform.rotation);
		player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		player.enabled = false;
		player.GetComponent<Renderer>().enabled = false;
		//gravityStore = player.GetComponent<Rigidbody2D> ().gravityScale;
		player.GetComponent<Rigidbody2D> ().gravityScale = 0;
		pointsRemovedOnDeath = ScoreManager.score;
		ScoreManager.AddPoints (-pointsRemovedOnDeath);
		//anim.SetBool ("Grounded", true);

		yield return new WaitForSeconds(respawnDelay);
		Debug.Log ("Player Respawned");
		//anim.SetBool ("Grounded", true);
		//Debug.Log ("gvstore"+gravityStore);
		player.GetComponent<Rigidbody2D> ().gravityScale = PlayerController.originalGravity;
		Debug.Log (player.GetComponent<Rigidbody2D> ().gravityScale);
		player.transform.position = currentCheckpoint.transform.position;
		PlayerController.grapplingInAir = false;
		player.GetComponent<Renderer>().enabled = true;
		player.enabled = true;
		Instantiate (respawnParticle, currentCheckpoint.transform.position, currentCheckpoint.transform.rotation);
		PlayerController.alive = true;
		 
	}
	

}
