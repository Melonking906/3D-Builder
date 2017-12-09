using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonScripts : MonoBehaviour 
{
	public GameObject Room;
	public GameObject UI;

	private UIScript uiScript;
	private RoomScript roomScript;

	void Start()
	{
		uiScript = UI.GetComponent<UIScript>();
		roomScript = Room.GetComponent<RoomScript>();
	}

	public void OpenImport()
	{
		uiScript.OpenImportMenu();
	}

	public void ClearBlocks()
	{
		roomScript.EmptyRoom();
	}

	public void QuitBuilder()
	{
		Application.Quit();
	}
}
