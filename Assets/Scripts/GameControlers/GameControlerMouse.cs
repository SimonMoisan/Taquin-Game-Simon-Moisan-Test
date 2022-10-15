using UnityEngine;

public class GameControlerMouse : MonoBehaviour
{
    [SerializeField] private bool cheatOn; //used to move an image no matter if its a neighbour of the emplty gridBox or not
    [SerializeField] private ImagePiece imagePiecePointed;
    [SerializeField] private ImagePiece imagePieceSelected;
    [SerializeField] private GridBox gridBoxToDropOn;

    private Ray ray;
    private RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Game.playerCanPlay)
            return;

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
        if(imagePiecePointed != null && Input.GetMouseButtonDown(0)) //Drag image piece
        {
            imagePieceSelected = imagePiecePointed;
            imagePieceSelected.Select();
        }

        if (imagePieceSelected != null && Input.GetMouseButtonUp(0) && gridBoxToDropOn == null) //Drop image piece on invalid position
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

            if(!gridBoxToDropOn.isOccupied && (gridBoxToDropOn.IsNeighbourOfImagePiece(imagePieceSelected) || cheatOn) && Input.GetMouseButtonUp(0))
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
