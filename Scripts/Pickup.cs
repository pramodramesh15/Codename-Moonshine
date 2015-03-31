using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

	public int pointsToAdd;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.GetComponent<PlayerController> () == null)
		{
			return;
		}

		ScoreManager.AddPoints (pointsToAdd);
		Destroy (gameObject);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
