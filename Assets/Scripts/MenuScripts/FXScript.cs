using UnityEngine;
using System.Collections;

public class FXScript : MonoBehaviour 
{
	public GameObject SpinCube;

	void Update () 
	{
		SpinCube.transform.Rotate(Vector3.forward, 20f * Time.deltaTime);
	}
}
