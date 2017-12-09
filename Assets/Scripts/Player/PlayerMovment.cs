using UnityEngine;
using System.Collections;

public class PlayerMovment : MonoBehaviour 
{
	public float WalkSpeed = 3f;
	public float JetForce = 15;
	public GameObject Body;

	private float moveSpeed = 0f;
	private Rigidbody rigidbody;

	public GameObject UI;
	private UIScript uiScript;

	void Start()
	{
		uiScript = UI.GetComponent<UIScript>();
		rigidbody = gameObject.GetComponent<Rigidbody>();
	}

	void Update() 
	{
		rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);

		//MenuDisable
		if( uiScript.IsUIControll() )
		{
			GetComponent<SimpleSmoothMouseLook>().enabled = false;
			return;
		}
		GetComponent<SimpleSmoothMouseLook>().enabled = true;

		//Controlls
		if( Input.GetKey( KeyCode.Space ) )
		{
			rigidbody.AddForce( new Vector3( 0, JetForce, 0 ) );
		} 

		float speed = WalkSpeed;
		int moveInputs = 0;

		if( Input.GetKey( KeyCode.LeftShift ) )
		{
			speed = speed * 2;
		}

		if( Input.GetKey(KeyCode.W) )
		{
			moveInputs++;
			moveSpeed = speed / moveInputs;

			moveForward();
		}
		if( Input.GetKey(KeyCode.A) )
		{
			moveInputs++;
			moveSpeed = speed / moveInputs;

			moveLeft();
		}
		if( Input.GetKey(KeyCode.S) )
		{
			moveInputs++;
			moveSpeed = speed / moveInputs;

			moveBack();
		}
		if( Input.GetKey(KeyCode.D) )
		{
			moveInputs++;
			moveSpeed = speed / moveInputs;

			moveRight();
		}
	}

	private void moveForward() 
	{
		Vector3 ajustedForward = new Vector3(transform.forward.x, 0, transform.forward.z);

		RaycastHit hitInfo;
		if( Physics.Raycast( transform.position, transform.forward, out hitInfo, 1.1f ) )
		{
			return;
		}

		transform.localPosition += ajustedForward * moveSpeed * Time.deltaTime;
	}

	private void moveBack() 
	{
		Vector3 ajustedForward = new Vector3(transform.forward.x, 0, transform.forward.z);

		transform.localPosition -= ajustedForward * moveSpeed * Time.deltaTime;
	}

	private void moveRight() 
	{
		transform.localPosition += transform.right * moveSpeed * Time.deltaTime;
	}

	private void moveLeft() 
	{
		transform.localPosition -= transform.right * moveSpeed * Time.deltaTime;
	}
}
