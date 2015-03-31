using UnityEngine;
using System.Collections;

public class DestroyFinishedParticle : MonoBehaviour {

	private ParticleSystem thisParticleSystem;


	// Use this for initialization
	void Start () 
	{
		thisParticleSystem = GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (thisParticleSystem.isPlaying)
			return;
		else
			Destroy (gameObject);
	
	}

	//When the particle effect is offscreen, ensure it's destroyed
	void onBecomeInvisible()
	{
		Destroy (gameObject);
	}

}
