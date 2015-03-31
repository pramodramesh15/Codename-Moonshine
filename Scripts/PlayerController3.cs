using UnityEngine;
using System.Collections;

public class PlayerController3 : MonoBehaviour {
	
	public float moveSpeed;
	public bool moveRight;
	
	public Rigidbody2D rbd;
	
	//Hit Wall check
	public Transform wallCheck;
	public float wallCheckRadius;
	public LayerMask whatIsWall;
	private bool hittingWall;
	
	//Hit Player Check
	public Transform rangeCheck;
	public float rangeCheckRadius;
	public LayerMask whatIsPlayer;
	public static bool hittingPlayer;
	
	//private bool attacking;
	private Animator anim;
	
	//Enemy Size
	public float enemyScaleX = 1;
	public float enemyScaleY = 1;
	public float enemyScaleZ = 1;
	public bool fixedScaling;
	
	//Hit Player Check
	public bool attached;
	private PlayerController player;
	public Transform playerCheck;
	public float playerCheckRadius;
	
	public float gravityStore;
	private PlayerController playerEnemy;

	
	//Revert Player Controls/Detach from enemy
	private object playerObject;
	public Camera playerCamera;
	public Camera enemyCamera;
	public PlayerController3 player3;
	private GameObject enemyObject;
	
	
	// Use this for initialization
	void Start ()
	{
		moveRight = Gus2Controller.moveRight;
		//rbd = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		//player = FindObjectOfType<PlayerController>();
		if (fixedScaling)
			enemyScaleY = enemyScaleZ = enemyScaleX;
		
		gameObject.transform.localScale = new Vector3(enemyScaleX,enemyScaleY,enemyScaleZ);
		player3 = FindObjectOfType<PlayerController3>();
		player = FindObjectOfType<PlayerController>();
		enemyCamera = Transform.FindObjectOfType<Camera> ();
		
	}
	
	// Function called certain amount of times per second/frame - unsure
	void FixedUpdate()
	{
		hittingWall = Physics2D.OverlapCircle (wallCheck.position, wallCheckRadius, whatIsWall);
		hittingPlayer = Physics2D.OverlapCircle (playerCheck.position, playerCheckRadius, whatIsPlayer);
	}
	


	void Update () 
	{ 
		
		if (hittingWall)
			moveRight = !moveRight;
		
		if (moveRight) {
			//transform.localScale = new Vector3 (1f, 1f, 1f);
			transform.localScale = new Vector3 (enemyScaleX,enemyScaleY,enemyScaleZ);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		} else {
			//transform.localScale = new Vector3 (-1f, 1f, 1f);
			transform.localScale = new Vector3 (-enemyScaleX,enemyScaleY,enemyScaleZ);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (-moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		}
		UpdatePassivePlayerBody ();
		//Detach player body
		if (Input.GetKey (KeyCode.LeftShift)) {
			//stopMovingPassivePlayer = true;
			EnablePlayerControls ();
			DisableEnemyControls ();
			
			//player2.Jump ();
			
		}


		/*
		if (PlayerController.justDetached == true)
			GetComponent<CircleCollider2D>().enabled = false;
		else {
			GetComponent<CircleCollider2D>().enabled = true;
		}
		*/
	}


	
	//Enable Player Controller
	public void EnablePlayerControls()
	{
		player.transform.SetParent (null);
		//player.transform.position += new Vector3(2f, 0f, 0f);
		StartCoroutine ("AttachDelay");
		player.tag = "Player";
		player.enabled = true;
		player.attaching = false;
		
		
		playerCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
		playerCamera.enabled = true;
		player.playerControlled = true; 
		
		player.GetComponent<Rigidbody2D> ().gravityScale = player.gravityStore;
		player.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		player.GetComponent<Collider2D> ().enabled = true;
		player.GetComponent<Rigidbody2D> ().WakeUp ();
		Debug.Log ("Original Player Enabled");
		
		
	}
	
	public IEnumerator AttachDelay()
	{
		PlayerController.justDetached = true;
		yield return new WaitForSeconds (0.5f);
		PlayerController.justDetached = false;
	}
	
	
	//Disable Enemy Controller
	public void DisableEnemyControls()
	{
		
		enemyObject = gameObject;
		
		enemyObject.GetComponent<Gus2Controller> ().enabled = true;
		enemyObject.GetComponent<Rigidbody2D> ().isKinematic = true;
		//Disables the camera on the Enemy
		enemyCamera.enabled = false;
		player3.enabled = false;
		
		
	}

		public void UpdatePassivePlayerBody()
	{
		GameObject.FindGameObjectWithTag("Gus2").GetComponent<CircleCollider2D>().enabled = false;
		GameObject.FindGameObjectWithTag("ControllingEnemy").transform.position = GameObject.FindGameObjectWithTag("AttachPosition").transform.position;
		GameObject.FindGameObjectWithTag("ControllingEnemy").transform.SetParent(GameObject.FindGameObjectWithTag("AttachPosition").transform);
		GameObject.FindGameObjectWithTag("Gus2").GetComponent<CircleCollider2D>().enabled = true;
	}

}
