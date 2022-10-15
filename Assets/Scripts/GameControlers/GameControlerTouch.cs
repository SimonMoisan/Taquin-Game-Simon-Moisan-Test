using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControlerTouch : MonoBehaviour
{
    [SerializeField] private ImagePiece imagePiecePointed;
    [SerializeField] private ImagePiece imagePieceSelected;
    [SerializeField] private GridBox gridBoxToDropOn;

    private Ray ray;
    private RaycastHit hit;
    private Touch firstTouch;

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Game.playerCanPlay)
            return;

        if (Input.touchCount > 0)
        {
            firstTouch = Input.touches[0];
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(firstTouch.position);
            if (firstTouch.phase == TouchPhase.Began)
                ray = Camera.main.ScreenPointToRay(touchPosition);
        }

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        DetectImagePiecePointing();
        DetectImagePieceDragging();

        if (imagePieceSelected != null)
            DetectGridBoxDropping();
    }

    //Check if a piece image is pointed by the player
    private void DetectImagePiecePointing()
    {
        if (Physics.Raycast(ray, out hit) && hit.collider.tag == "ImagePiece")
        {
            imagePiecePointed = hit.collider.GetComponent<ImagePiece>();
        }
        else
            imagePiecePointed = null;
    }

    //Check if a piece image is selected by the player by touching it
    private void DetectImagePieceDragging()
    {
        if (imagePiecePointed != null && firstTouch.phase == TouchPhase.Began) //Drag image piece
        {
            imagePieceSelected = imagePiecePointed;
            imagePieceSelected.Select();
        } 

        if (imagePieceSelected != null && firstTouch.phase == TouchPhase.Ended && gridBoxToDropOn == null) //Drop image piece on invalid position
        {
            imagePieceSelected.UnSelect();
            imagePieceSelected = null;
        }
    }

    //Check if an image piece is dropped in a valid gridBox
    private void DetectGridBoxDropping()
    {
        if (Physics.Raycast(ray, out hit) && hit.collider.tag == "GridBox") //Point gridBox
        {
            gridBoxToDropOn = hit.collider.GetComponent<GridBox>();

            if (!gridBoxToDropOn.isOccupied && gridBoxToDropOn.IsNeighbourOfImagePiece(imagePieceSelected) && firstTouch.phase == TouchPhase.Ended)
            {
                imagePieceSelected.MoveToGridBox(gridBoxToDropOn, true);
                gridBoxToDropOn = null;
                imagePieceSelected = null;
            } 
        }
        else
            gridBoxToDropOn = null;
    }
}
