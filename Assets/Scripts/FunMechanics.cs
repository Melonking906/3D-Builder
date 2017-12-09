using UnityEngine;
using System.Collections;

public class FunMechanics : MonoBehaviour 
{
	private bool gravity = false;

	public GameObject UI;
	private UIScript uiScript;

	void Start ()
	{
		uiScript = UI.GetComponent<UIScript>();
	}

	void Update () 
	{
		if( Input.GetKeyDown( KeyCode.G ) )
		{
			if( gravity )
			{
				uiScript.ShowAlert("Physics Disabled!");
				gravity = false;
			} 
			else
			{
				uiScript.ShowAlert("Physics Enabled!");
				gravity = true;
			}
		}

		GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");

		foreach (GameObject block in blocks )
		{
			if( block.GetComponent<MeshCollider>() != null )
			{
				continue;
			}

			if( gravity )
			{
				block.GetComponent<Rigidbody>().isKinematic = false;
				block.GetComponent<Rigidbody>().useGravity = true;
			} 
			else
			{
				block.GetComponent<Rigidbody>().isKinematic = true;
				block.GetComponent<Rigidbody>().useGravity = false;
			}
		}
	}
}
