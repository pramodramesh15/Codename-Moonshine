using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	//Movement variables
	public float moveSpeed;
	public float jumpHeight;
	public float wallJumpHeight;
	public float chainWallJumpHeight;
	private bool grounded;
	private bool doubleJumped;
	private Animator anim;
	private float moveVelocity;
	public static bool alive;
	public static float originalGravity;

	//Player Movement Controller 
	public bool attached;
	private PlayerController player;
	public bool airDashed;
	public float invisibilityTime;

	//Hit Wall check
	public Transform wallCheck;
	public float wallCheckRadius;
	public LayerMask whatIsWall;
	private bool hittingWall;

	//Wall Jump
	private bool wallJumped;

	private float jumpDir;
	private float prevJumpDir;

	//Groung Collision Checks
	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask whatIsGround;

	//Player Shoot Projectiles
	public Transform firePoint;
	public GameObject shock;

	// Controlling Enemy Object Variables
	public bool playerControlled;
	private GameObject enemyObject;
	public Camera playerCamera;
	private string enemyToAttachTo;
	public float gravityStore ;
	public bool attaching;
	public bool making;

	//Grapple Height
	private static int i = 0;
	public float grappleHeight;
	public static bool grappling;
	public static bool grapplingInAir;
	public static bool toggled;
	public RaycastHit2D ceiling; 
	public Transform ceilingCheck;
	private bool hittingCeiling;
	public float positiveGravity;
	public float negativeGravity;
	public float grappleCameraY;
	public float groundCameraY;
	private float cameraSmoothing;
	private float originalPosition;
	private bool cameraAdjusting ;
	private bool cameraAdjusted ;
	private float loopTime;
	private bool dontAdjustCamera;


	public static bool justDetached;


	
	// Use this for initialization
	void Start () 
	{

		cameraSmoothing = 0.15f;
		alive = true;
		playerControlled = true;
		anim = GetComponent<Animator> ();
		player = FindObjectOfType<PlayerController>();
		playerCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
		gravityStore = GetComponent<Rigidbody2D> ().gravityScale;
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
		groundCameraY = playerCamera.transform.position.y;

		originalGravity = player.GetComponent<Rigidbody2D> ().gravityScale;
		//Debug.Log(""+player.transform.parent);
	}
	
	// Function called certain amount of times per second/frame - unsure
	void FixedUpdate()
	{
		//Debug.Log ("UpdatingPlayer");
		if (playerControlled) {
			grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
			hittingWall = Physics2D.OverlapCircle (wallCheck.position, wallCheckRadius, whatIsWall);
			hittingCeiling = Physics2D.OverlapCircle (ceilingCheck.position, groundCheckRadius, whatIsGround);
		}
	}
	
	
	// UPDATE CALL ONCE PER FRAME
	void Update () 
	{
		//if (!making)
		//StartCoroutine ("ChildMaker");

		if (!playerControlled) 
		{
			Debug.Log ("Player script should not run!");
		}

		if (playerControlled) 
		{
			Debug.Log ("" + "g "+grounded +" wj "+wallJumped+" gpia "+grapplingInAir+" gvtySc "+ GetComponent<Rigidbody2D>().gravityScale);

			if (grounded)
			{
				jumpDir = prevJumpDir = 0f;
				//airDashed = false;
				//doubleJumped = false;
				wallJumped = false;
				//grapplingInAir = false;
			}
		
		
			// Set animation to Idle
			anim.SetBool ("Grounded", grounded);

		
			// GROUND JUMP
			if (Input.GetKey (KeyCode.W) && grounded) 
				Jump ();
		

			//WALL JUMP
			if (Input.GetKey (KeyCode.W) && hittingWall)
			{	
				jumpDir = transform.localScale.x;
				if(!wallJumped || (wallJumped && (jumpDir == -prevJumpDir)))
				{
					wallJumped = true;
					wallJump (wallJumped);
					prevJumpDir = jumpDir;
				}
			}
		
	/*
			//Shooting a Projectile
			if (Input.GetKeyDown (KeyCode.LeftControl))
				Instantiate (shock, firePoint.position, firePoint.rotation); 
		
	*/	
			//DETERMINE MOVEMENT LEFT / RIGHT

			moveVelocity = 0f;
		
			if (Input.GetKey (KeyCode.D)&& !grapplingInAir) 
			{
				moveVelocity = moveSpeed;
				transform.localScale = new Vector3 (1f, 1f, 1f);
			}
		
			if (Input.GetKey (KeyCode.A) && !grapplingInAir) 
			{
				Debug.Log ("Moving");
				moveVelocity = -moveSpeed;
				transform.localScale = new Vector3 (-1f, 1f, 1f);
			}

			if(grapplingInAir && !grounded)
			{
				Debug.Log ("Grappling so no moving");
				moveVelocity = 0f;
				if(hittingCeiling || grounded )
					grapplingInAir = false;
			}


			// Move function call
			Move ();

			// Set animation to Walk
			anim.SetFloat ("Speed", Mathf.Abs (GetComponent<Rigidbody2D> ().velocity.x));
		
	/*
			//Make Player Invisible to enemies for certain duration
			if (Input.GetKey (KeyCode.LeftShift)) 
			{
				StartCoroutine ("MakePlayerInvisible");
			}
	*/
			/*
			enemyObject = GameObject.FindGameObjectWithTag ("Gus");
			if(player.GetComponent<CircleCollider2D>().IsTouching(enemyObject.GetComponent<CircleCollider2D>()))
				Debug.Log ("Colliders Touch");
			*/


			//Grapple Ceiling - Very primitive - ensure no heights 
			// If key pressed, send ray cast, if ceiling at a certain height, store gravity scale and change player's gravity scale to -ve of stored value.
			// Ensure ceiling check - if no ceiling || if pressed space again, reverse gravity
			if(Input.GetKeyDown(KeyCode.Space))
			{
				ceiling = Physics2D.Raycast(player.transform.position,Vector2.up,grappleHeight,whatIsGround);
				if(ceiling.collider != null)
				{
					grapplingInAir = true;
					toggled = !toggled;
					Debug.Log ("Hit " + ++i);
					if(GetComponent<Rigidbody2D>().gravityScale >0)
					{
						groundCameraY = playerCamera.transform.position.y;
						GetComponent<Rigidbody2D>().gravityScale = negativeGravity;
						StartCoroutine("GrappleCamera");
						//playerCamera.transform.position = new Vector3 (playerCamera.transform.position.x, playerCamera.transform.position.y - grappleCameraY, playerCamera.transform.position.z);
					}
					else 
					{
						GetComponent<Rigidbody2D>().gravityScale = positiveGravity;
						StartCoroutine("GroundCamera");
						//playerCamera.transform.position = new Vector3 (playerCamera.transform.position.x, groundCameraY, playerCamera.transform.position.z);
					}



				}

			}
			/*
			if(toggled && grappling)
			   {
				ceiling = Physics2D.Raycast(player.transform.position,Vector2.up,grappleHeight,whatIsGround);
			   if(ceiling.collider == null)
					GetComponent<Rigidbody2D>().gravityScale = -GetComponent<Rigidbody2D>().gravityScale;
			}
			/*
			if(toggled && !grounded && grappling && !hittingCeiling)
				GetComponent<Rigidbody2D>().gravityScale = -GetComponent<Rigidbody2D>().gravityScale;
			
			if(grounded)
				grappling = false;

			//If Ceiling ends, reverse gravity too

			if(toggled && hittingCeiling)
			{
				grappling = true;
			}

			/*
			if(toggled && grappling && !hittingCeiling)
			{
				grappling = false;
				GetComponent<Rigidbody2D>().gravityScale = -GetComponent<Rigidbody2D>().gravityScale;

			}
	*/

		}
	}
/*
	public void GrappleToggleDelay()
	{
		//yield return new WaitForSeconds (0.25f);
		//toggled = !toggled;
		if( hittingCeiling && GetComponent<Rigidbody2D>().gravityScale < 0)
			grappling = !grappling;
	}
*/
	
	public void Jump()
	{
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, jumpHeight);
	}
	public void wallJump(bool wallJumped)
	{
		if(!wallJumped)
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, wallJumpHeight);
		else
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, chainWallJumpHeight);
	}
	
	public void Move()
	{
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveVelocity, GetComponent<Rigidbody2D> ().velocity.y);
	}

	public IEnumerator MakePlayerInvisible()
	{	
		//Makeshift - Changing Tag to anything from Player so that enemies dont recognize 
		//player.tag = "Invisible";
		if (player.tag != "ControllingEnemy")
		{

			player.tag = "Invisible";
			Debug.Log ("Invisible");
			yield return new WaitForSeconds (invisibilityTime);
			Debug.Log ("No Longer Invisible");
			if(player.tag != "ControllingEnemy")
				player.tag = "Player";

		}
	}

	public IEnumerator GrappleCamera()
	{	
		while (cameraAdjusting)
		{
			yield return new WaitForSeconds (0.1f);
		}
		cameraAdjusting = true;
			
	

		//Player must have hit the ceiling

		while (!hittingCeiling && !dontAdjustCamera) {
			loopTime += 0.1f;
			yield return new WaitForSeconds (0.1f);
			if (loopTime > 1f) {
				cameraAdjusting = false;
				dontAdjustCamera = true;
			}

		}



		if (!dontAdjustCamera)
		{
			originalPosition = playerCamera.transform.position.y; 

			while (playerCamera.transform.position.y > originalPosition + grappleCameraY) 
			{
				playerCamera.transform.position = new Vector3 (playerCamera.transform.position.x, playerCamera.transform.position.y - cameraSmoothing, playerCamera.transform.position.z);
				yield return new WaitForSeconds (0.000005f);
			}

			//playerCamera.transform.position = new Vector3 (playerCamera.transform.position.x, playerCamera.transform.position.y + grappleCameraY, playerCamera.transform.position.z);

			Debug.Log ("Changed Camera Grapple");
			cameraAdjusted = true;

			cameraAdjusting = false;
		}
	}




	public IEnumerator GroundCamera()
	{	
		while (cameraAdjusting)
		{
			yield return new WaitForSeconds (0.1f);
		}
			cameraAdjusting = true;
			while (!grounded && cameraAdjusted)
			{
				yield return new WaitForSeconds (0.1f);
			}

			originalPosition = playerCamera.transform.position.y; 
			while (playerCamera.transform.position.y < originalPosition - grappleCameraY) {
				playerCamera.transform.position = new Vector3 (playerCamera.transform.position.x, playerCamera.transform.position.y + cameraSmoothing, playerCamera.transform.position.z);
				yield return new WaitForSeconds (0.000005f);
			}
			//playerCamera.transform.position = new Vector3 (playerCamera.transform.position.x, groundCameraY, playerCamera.transform.position.z);
	
			Debug.Log ("Changed Camera Ground");
			cameraAdjusted = false;
			cameraAdjusting = false;
		}

	public IEnumerator EnsureNoGravityVacuum()
	{	
		if (player.tag != "ControllingEnemy")
		{
			
			player.tag = "Invisible";
			Debug.Log ("Invisible");
			yield return new WaitForSeconds (invisibilityTime);
			Debug.Log ("No Longer Invisible");
			if(player.tag != "ControllingEnemy")
				player.tag = "Player";
			
		}
	}




	//Disable Player Controller
	public void DisablePlayerControls()
	{
		player.tag = "ControllingEnemy";
		playerCamera.enabled = false;
		playerControlled = false; 
		player.enabled = false;

		player.GetComponent<Rigidbody2D> ().gravityScale = 0;
		player.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		player.GetComponent<Collider2D> ().enabled = false;
		player.GetComponent<Rigidbody2D> ().Sleep ();
		Debug.Log ("Player Attached");
	}

	/*
	public IEnumerator ChildMaker()
	{
		making = true;
		yield return new WaitForSeconds (2f);
		//player.transform.position = GameObject.FindGameObjectWithTag("AttachPosition").transform.position;
		player.transform.parent = GameObject.FindGameObjectWithTag("AttachPosition").transform ;
		yield return new WaitForSeconds (3f);
		player.transform.parent = null;

		yield return new WaitForSeconds (3f);
		player.transform.position += new Vector3(6f, 1f, 1f);
		Debug.Log ("Player nulled");
		making = false;
	}
	*/




	//Enable Enemy Controller
	public void EnableEnemyControls(float enemy)
	{
		if (enemy == 1) {
			enemyObject = GameObject.FindGameObjectWithTag (enemyToAttachTo);
			enemyObject.GetComponent<GusController> ().enabled = false;
			enemyObject.GetComponent<PlayerController2> ().enabled = true;
			enemyObject.GetComponent<Rigidbody2D> ().isKinematic = false;
			enemyObject.GetComponent<Collider2D> ().enabled = true;

			//Enables the camera on the Enemy
			Transform.FindObjectOfType<Camera> ().enabled = true;
			//playerCamera.enabled = true;
			//enemyObject.GetComponent<Camera> ().enabled = true;

		} else if (enemy == 2) {
			enemyObject = GameObject.FindGameObjectWithTag (enemyToAttachTo);
			enemyObject.GetComponent<Gus2Controller> ().enabled = false;
			enemyObject.GetComponent<PlayerController3> ().enabled = true;
			enemyObject.GetComponent<Rigidbody2D> ().isKinematic = false;
			enemyObject.GetComponent<Collider2D> ().enabled = true;
			
			//Enables the camera on the Enemy
			Transform.FindObjectOfType<Camera> ().enabled = true;
		} 
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (!justDetached) {

			enemyToAttachTo = other.gameObject.tag;
			if (player.tag != "Invisible") {
				//Debug.Log (alive);
				if (other.gameObject.name == "Gus" && alive) {
					Debug.Log ("Attaching");
					attaching = true;
					//Disable Player Controller
					DisablePlayerControls ();
					//Enable Enemy Controller
					EnableEnemyControls (1);
				}
				if (other.gameObject.name == "Gus2" && alive) {
					Debug.Log ("Attaching");
					//Disable Player Controller
					DisablePlayerControls ();
					//Enable Enemy Controller
					EnableEnemyControls (2);
				}

			}
		}
	}
}