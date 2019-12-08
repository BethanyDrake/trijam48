using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackGen : MonoBehaviour {
    public GameObject grass_wide_prefab;
    public GameObject grass_narrow_prefab; //just uses the wide one, for simplicity
    public GameObject bridge_wide_prefab;
    public GameObject bridge_narrow_prefab;
    public LinkedList<GameObject> pieces = new LinkedList<GameObject>();

    private GameObject piece_prefab;

    public bool raised = false;
    public float offset = 0.01f;

    //public GameObject prev_piece;P.Place(new_piece, angle);
    private GameObject new_piece;

    public float angle;
    public bool using_fences = true;
    public bool using_narrow = false;
    public bool using_bridge = false;

    [Range(-1, 1)]
    public int narrow_pos = 0;

    /*  Press UpArrow to add new straight piece
     *  Press Left/Right Arrow to add new turning piece
     *  
     *  hotkeys:
     *  press f to toggle fences
     *  press n to toggle narrow
     *  press b to toggle bridge
     *  q/e for left/right narrow positioning
     *  
     *  press backspace or downarrow to remove last piece
     *  
     */

    
    

    private GameObject level;
    // Use this for initialization
    void Start () {
        level = new GameObject();
        level.transform.position = Vector3.zero;
        level.name = "Level";
       

       
    }

	// Update is called once per frame
	void Update () {
        
        ReadToggles();
        ReadPiecePlacement();

        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            RemovePiece();
        }

        
    }

    void RemovePiece()
    {
        GameObject Last = pieces.First.Value;
        pieces.RemoveFirst();
        Destroy(Last);

        raised = !raised;
    }

    void ReadPiecePlacement()
    {   //adds straight or turning peices when player presses arrowkeys
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            AddPiece(0);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AddPiece(-angle);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            AddPiece(angle);
        }
    }
    void ReadToggles()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            using_fences = !using_fences;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            using_bridge = !using_bridge;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            using_narrow = !using_narrow;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            narrow_pos = -1;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            narrow_pos = 1;
        }


        UpdatePieceType();
    }

    void UpdatePieceType()
    {
        //update piece type
        if (using_narrow)
        {
            if (using_bridge)
            {
                piece_prefab = bridge_narrow_prefab;
            }
            else
            {
                piece_prefab = grass_narrow_prefab;

            }
        }
        else
        {
            if (using_bridge)
            {
                piece_prefab = bridge_wide_prefab;
            }
            else
            {
                piece_prefab = grass_wide_prefab;

            }
        }
    }

    void AddPiece(float angle)
    {
        new_piece = Instantiate(piece_prefab);
        P.Place(new_piece, angle);
    }

    PiecePlace P = new PiecePlace();
    void AddPiece1(float angle)
    {   //add new piece
        //+ve angle is a right turn

        new_piece = Instantiate(piece_prefab);
        
        GameObject prev_piece;
        if (pieces.Count == 0)
        {
            prev_piece = null;
            new_piece.transform.position = Vector3.zero;
        }
        else
        {
            prev_piece = pieces.First.Value;
            new_piece.transform.position = prev_piece.transform.position;
            new_piece.transform.rotation = prev_piece.transform.rotation;
        }
        

        

        //apply rotation
        new_piece.transform.Rotate(Vector3.up, angle);

        //align on "outside edge" so there are no gaps
        if (angle > 0) AlignLeft(new_piece,prev_piece);
        if (angle < 0 ) AlignRight(new_piece, prev_piece);
        if (angle == 0)
        {
            if (narrow_pos==-1) AlignLeft(new_piece, prev_piece);
            else AlignRight(new_piece, prev_piece);
        }

        //reduce flickering
        if (raised) new_piece.transform.position += offset * Vector3.up;
        else
        {
            new_piece.transform.position -= offset * Vector3.up;
            BoxCollider[] colls = new_piece.GetComponentsInChildren<BoxCollider>();
            foreach (BoxCollider coll in colls)
            {
               
                Vector3 newWorldPos = coll.transform.TransformPoint(coll.center) + offset * Vector3.up;
                coll.center = coll.transform.InverseTransformPoint(newWorldPos);
            }
        }
        

        //remove fences
        if (!using_fences)
        {
            Destroy(new_piece.transform.Find("Fences").gameObject);
        }

        //finish
        new_piece.transform.parent = level.transform;
        pieces.AddFirst(new_piece);
        raised = !raised;
        //prev_piece = new_piece;

    }



    void AlignLeft(GameObject moving_top, GameObject still_bottom)
    {   //moves the "moving_top" in position in front of "still_bottom", aligning on the left
        if (still_bottom == null) return;
        Vector3 from = moving_top.GetComponent<Anchors>().BL.transform.position;
        Vector3 to = still_bottom.GetComponent<Anchors>().TL.transform.position;

        moving_top.transform.position += to - from;
    }

    void AlignRight(GameObject moving_top, GameObject still_bottom)
    {   //moves the "moving_top" in position in front of "still_bottom", aligning on the right
        if (still_bottom == null) return;
        Vector3 from = moving_top.GetComponent<Anchors>().BR.transform.position;
        Vector3 to = still_bottom.GetComponent<Anchors>().TR.transform.position;

        moving_top.transform.position += to - from;
    }
}
