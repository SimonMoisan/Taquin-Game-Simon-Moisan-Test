using System.Collections;
using UnityEngine;

public class ImagePiece : MonoBehaviour
{
    public Vector3 rightPosition;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color selectedColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool IsImageAtRightPosition()
    {
        return transform.position == rightPosition;
    }

    public void MoveToGridBox(GridBox gridBox, bool useAnimation)
    {
        Vector3 targetPosition = new Vector3(gridBox.transform.position.x, gridBox.transform.position.y, transform.position.z);

        //Animation for player moves only, nor for shuffle moves
        if(useAnimation)
            StartCoroutine(MoveToGridBoxCoroutine(targetPosition));
        else
        {
            transform.position = targetPosition;
            GridManager.Grid.UpdateGridBoxesStates();
        }   
    }

    //Coroutine of image piece move animation, player can't play during animation
    IEnumerator MoveToGridBoxCoroutine(Vector3 targetPosition)
    {
        GameManager.Game.playerCanPlay = false;
        LeanTween.moveLocal(gameObject, targetPosition, 0.25f);
        yield return new WaitForSeconds(0.25f);
        UnSelect();
        GridManager.Grid.UpdateGridBoxesStates();
        GameManager.Game.playerCanPlay = true;
    }

    public bool IsNeighbourOfGridBox(GridBox gridBox)
    {
        return Vector3.Distance(transform.position, gridBox.transform.position) <= 1f;
    }

    public void Select()
    {
        spriteRenderer.color = selectedColor;
    }

    public void UnSelect()
    {
        spriteRenderer.color = defaultColor;
    }
}
