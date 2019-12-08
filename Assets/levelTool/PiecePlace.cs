using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PiecePlace {
    public LinkedList<GameObject> pieces = new LinkedList<GameObject>();
    private GameObject prev_piece = null; //first item in pieces, for convenience
    private GameObject new_piece = null; //peice we are currently adding
    public float offset = 0.01f; //each peice should be placed a little above or a little below the previous piece
    

    public void Place(GameObject piece, float angle)
    {
        //can only place pieces with an anchor component
        if (piece.GetComponent<Anchors>() == null)
        {
            Debug.LogError("Cannot place " + piece.name);
            return;
        }
        new_piece = piece;
        if (prev_piece != null)
        {

            ApplyRotation(angle);
            AlignAnchors(angle);
            ApplyOffset();
            
        }

        UpdateHistory();
        return;



    }

    public void RemovePiece()
    {
        //removes the most recently added piece
        pieces.RemoveFirst();
        prev_piece = pieces.First.Value;
        offset = -offset;
    }

    private void ApplyOffset()
    {   //raises or lowers a peice by a small amount to avoid flickering
        new_piece.transform.position += offset * Vector3.up;
    }

    private void UpdateHistory()
    {   
        pieces.AddFirst(new_piece);
        prev_piece = new_piece;
        offset = -offset;
        
    }

    private void ApplyRotation(float angle)
    {   
        //make new_piece have the same rotation as prev_piece, rotated by angle
        new_piece.transform.rotation = prev_piece.transform.rotation;
        new_piece.transform.Rotate(Vector3.up, angle);
    }

    private void AlignAnchors(float angle)
    {
        //align on "outside edge" so there are no gaps
        if (angle > 0) AlignLeft();
        if (angle < 0) AlignRight();
        if (angle == 0) AlignCenter();
   
    }

    void AlignCenter()
    {
        //align the peice on the left, then move half the distance needed to align right
        AlignLeft();
        Vector3 from = new_piece.GetComponent<Anchors>().BR.transform.position;
        Vector3 to = prev_piece.GetComponent<Anchors>().TR.transform.position;
        new_piece.transform.position += (to - from)/2;
    }

    void AlignLeft()
    {
        //aligns the bottom right anchor of new_peice with the top right anchor of prev_peice
        Vector3 from = new_piece.GetComponent<Anchors>().BL.transform.position;
        Vector3 to = prev_piece.GetComponent<Anchors>().TL.transform.position;

        new_piece.transform.position += to - from;
    }

    void AlignRight()
    {   //aligns the bottom right anchor of new_peice with the top right anchor of prev_peice
        Vector3 from = new_piece.GetComponent<Anchors>().BR.transform.position;
        Vector3 to = prev_piece.GetComponent<Anchors>().TR.transform.position;

        new_piece.transform.position += to - from;
    }

    
}
