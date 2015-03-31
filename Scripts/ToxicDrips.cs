using UnityEngine;
using System.Collections;

public class ToxicDrips : MonoBehaviour {
	
	public float speed;
	
	// Use this for initialization
	void Start () 
	{		
	}
	
	// Update is called once per frame
	void Update () 
	{
		GetComponent<Rigidbody2D> ().velocity = new Vector2 ( 0, -speed);                                          
	}
	

	void OnTriggerEnter2D(Collider2D other)
	{
		Destroy (gameObject);
	}

	
	
}
