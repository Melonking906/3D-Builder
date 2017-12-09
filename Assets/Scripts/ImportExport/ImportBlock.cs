using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImportBlock : MonoBehaviour 
{
	public GameObject PathField;
	public GameObject Scale;
	public GameObject BlankObject;
	public GameObject FileSelectorUI;
	public Material BlockMaterial;

	public GameObject MasterBlock;
	public GameObject UI;
	private UIScript uiScript;

	private ObjImporter importer;

	private Vector3 farLocation = new Vector3(0,0,5000);

	void Start ()
	{
		uiScript = UI.GetComponent<UIScript>();
		importer = GetComponent<ObjImporter>();
	}

	public void SelectFile()
	{
		FileSelectorUI.SetActive( true );
	}

	public void Import()
	{
		Mesh importedMesh = importer.ImportFile( PathField.GetComponent<InputField>().text );

		GameObject importedBlock = (GameObject) Instantiate( BlankObject, farLocation, transform.rotation );

		importedBlock.GetComponent<MeshFilter>().mesh = importedMesh;
		importedBlock.GetComponent<MeshCollider>().sharedMesh = importedMesh;
		importedBlock.GetComponent<MeshCollider>().sharedMesh.RecalculateBounds();
		importedBlock.GetComponent<MeshCollider>().sharedMesh.RecalculateNormals();
		importedBlock.GetComponent<MeshRenderer>().material = BlockMaterial;

		importedBlock.tag = "Block";

		float scale = 0.07f;

		string scaleString = Scale.GetComponent<InputField>().text;

		float parseScale = scale;
		if( float.TryParse( scaleString, out parseScale) )
		{
			scale = parseScale;
		}

		importedBlock.transform.localScale = new Vector3(scale, scale, scale);

		MasterBlock.GetComponent<BlockMaster>().AddBlock(importedBlock);

		uiScript.CloseMenus();
		uiScript.ShowAlert("Import Completed!");
	}
}