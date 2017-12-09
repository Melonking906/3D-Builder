using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshMaker : MonoBehaviour
{
	public void Render()
	{
		Vector3 savedLocation = transform.position;
		transform.position = new Vector3(0,0,0);

		GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
		List<MeshFilter> filterList = new List<MeshFilter>();

		foreach (GameObject block in blocks)
		{
			filterList.Add((MeshFilter)block.GetComponent("MeshFilter"));
		}

		//http://docs.unity3d.com/ScriptReference/Mesh.CombineMeshes.html

		MeshFilter[] meshFilters = filterList.ToArray();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];

		int i = 0;
		while (i < meshFilters.Length)
		{
			combine[i].mesh = meshFilters[i].sharedMesh;
			combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
			i++;
		}

		transform.GetComponent<MeshFilter>().mesh = new Mesh();
		transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);

		transform.GetComponent<MeshFilter>().mesh.RecalculateNormals();
		transform.GetComponent<MeshFilter>().mesh.RecalculateBounds();

		transform.position = savedLocation;
	}
}