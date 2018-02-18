using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;
public class Player : MonoBehaviour {
	public GameObject pres;
	bool Add = false;
	public Image dis;
	public Slider en;
	private bool EnIs = true;
	public float alphaLevel;
	public SpriteRenderer Op;
	public Slider Bar;
	public float maxSpeed = 5f;
	public float energy = 50f;
	float seconds = 5f;
	public Collider2D col;
	public float jumpForce = 5f;
	bool facingRight = true;
	Rigidbody2D myRB;
	public float health = 100;
    Animator anim;

	//GroundCheck
	public bool isGrounded;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask WhatIsGround;


	void Start () {
		health = 100f;
		Op = GetComponent<SpriteRenderer> ();
		en.GetComponent<Slider> ();
		col = GetComponent<Collider2D> ();
		anim = GetComponent<Animator> ();
		myRB = GetComponent<Rigidbody2D> ();
	
	}


	void FixedUpdate () {
		
		isGrounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, WhatIsGround);
		anim.SetBool ("Ground",isGrounded);
		anim.SetFloat ("vSpeed",myRB.velocity.y);

		if (isGrounded) {
			Move ();
			Fight ();

		}
		if (EnIs) {
		
			StartCoroutine (Magic());
		}
		if (Input.GetKeyDown (KeyCode.M)) {
			EnIs = false;
		}

   }
	void Update(){
		
		if(health<100f){
			Add = true;
		}
		else{
			Add = false;
		}
		if(Add){
			health = health + 1f/2;	
		}

		en.maxValue = 100f;
		en.value = energy;
		Bar.maxValue = 100f;
		Bar.value = health;
		if (isGrounded && Input.GetKeyDown (KeyCode.Space)) {
			myRB.AddForce(new Vector2 (0,jumpForce));
			anim.SetBool ("Ground",false);
		}

		Energy ();

	}

	void Flip(){
		facingRight =  !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;

	}
	void Move(){                                                                              //Move
		float move = Input.GetAxis ("Horizontal");
		anim.SetFloat ("Speed",Mathf.Abs(move));
		myRB.velocity = new Vector2 (move * maxSpeed,myRB.velocity.y);

		if (move > 0 && !facingRight) {
			Flip ();
		}
		else if(move < 0 && facingRight){
			Flip ();
		}



	}
	 void Fight(){                                                                                //Fight
		if (Input.GetMouseButton (0)) {
			anim.SetTrigger ("Attack");
		}
	}
	IEnumerator Magic(){
		if (Input.GetKeyDown (KeyCode.M)&&energy>=100f) {
			col.isTrigger = true;
			GetComponent<SpriteRenderer> ().color = new Color (1,1,1,0.5f);
			yield return new WaitForSeconds (seconds);
			if (seconds == 2.5f) {
				pres.SetActive (false);

			}
			if (seconds == 5f) {
				pres.SetActive (false);
				col.isTrigger = false;
				GetComponent<SpriteRenderer> ().color = new Color (1,1,1,1);
			}

			energy = energy - 80f;

		}
	
	}



	void Energy(){

		if (Input.GetKey (KeyCode.E)) {
			energy = energy + 0.5f;

		}
	}






	void OnCollisionEnter2D(Collision2D other){
	if (other.gameObject.tag == "PlatformDead") {
			
		health = health - 20;

	}
}

}