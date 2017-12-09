using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PrinterSelector : MonoBehaviour 
{
	public GameObject SettingsObject;
	public GameObject Room;

	private Dropdown dropdown;
	private RoomScript roomScript;

	void Start() 
	{
		dropdown = this.GetComponent<Dropdown>();
		roomScript = Room.GetComponent<RoomScript>();

		dropdown.onValueChanged.AddListener( delegate {
			onDropdownChange( dropdown );
		} );
	}

	void Destroy() 
	{
		dropdown.onValueChanged.RemoveAllListeners();
	}

	private void onDropdownChange( Dropdown d )
	{
		SettingsObject.GetComponent<BSettings>().SelectedType = d.captionText.text;
		roomScript.ReloadRoom();
	}
}