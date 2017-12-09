using UnityEngine;
using System.Collections;

public class RoomScript : MonoBehaviour 
{
	private GameObject settingsObject;
	public GameObject Player;
	public GameObject Mark;

	private int roomScaleFactor = 15;
	private int markCount = 5;
	private float markSpace = 3.4f;

	void Start()
	{
		ReloadRoom();
	}

	public void ReloadRoom()
	{
		EmptyRoom();

		settingsObject = GameObject.FindGameObjectWithTag("Settings");

		BSettings.Printer printer = settingsObject.GetComponent<BSettings>().getSelectedPrinter();

		Vector3 scale = new Vector3(printer.X / roomScaleFactor, printer.Y / roomScaleFactor, printer.Z / roomScaleFactor);

		this.transform.localScale = scale;
		this.transform.position = new Vector3();

		Player.transform.position = new Vector3(0, 5, 0);

		//Mark Generators
		foreach( GameObject mark in GameObject.FindGameObjectsWithTag("Mark") )
		{
			Destroy(mark);
		}

		for( float x = -markCount; x < markCount+1; x++ )
		{
			for( float z = -markCount; z < markCount+1; z++ )
			{
				Vector3 markPoint = new Vector3(markSpace * x, -0.4f, markSpace * z);
				if( markPoint.x == 0 && markPoint.z == 0 )
				{
					continue;
				}

				PlaceAMark(markPoint);
			}
		}
	}

	private void PlaceAMark(Vector3 markPoint)
	{
		Vector3 markScale = new Vector3(0.5f, 1f, 0.5f);
		GameObject mark = (GameObject) Instantiate(Mark);
		mark.transform.parent = this.gameObject.transform;
		mark.transform.localScale = markScale;
		mark.transform.localPosition = markPoint; 
		mark.tag = "Mark";
	}

	public void EmptyRoom()
	{
		GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
		foreach( GameObject block in blocks )
		{
			Destroy( block );
		}
	}
}