using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BSettings : MonoBehaviour 
{
	public string SelectedType;

	private List<Printer> printers;

	void Start () 
	{
		Object.DontDestroyOnLoad (this.gameObject);

		//All settings are in inches
		printers = new List<Printer>();
		printers.Add(new Printer("PrinterBot Play", 4f, 4f, 5f));
		printers.Add(new Printer("PrinterBot Simple", 6f, 6f, 6f));
		printers.Add(new Printer("PrinterBot Plus", 10f, 10f, 10f));
		printers.Add(new Printer("Ultimaker 2+", 8.7f, 8.7f, 8f));
		printers.Add(new Printer("Ultimaker 2 Go", 4.7f, 4.7f, 4.5f));

		SelectedType = printers[ 0 ].Name;
	}

	public Printer getSelectedPrinter()
	{
		foreach( Printer printer in printers )
		{
			if( printer.Name.Equals( SelectedType ) )
			{
				return printer;
			}
		}

		return null;
	}

	public class Printer
	{
		public float X { get; private set; }
		public float Y { get; private set; }
		public float Z { get; private set; }
		public string Name { get; private set; }

		public Printer(string name, float x, float y, float z)
		{
			Name = name;
			X = x;
			Y = y;
			Z = z;
		}
	}
}
