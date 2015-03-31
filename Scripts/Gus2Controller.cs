using UnityEngine;
using System.Collections;

public class Gus2Controller : MonoBehaviour {
	
	public float moveSpeed;
	public static bool moveRight;
	
	public Rigidbody2D rbd;

	private bool grounded;

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
	
	// Use this for initialization
	void Start ()
	{
		//rbd = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		//player = FindObjectOfType<PlayerController>();
		if (fixedScaling)
			enemyScaleY = enemyScaleZ = enemyScaleX;
		
		gameObject.transform.localScale = new Vector3(enemyScaleX,enemyScaleY,enemyScaleZ);
		
	}
	
	// Function called certain amount of times per second/frame - unsure
	void FixedUpdate()
	{
		hittingWall = Physics2D.OverlapCircle (wallCheck.position, wallCheckRadius, whatIsWall);
		hittingPlayer = Physics2D.OverlapCircle (playerCheck.position, playerCheckRadius, whatIsPlayer);
		//grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
	}
	
	
	void Update () 
	{ 
		
		if (hittingWall )
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
		
		/*
		if (PlayerController.justDetached == true)
			GetComponent<CircleCollider2D>().enabled = false;
		else {
			GetComponent<CircleCollider2D>().enabled = true;
		}
		*/
	}
	
}
