using UnityEngine;
using System.Collections;

public class PlayerController2 : MonoBehaviour {
	
	public float moveSpeed;
	public float jumpHeight;
	public float wallJumpHeight;
	
	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask whatIsGround;
	
	public Transform firePoint;
	public GameObject shock;
	
	//private bool grounded;
	private bool doubleJumped;
	private Animator anim;
	private float moveVelocity;
	
	public bool attached;
	private PlayerController2 player2;
	
	public bool airDashed;
	public float invisibilityTime;
	
	
	//private bool enemyControlled = false;
	//private bool playerControlled = true;
	private GameObject enemyObject;
	//private bool stopMovingPassivePlayer;
	
	//Hit Wall check
	public Transform wallCheck;
	public float wallCheckRadius;
	public LayerMask whatIsWall;
	//private bool hittingWall;
	
	//Wall Jump
	private bool wallJumped;
	//private string lastMoveDirection;
	//private Vector2 wallJumpForce = new Vector2 (2f,2f);


	//Revert Player Controls/Detach from enemy
	private object playerObject;
	public Camera playerCamera;
	public Camera enemyCamera;
	public PlayerController player;


	
	// Use this for initialization
	void Start () {
		//Debug.Log ("Hey");
		
		anim = GetComponent<Animator> ();
		player2 = FindObjectOfType<PlayerController2>();
		player = FindObjectOfType<PlayerController>();
		enemyCamera = Transform.FindObjectOfType<Camera> ();
		//stopMovingPassivePlayer = false;

	}
	
	// Function called certain amount of times per second/frame - unsure
	void FixedUpdate()
	{



		//if (playerControlled) {
		
		//grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
		//hittingWall = Physics2D.OverlapCircle (wallCheck.position, wallCheckRadius, whatIsWall);
		//}
	}
	
	
	// Update is called once per frame
	void Update () 
	{
		//if(playerControlled)
		//{
		/*
		if (grounded) 
		{
			//airDashed = false;
			//doubleJumped = false;
			wallJumped = false;
		}
		
		
		
		//anim.SetBool ("Grounded", grounded);
		
		
		
		if (Input.GetKey (KeyCode.W) && grounded) 
			Jump ();
		
		//Wall Jump - Triple Jump
		if (Input.GetKey (KeyCode.W) && hittingWall && !wallJumped) {	
			wallJumped = true;
			wallJump ();
		}
		
			
		*/
		//Movement Left/Right
		moveVelocity = 0f;
		
		if (Input.GetKey (KeyCode.D)) {
			//lastMoveDirection = "right";
			moveVelocity = moveSpeed;
			transform.localScale = new Vector3 (1f, 1f, 1f);
			//if(!grounded && doubleJumped && !airDashed)
			//	AirDash(8);
			
		}
		
		if (Input.GetKey (KeyCode.A)) {
			//lastMoveDirection = "left";
			moveVelocity = -moveSpeed;
			transform.localScale = new Vector3 (-1f, 1f, 1f);
			//if(!grounded && doubleJumped && !airDashed)
			//	AirDash (-4);
		}
		Move ();
//		anim.SetFloat ("Speed", Mathf.Abs (GetComponent<Rigidbody2D> ().velocity.x));


		//if (!stopMovingPassivePlayer)
		{
			UpdatePassivePlayerBody ();
			//Update Player Body location constantly
			//StartCoroutine("UpdatePassivePlayerBody");
		}
		

		//Detach player body
		if (Input.GetKey (KeyCode.LeftShift)) {
			//stopMovingPassivePlayer = true;
			EnablePlayerControls ();
			DisableEnemyControls ();
			
			//player2.Jump ();
			
		}


		/*
		//Invisible
		if (Input.GetKey (KeyCode.LeftShift)) {
			StartCoroutine ("MakePlayerInvisible");
			//Debug.Log ("Player Tag " + player.tag);
		}*/

		
	} 

	public void UpdatePassivePlayerBody()
	{
		GameObject.FindGameObjectWithTag("Gus").GetComponent<CircleCollider2D>().enabled = false;
		GameObject.FindGameObjectWithTag("ControllingEnemy").transform.position = GameObject.FindGameObjectWithTag("AttachPosition").transform.position;
		GameObject.FindGameObjectWithTag("ControllingEnemy").transform.parent = GameObject.FindGameObjectWithTag("AttachPosition").transform ;
		GameObject.FindGameObjectWithTag("Gus").GetComponent<CircleCollider2D>().enabled = true;
	}

		//Enable Player Controller
		public void EnablePlayerControls()
		{
			player.transform.parent  = null;
			//player.transform.position += new Vector3(2f, 0f, 0f);
			StartCoroutine ("AttachDelay");
			player.tag = "Player";
			player.enabled = true;
			player.attaching = false;
			
			
			playerCamera = Camera.main;
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

		enemyObject.GetComponent<GusController> ().enabled = true;
		enemyObject.GetComponent<Rigidbody2D> ().isKinematic = true;
		//enemyObject.GetComponent<Collider2D> ().enabled = false;
		//Disables the camera on the Enemy
		enemyCamera.enabled = false;
		enemyObject.GetComponent<PlayerController2> ().enabled = false;
		

	}
	
	public void Jump()
	{
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, jumpHeight);
	}
	public void wallJump()
	{
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, wallJumpHeight);
	}
	
	public void Move()
	{
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveVelocity, GetComponent<Rigidbody2D> ().velocity.y);
	}

	public IEnumerator MakePlayerInvisible()
	{	
		player2.tag = "Invisible";
		Debug.Log ("Invisible");
		yield return new WaitForSeconds(invisibilityTime);
		Debug.Log ("No Longer Invisible");
		player2.tag = "Player";
	}

}