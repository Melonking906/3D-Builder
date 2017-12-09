using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
	//GameUI Objects
    public GameObject BlockCountObject;
    public GameObject PointLocObject;
    public GameObject RotationObject;
	public GameObject BlockName;
	public GameObject SelectorDisplay;
	public GameObject AlertObject;

	public GameObject Menu;
	public GameObject ImportMenu;
	public GameObject GameUI;

    public GameObject Point;
	public GameObject MasterBlock;

    private Text blockCountText;
    private Text pointLocText;
    private Text rotationText;
	private Text blockNameText;
	private Text selectorDisplayText;
	private Text alertText;

	private Dictionary<string,GameObject> menus;
	private string selectedMenu = "game";
	private bool refreshMenus = true;

	private int savedSelectedBlock = -1;

	void Start ()
    {
        blockCountText = BlockCountObject.GetComponent<Text>();
        pointLocText = PointLocObject.GetComponent<Text>();
		rotationText = RotationObject.GetComponent<Text>();
		blockNameText = BlockName.GetComponent<Text>();
		selectorDisplayText = SelectorDisplay.GetComponent<Text>();
		alertText = AlertObject.GetComponent<Text>();

		alertText.text = "";

		menus = new Dictionary<string, GameObject>();
		menus.Add( "game", GameUI );
		menus.Add("main", Menu);
		menus.Add( "import", ImportMenu );
	}

	void Update ()
    {
        int blockCount = GameObject.FindGameObjectsWithTag("Block").Length;
        blockCountText.text = "Blocks: " + blockCount;

        Vector3 pointLoc = Point.transform.position;
        pointLocText.text = "Point: " + Mathf.Round(pointLoc.x) + " " + Mathf.Round(pointLoc.y) + " " + Mathf.Round(pointLoc.z);

		float rotation = Mathf.Floor(MasterBlock.transform.localEulerAngles.z);
		rotationText.text = "Rotation: " + rotation + "d";

		BlockMaster blockMaster = MasterBlock.GetComponent<BlockMaster>();
		if( blockMaster.SelectedBlock != savedSelectedBlock )
		{
			blockNameText.text = blockMaster.Block.name.Replace("(Clone)", "");

			string displayString = "";
			for( int i = 0; i < blockMaster.Blocks.Count; i++ )
			{
				if( blockMaster.SelectedBlock == i )
				{
					displayString += "+";
				} 
				else if( i == 0 )
				{
					displayString += "~";
				}
				else
				{
					displayString += "-";
				}
				displayString += " ";
			}

			selectorDisplayText.text = displayString;

			savedSelectedBlock = blockMaster.SelectedBlock;
		}

		//Mouse Managment
		if( selectedMenu.Equals( "game" ) )
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		} 
		else
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		//Main Menu Toggle
		if( Input.GetKeyDown( KeyCode.Escape ) )
		{
			if( selectedMenu.Equals( "game" ) )
			{
				setMenu( "main" );
			} 
			else
			{
				setMenu( "game" );
			}
		}

		//Menu Scripting
		if( !refreshMenus )
		{
			return;
		}
		refreshMenus = false;

		foreach( GameObject menu in menus.Values )
		{
			menu.SetActive(false);
		}

		GameObject openMenu = menus[selectedMenu];
		openMenu.SetActive(true);
    }

	private void setMenu(string menu)
	{
		refreshMenus = true;
		selectedMenu = menu;
	}

	public void CloseMenus()
	{
		setMenu("game");
	}

	public void OpenImportMenu()
	{
		setMenu("import");
	}

	public void ShowAlert( string message )
	{
		message = message.ToUpper();

		StartCoroutine(QuickShow( alertText, message, 2 ));
	}

	public bool IsUIControll()
	{
		return !selectedMenu.Equals("game");
	}

	IEnumerator QuickShow( Text textObject, string text, int seconds )
	{
		//string oldText = textObject.text;
		string oldText = "";

		textObject.text = text;

		yield return new WaitForSeconds( seconds );

		textObject.text = oldText;
	}
}
