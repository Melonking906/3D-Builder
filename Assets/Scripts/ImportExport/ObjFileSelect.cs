using UnityEngine;

//Modified from http://wiki.unity3d.com/index.php?title=ImprovedFileBrowser
using System;
using UnityEngine.UI;


public class ObjFileSelect : MonoBehaviour
{
	public GameObject PathField;

	protected FileBrowser fileBrowser;

	[SerializeField]
	protected Texture2D	m_directoryImage, m_fileImage;

	protected void OnGUI()
	{
		if( fileBrowser != null )
		{
			fileBrowser.OnGUI();
		} 
		else
		{
			OnGUIMain();
		}
	}

	protected void OnGUIMain()
	{
		fileBrowser = new FileBrowser( new Rect( 100, 100, 600, 500 ), "Pick an obj file", FileSelectedCallback);
		fileBrowser.SelectionPattern = "*.obj";
		fileBrowser.DirectoryImage = m_directoryImage;
		fileBrowser.FileImage = m_fileImage;
		fileBrowser.CurrentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		fileBrowser.BrowserType = FileBrowserType.File;
	}

	protected void FileSelectedCallback( string path )
	{
		fileBrowser = null;
		PathField.GetComponent<InputField>().text = path;
		this.gameObject.SetActive(false);
	}
}