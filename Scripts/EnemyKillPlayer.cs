using UnityEngine;
using System.Collections;

public class EnemyKillPlayer : MonoBehaviour {
	
	public LevelManager levelManager;
	public PlayerController player;
	//public GameObject EnemyBeingControlled;

	// Use this for initialization
	void Start () 
	{
		levelManager = FindObjectOfType<LevelManager>();
		player = FindObjectOfType<PlayerController>();
		//EnemyBeingControlled = GameObject.FindGameObjectWithTag ("EnemyBeingControlled");

	}
	/*
	// Update is called once per frame
	void Update () 
	{
		//player = FindObjectOfType<PlayerController>();
		//levelManager = FindObjectOfType<LevelManager>();
	}
*/
	
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player" && PlayerController.alive == true)
			levelManager.RespawnPlayer();
			//PlayerController.alive = false;
	}
}