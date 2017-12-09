using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockMaster : MonoBehaviour
{
    public List<GameObject> Blocks;
    public GameObject Block { get; private set; }

	public int SelectedBlock { get; private set; }
    private bool resetFlipFlop = false;

    void Start ()
    {
		SelectedBlock = 0;
        UpdateBlock();
    }
	
	void Update ()
    {
		Block.GetComponent<MeshRenderer>().enabled = true;

		if( SelectedBlock == 0 )
		{
			Block.GetComponent<MeshRenderer>().enabled = false;
		}

        Block.transform.position = this.transform.position;
        Block.transform.rotation = this.transform.rotation;

        //++ Input Controlls ++

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
			if (SelectedBlock < Blocks.Count-1)
            {
				SelectedBlock++;
                UpdateBlock();
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
			if (SelectedBlock > 0)
            {
				SelectedBlock--;
                UpdateBlock();
            }
        }

		for( int i = 0; i < Blocks.Count; i++ )
		{
			if( Input.GetKeyDown( i.ToString() ) )
			{
				SelectedBlock = i; 
				UpdateBlock();
			}
		}

        if (Input.GetKey(KeyCode.Q))
        {
            this.transform.Rotate(0, 0, 2);
        }

        if (Input.GetKey(KeyCode.E))
        {
            this.transform.Rotate( 0, 0, -2 );
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            this.transform.rotation = new Quaternion();

            if (resetFlipFlop)
            {
                resetFlipFlop = false;
            }
            else
            {
                this.transform.Rotate(0, 0, 90);
                resetFlipFlop = true;
            }
        }
    }

	public void UpdateBlock()
	{
		UpdateBlock(Blocks[SelectedBlock]);
	}

	public void UpdateBlock(GameObject block)
    {
        //Clear any old master block
        Destroy(Block);

        //Spawn a new master block;
		Block = (GameObject) Instantiate(block, this.gameObject.transform.position, this.gameObject.transform.rotation);
        Block.transform.localScale = new Vector3(Block.transform.localScale.x / 2, Block.transform.localScale.y / 2, Block.transform.localScale.z / 2);
    }

	public void AddBlock(GameObject block)
	{
		Blocks.Add( block );
		SelectedBlock = Blocks.Count - 1;
		UpdateBlock();
	}
}
