using UnityEngine;

public class GridBox : MonoBehaviour
{
    public bool isOccupied;

    //Check if a specific image piece is neighbour of this gridBox
    public bool IsNeighbourOfImagePiece(ImagePiece imagePiece)
    {
        return Vector3.Distance(transform.position, imagePiece.transform.position) <= 1f;
    }
}
