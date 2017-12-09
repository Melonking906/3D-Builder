using UnityEngine;
using System.Collections;

public class ExportBlock : MonoBehaviour 
{
	public GameObject Render;

	public GameObject UI;
	private UIScript uiScript;

	void Start ()
	{
		uiScript = UI.GetComponent<UIScript>();
	}

	public void Export()
	{
		if(GameObject.FindGameObjectsWithTag("Block").Length < 1)
		{
			uiScript.ShowAlert("Nothing to export!");
			return;
		}

		Render.GetComponent<MeshMaker>().Render();

		Vector3 saveScale = Render.transform.localScale;
		float exportScale = 14f;
		Render.transform.localScale = new Vector3(saveScale.x * exportScale, saveScale.y * exportScale, saveScale.z * exportScale);

		string name = ObjExporter.DoExport(Render, false);

		Render.transform.localScale = saveScale;

		uiScript.CloseMenus();
		uiScript.ShowAlert("Exported! - " + name);
	}
}