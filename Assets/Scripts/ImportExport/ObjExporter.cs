using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;

//Refrences code from the following sources:
//http://wiki.unity3d.com/index.php?title=ExportOBJ
//http://wiki.unity3d.com/index.php/ObjExporter


public class ObjExporterScript
{
	public static string MeshToString(MeshFilter mf) 
	{
		int vertexOffset = 0;
		int normalOffset = 0;
		int uvOffset = 0;

		Mesh m = mf.sharedMesh;
		Material[] mats = mf.GetComponent<Renderer>().sharedMaterials;

		StringBuilder sb = new StringBuilder();

		sb.Append("g ").Append(mf.name).Append("\n");
		foreach(Vector3 lv in m.vertices) 
		{
			Vector3 wv = mf.transform.TransformPoint(lv);

			//This is sort of ugly - inverting x-component since we're in
			//a different coordinate system than "everyone" is "used to".
			sb.Append(string.Format("v {0} {1} {2}\n",-wv.x,wv.y,wv.z));
		}
		sb.Append("\n");

		foreach(Vector3 lv in m.normals) 
		{
			Vector3 wv = mf.transform.TransformDirection(lv);

			sb.Append(string.Format("vn {0} {1} {2}\n",-wv.x,wv.y,wv.z));
		}
		sb.Append("\n");

		foreach(Vector3 v in m.uv) 
		{
			sb.Append(string.Format("vt {0} {1}\n",v.x,v.y));
		}

		for (int material=0; material < m.subMeshCount; material ++) {
			sb.Append("\n");
			sb.Append("usemtl ").Append(mats[material].name).Append("\n");
			sb.Append("usemap ").Append(mats[material].name).Append("\n");

			int[] triangles = m.GetTriangles(material);
			for (int i=0;i<triangles.Length;i+=3) 
			{
				//Because we inverted the x-component, we also needed to alter the triangle winding.
				sb.Append(string.Format("f {1}/{1}/{1} {0}/{0}/{0} {2}/{2}/{2}\n", 
					triangles[i]+1 + vertexOffset, triangles[i+1]+1 + normalOffset, triangles[i+2]+1 + uvOffset));
			}
		}

		vertexOffset += m.vertices.Length;
		normalOffset += m.normals.Length;
		uvOffset += m.uv.Length;

		return sb.ToString();
	}
}

public class ObjExporter : ScriptableObject
{
	public static string DoExport( GameObject render, bool makeSubmeshes )
	{
		string meshName = render.name;

		//string fileName = EditorUtility.SaveFilePanel("Export .obj file", "", meshName, "obj");
		string fileName = "";
		int fileCount = 0;
		do
		{
			fileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/render-" + fileCount + ".obj";
			fileCount++;
		} 
		while (File.Exists(fileName));

		StringBuilder meshString = new StringBuilder();

		meshString.Append("#" + meshName + ".obj"
			+ "\n#" + System.DateTime.Now.ToLongDateString() 
			+ "\n#" + System.DateTime.Now.ToLongTimeString()
			+ "\n#-------" 
			+ "\n\n");

		Transform t = render.transform;

		Vector3 originalPosition = t.position;
		t.position = Vector3.zero;

		if (!makeSubmeshes)
		{
			meshString.Append("g ").Append(t.name).Append("\n");
		}
		meshString.Append(processTransform(t, makeSubmeshes));

		WriteToFile(meshString.ToString(),fileName);

		t.position = originalPosition;

		return Path.GetFileName(fileName);
	}

	static string processTransform(Transform t, bool makeSubmeshes)
	{
		StringBuilder meshString = new StringBuilder();

		meshString.Append("#" + t.name
			+ "\n#-------" 
			+ "\n");

		if (makeSubmeshes)
		{
			meshString.Append("g ").Append(t.name).Append("\n");
		}

		MeshFilter mf = t.GetComponent<MeshFilter>();
		if (mf)
		{
			meshString.Append(ObjExporterScript.MeshToString(mf));
		}

		for(int i = 0; i < t.childCount; i++)
		{
			meshString.Append(processTransform(t.GetChild(i), makeSubmeshes));
		}

		return meshString.ToString();
	}

	static void WriteToFile(string s, string filename)
	{
		using (StreamWriter sw = new StreamWriter(filename)) 
		{
			sw.Write(s);
		}
	}
}