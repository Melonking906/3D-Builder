using UnityEngine;
using System.Collections;

public class Tool : MonoBehaviour
{
    public GameObject point;
    public GameObject blockMaster;

	public Material blockMat;
	public Material selectedBlockMat;
	public Material holoMat;
	public Material pointerMat;

	public GameObject UI;
	private UIScript uiScript;

	private bool snapToggle = false;
	private bool carrying = false;

	private GameObject carriedObject;

    void Start ()
    {
		uiScript = UI.GetComponent<UIScript>();
    }

    void Update()
    {
        //Sight Ray
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
		Physics.Raycast( ray, out hit, 100 );

		//Disable clicking if the ui is open
		if( uiScript.IsUIControll() )
		{
			return;
		}

		//++ Spawn Handeling ++
		GameObject contactBlock = hit.rigidbody.gameObject;

		foreach( GameObject block in GameObject.FindGameObjectsWithTag("Block") )
		{
			block.GetComponent<MeshRenderer>().material = blockMat;
			if( block.Equals( contactBlock ) )
			{
				block.GetComponent<MeshRenderer>().material = selectedBlockMat;
			}
		}

		//Pointer to Holo Switch
		point.transform.localScale = blockMaster.GetComponent<BlockMaster>().Block.transform.localScale * 2;
		point.GetComponent<MeshFilter>().mesh = blockMaster.GetComponent<BlockMaster>().Block.GetComponent<MeshFilter>().mesh;

		if( blockMaster.GetComponent<BlockMaster>().SelectedBlock == 0 )
		{
			point.GetComponent<MeshRenderer>().material = pointerMat;

			point.transform.position = hit.point;
			point.transform.rotation = Quaternion.LookRotation(ray.direction, Vector3.up);
		} 
		else
		{
			Vector3 faceDirection = ray.direction;
			faceDirection.y = 0;

			point.transform.rotation = Quaternion.LookRotation( faceDirection, blockMaster.transform.up );
			point.GetComponent<MeshRenderer>().material = holoMat;

			point.layer = 8;
			point.GetComponent<BoxCollider>().enabled = true;

			if( snapToggle )
			{
				Vector3 snapPoint = hit.point;

				if( contactBlock.tag.Equals( "Block" ) )
				{
					point.transform.rotation = contactBlock.gameObject.transform.rotation;

					contactBlock.layer = 8;
					point.layer = 0;
					snapPoint = SurfaceCenterInDirection( contactBlock, hit.normal );
					point.layer = 8;
					contactBlock.layer = 0;
				}
				
				point.transform.position = ObjectOnSurfacePoint( point, snapPoint, hit.normal );
			} else
			{
				point.transform.position = ObjectOnSurfacePoint( point, hit.point, hit.normal );
			}

			point.GetComponent<BoxCollider>().enabled = false;
			point.layer = 2;
		}

		//Spawn Object
		float placeSpeed = 9;

		if (Input.GetMouseButtonDown(0))
		{
			if( contactBlock == null || !(contactBlock.CompareTag( "Block" ) || contactBlock.CompareTag( "BuildOn" )) )
			{
				uiScript.ShowAlert("Build on the floor please!");
				return;
			}

			//Dont allow placing of the pointer block
			if(blockMaster.GetComponent<BlockMaster>().SelectedBlock == 0)
			{
				return;
			}

			//Generate a Block
			GameObject block = (GameObject) Instantiate(blockMaster.GetComponent<BlockMaster>().Block);
			block.tag = "Block";
			block.transform.rotation = point.transform.rotation;
			block.layer = 0;
			ScaleBlock(block);

			//Position Block
			StartCoroutine(MoveBlock(block, point.transform.position, placeSpeed));
		}

		//Remove Object
		if (Input.GetMouseButtonDown(1))
		{
			if (hit.rigidbody.gameObject.tag.Equals("Block"))
			{
				Destroy(hit.rigidbody.gameObject);
			}
		}

		//++ Key Controll handeling ++
		if( Input.GetKeyDown( KeyCode.Tab ) )
		{
			if( snapToggle )
			{
				uiScript.ShowAlert("Stack Disabled!");
				snapToggle = false;
			} 
			else
			{
				uiScript.ShowAlert("Stack Enabled!");
				snapToggle = true;
			}
		}

		//++ Block Control ++
		GameObject hitBlock = hit.rigidbody.gameObject;
		if( hitBlock == null || !hitBlock.tag.Equals("Block") )
		{
			hitBlock = null;
		}

		Vector3 ajustedForward = new Vector3(transform.forward.x, 0, transform.forward.z);

		float moveStep = 0.05f;
		float moveSpeed = 0.5f;

		//HitBlock Controlls
		if( hitBlock != null )
		{
			bool shiftDown = false;
			if( Input.GetKey( KeyCode.LeftShift ) || Input.GetKey( KeyCode.RightShift ) )
			{
				shiftDown = true;

				float modifySpeed = 2f;

				if( Input.GetKey( KeyCode.UpArrow ) )
				{
					float scaleAmount = 2f;
					Vector3 newScale = new Vector3(hitBlock.transform.localScale.x, hitBlock.transform.localScale.y, hitBlock.transform.localScale.z * scaleAmount);

					hitBlock.transform.localScale = Vector3.Lerp( hitBlock.transform.localScale, newScale, Time.deltaTime/modifySpeed );
				}
				if( Input.GetKey( KeyCode.DownArrow ) )
				{
					float scaleAmount = 0.1f;
					Vector3 newScale = new Vector3(hitBlock.transform.localScale.x, hitBlock.transform.localScale.y, hitBlock.transform.localScale.z * scaleAmount);

					hitBlock.transform.localScale = Vector3.Lerp( hitBlock.transform.localScale, newScale, Time.deltaTime/modifySpeed );
				}
				if( Input.GetKey( KeyCode.LeftArrow ) )
				{
					float scaleAmount = 2f;
					Vector3 newScale = new Vector3(hitBlock.transform.localScale.x * scaleAmount, hitBlock.transform.localScale.y, hitBlock.transform.localScale.z);

					hitBlock.transform.localScale = Vector3.Lerp( hitBlock.transform.localScale, newScale, Time.deltaTime/modifySpeed );
				}
				if( Input.GetKey( KeyCode.RightArrow ) )
				{
					float scaleAmount = 0.1f;
					Vector3 newScale = new Vector3(hitBlock.transform.localScale.x * scaleAmount, hitBlock.transform.localScale.y, hitBlock.transform.localScale.z);

					hitBlock.transform.localScale = Vector3.Lerp( hitBlock.transform.localScale, newScale, Time.deltaTime/modifySpeed );
				}
				if( Input.GetKey( KeyCode.Period ) )
				{
					float scaleAmount = 2f;
					Vector3 newScale = new Vector3(hitBlock.transform.localScale.x, hitBlock.transform.localScale.y * scaleAmount, hitBlock.transform.localScale.z);

					hitBlock.transform.localScale = Vector3.Lerp( hitBlock.transform.localScale, newScale, Time.deltaTime/modifySpeed );
				}
				if( Input.GetKey( KeyCode.Comma ) )
				{
					float scaleAmount = 0.1f;
					Vector3 newScale = new Vector3(hitBlock.transform.localScale.x, hitBlock.transform.localScale.y * scaleAmount, hitBlock.transform.localScale.z);

					hitBlock.transform.localScale = Vector3.Lerp( hitBlock.transform.localScale, newScale, Time.deltaTime/modifySpeed );
				}
			}

			if( Input.GetKeyDown( KeyCode.C ) )
			{
				blockMaster.GetComponent<BlockMaster>().UpdateBlock(hitBlock);
			}
			if( Input.GetKey( KeyCode.L ) )
			{
				hitBlock.transform.localScale = Vector3.Lerp( hitBlock.transform.localScale, blockMaster.transform.localScale*2, Time.deltaTime/5 );
			}
			if( Input.GetKeyDown( KeyCode.UpArrow ) && !shiftDown )
			{
				Vector3 target = new Ray(hitBlock.transform.position, ajustedForward).GetPoint( moveStep );
				StartCoroutine(MoveBlock( hitBlock, target, moveSpeed ));
			}
			if( Input.GetKeyDown( KeyCode.DownArrow ) && !shiftDown )
			{
				Vector3 target = new Ray(hitBlock.transform.position, ajustedForward).GetPoint( -moveStep );
				StartCoroutine(MoveBlock( hitBlock, target, moveSpeed ));
			}
			if( Input.GetKeyDown( KeyCode.LeftArrow ) && !shiftDown )
			{
				Vector3 target = new Ray(hitBlock.transform.position, transform.right).GetPoint( -moveStep );
				StartCoroutine(MoveBlock( hitBlock, target, moveSpeed ));
			}
			if( Input.GetKeyDown( KeyCode.RightArrow ) && !shiftDown )
			{
				Vector3 target = new Ray(hitBlock.transform.position, transform.right).GetPoint( moveStep );
				StartCoroutine(MoveBlock( hitBlock, target, moveSpeed ));
			}
			if( Input.GetKeyDown( KeyCode.Period ) && !shiftDown )
			{
				Vector3 target = new Ray(hitBlock.transform.position, transform.up).GetPoint( moveStep );
				StartCoroutine(MoveBlock( hitBlock, target, moveSpeed ));
			}
			if( Input.GetKeyDown( KeyCode.Comma ) && !shiftDown )
			{
				Vector3 target = new Ray(hitBlock.transform.position, transform.up).GetPoint( -moveStep );
				StartCoroutine(MoveBlock( hitBlock, target, moveSpeed ));
			}
			if( Input.GetKey( KeyCode.LeftBracket ) )
			{
				hitBlock.transform.Rotate(0, 0, 1);
			}
			if( Input.GetKey( KeyCode.RightBracket ) )
			{
				hitBlock.transform.Rotate(0, 0, -1);
			}
			if( Input.GetKeyDown( KeyCode.Semicolon ) )
			{
				hitBlock.transform.Rotate(0, 90, 0);
			}
			if( Input.GetKey( KeyCode.Quote ) )
			{
				hitBlock.transform.Rotate(0, 1, 0);
			}
			if( Input.GetKey( KeyCode.Backslash ) )
			{
				hitBlock.transform.Rotate(0, -1, 0);
			}
			if( Input.GetKey( KeyCode.Quote ) )
			{
				hitBlock.transform.Rotate(0, 1, 0);
			}
			if( Input.GetKey( KeyCode.Equals ) )
			{
				float scaleAmount = 2f;
				Vector3 newScale = new Vector3(hitBlock.transform.localScale.x * scaleAmount, 
					hitBlock.transform.localScale.y * scaleAmount, 
					hitBlock.transform.localScale.z * scaleAmount);
				
				hitBlock.transform.localScale = Vector3.Lerp( hitBlock.transform.localScale, newScale, Time.deltaTime/5 );
			}
			if( Input.GetKey( KeyCode.Minus ) )
			{
				float scaleAmount = 0.1f;
				Vector3 newScale = new Vector3(hitBlock.transform.localScale.x * scaleAmount, 
					hitBlock.transform.localScale.y * scaleAmount, 
					hitBlock.transform.localScale.z * scaleAmount);

				hitBlock.transform.localScale = Vector3.Lerp( hitBlock.transform.localScale, newScale, Time.deltaTime/5 );
			}
		}

		//Carry Handeler
		if( carrying )
		{
			GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
			carriedObject.transform.position = camera.transform.position + camera.transform.forward * 1.5f;
		}

		if( Input.GetKeyDown( KeyCode.P ) )
		{
			if( carrying )
			{
				carrying = false;
			} 
			else
			{
				if( hitBlock != null )
				{
					carrying = true;
					carriedObject = hitBlock;
				}
			}
		}
    }

	IEnumerator MoveBlock( GameObject block, Vector3 target, float speed )
	{
		float moveStep = speed * Time.deltaTime;

		while( block.transform.position != target )
		{
			block.transform.position = Vector3.MoveTowards(block.transform.position, target, moveStep);
			yield return null;
		}
	}

	private void ScaleBlock( GameObject block )
	{
		block.transform.localScale = new Vector3(block.transform.localScale.x * 2, block.transform.localScale.y * 2, block.transform.localScale.z * 2);
	}

	private Vector3 ObjectOnSurfacePoint( GameObject gameObject, Vector3 surfacePoint, Vector3 direction )
	{
		Vector3 savedPosition = gameObject.transform.position;

		Ray surfaceRay = new Ray(surfacePoint, direction);

		gameObject.transform.position = surfaceRay.GetPoint(5);

		RaycastHit pointDistanceHit;
		if( Physics.Raycast(surfaceRay, out pointDistanceHit, Mathf.Infinity, (1 << 8)) )
		{
			Vector3 rayNormalDepth = gameObject.transform.position - pointDistanceHit.point;
		
			gameObject.transform.position = savedPosition;
			return surfacePoint + rayNormalDepth;
		}
		gameObject.transform.position = savedPosition;
		return new Vector3();
	}

	private Vector3 SurfaceCenterInDirection( GameObject gameObject, Vector3 direction )
	{
		Ray bounceRay = new Ray(gameObject.transform.position, direction);

		Vector3 bouncePoint = bounceRay.GetPoint(10);

		Vector3 returnDirection = gameObject.transform.position - bouncePoint;
		Ray returnBounceRay = new Ray(bouncePoint, returnDirection);

		RaycastHit bounceReturnHit;
		if( Physics.Raycast( returnBounceRay, out bounceReturnHit, Mathf.Infinity, (1 << 8) ) )
		{
			return bounceReturnHit.point;
		}

		return new Vector3();
	}
}