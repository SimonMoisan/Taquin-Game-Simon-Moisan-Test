using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid limits")]
    [SerializeField] private int xSize;
    [SerializeField] private int ySize;
    [Space]
    [SerializeField] private List<GridRow> gridRows;
    [SerializeField] private GridBox gridBoxPrefab;
    [Space]
    [SerializeField] private List<ImagePiece> imagePiecesPrefabs;
    [SerializeField] private List<ImagePiece> imagePiecesOnGrid;
    [Header("Image layout Randomizer")]
    [SerializeField] private int minRandomMovementNbr;
    [SerializeField] private int maxRandomMovementNbr;

    private bool firstUpdateDone;

    public static GridManager Grid;
    private void Awake()
    {
        if (Grid == null)
            Grid = this;
    }

    public void LoadImagePieces(List<ImagePiece> imagePiecesToLoad)
    {
        imagePiecesPrefabs = imagePiecesToLoad;
        InitiateGrid();
        InitiateImagePiecesOnGrid();
    }

    //Put gridBoxes
    private void InitiateGrid()
    {
        for (int y = 0; y < ySize; y++)
        {
            GridRow newGridRow = new GridRow();

            for (int x = 0; x < xSize; x++)
            {
                GridBox newGridBox = Instantiate(gridBoxPrefab, transform);
                newGridBox.transform.position = new Vector3(x, y, 0);

                newGridRow.gridBoxes.Add(newGridBox);
            }

            gridRows.Add(newGridRow);
        }
    }

    //Put image pieces on gridBoxes and shuffle them
    private void InitiateImagePiecesOnGrid()
    {
        imagePiecesOnGrid = new List<ImagePiece>();
        for (int i = 0; i < imagePiecesPrefabs.Count; i++)
        {
            ImagePiece newImagePiece = Instantiate(imagePiecesPrefabs[i], transform);
            imagePiecesOnGrid.Add(newImagePiece);
        }

        UpdateGridBoxesStates();
        RandomizeImagePiecePosition();
    }

    //Update states of every gridBoxes (occupied or not)
    public void UpdateGridBoxesStates()
    {
        ResetGridBoxesStates();

        for (int i = 0; i < imagePiecesOnGrid.Count; i++)
        {
            int imagePiecePositionX = Mathf.RoundToInt(imagePiecesOnGrid[i].transform.position.x);
            int imagePiecePositionY = Mathf.RoundToInt(imagePiecesOnGrid[i].transform.position.y);

            gridRows[imagePiecePositionY].gridBoxes[imagePiecePositionX].isOccupied = true;
        }

        if(firstUpdateDone)
            IsImageRight();

        firstUpdateDone = true;
    }

    //Set every gridBoxes to "not occupied" state
    private void ResetGridBoxesStates()
    {
        for (int i = 0; i < gridRows.Count; i++)
            for (int j = 0; j < gridRows[i].gridBoxes.Count; j++)
                gridRows[i].gridBoxes[j].isOccupied = false;
    }

    //Check if each image piece is at it's right position, if it's the case the player win !
    public void IsImageRight()
    {
        for (int i = 0; i < imagePiecesOnGrid.Count; i++)
            if (!imagePiecesOnGrid[i].IsImageAtRightPosition())
                return;

        GameManager.Game.GameWin();
    }

    //Shuffle Image pieces position by moving them in random position, so puzzle is always solvable
    private void RandomizeImagePiecePosition()
    {
        int randomMovementNbr = Random.Range(minRandomMovementNbr, maxRandomMovementNbr);

        ImagePiece previouslyMovedImagePiece = null;
        for (int i = 0; i < randomMovementNbr; i++)
        {
            //Find the not occupied grid box and all of its image piece neightbours
            GridBox notOccupiedGridBox = GetNotOccupiedGridBox();
            List<ImagePiece> imagePieceNeighboursOfNotOccupiedGridBox = GetImagePieceNeighbourOfGridBox(notOccupiedGridBox);

            //Remove previously moved image piece to avoid rollback of pieces
            if(previouslyMovedImagePiece != null && imagePieceNeighboursOfNotOccupiedGridBox.Contains(previouslyMovedImagePiece))
                imagePieceNeighboursOfNotOccupiedGridBox.Remove(previouslyMovedImagePiece);

            //Choose a random image piece to move to the not occupied gridBox
            int randomImagePieceIndex = Random.Range(0, imagePieceNeighboursOfNotOccupiedGridBox.Count);
            ImagePiece imagePieceToMove = imagePieceNeighboursOfNotOccupiedGridBox[randomImagePieceIndex];
            imagePieceToMove.MoveToGridBox(notOccupiedGridBox, false);
            previouslyMovedImagePiece = imagePieceToMove;

            UpdateGridBoxesStates();
        }
    }

    //Find the only one gridBox not occupied by an image piece
    private GridBox GetNotOccupiedGridBox()
    {
        for (int i = 0; i < gridRows.Count; i++)
            for (int j = 0; j < gridRows[i].gridBoxes.Count; j++)
            {
                if (!gridRows[i].gridBoxes[j].isOccupied)
                    return gridRows[i].gridBoxes[j];
            }
        return null;
    }

    //Get a list of every image pieces next to the unocciped gridBox
    private List<ImagePiece> GetImagePieceNeighbourOfGridBox(GridBox gridBox)
    {
        List<ImagePiece> imagePiecesNeighbour = new List<ImagePiece>();
        for (int i = 0; i < imagePiecesOnGrid.Count; i++)
        {
            if (gridBox.IsNeighbourOfImagePiece(imagePiecesOnGrid[i]))
                imagePiecesNeighbour.Add(imagePiecesOnGrid[i]);
        }

        return imagePiecesNeighbour;
    }

    [System.Serializable]
    public class GridRow
    {
        public List<GridBox> gridBoxes;

        public GridRow()
        {
            gridBoxes = new List<GridBox>();
        }
    }
}
