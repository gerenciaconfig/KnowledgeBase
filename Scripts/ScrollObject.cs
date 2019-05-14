using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScrollObject : MonoBehaviour {

    public int initialSpacing = 20;               // Space between pieces
    private float spacing;               // Space between pieces
    
    private float piecesSize;               // Size of each piece
    private Transform curChild;             // current child
    private float curChildColSize;          // current child's collider size

    private float firstChildPosY;           // First child y position 
    private float lastChildPosY;            // last child y position

    private ScrollMovement scrollMovement;

    private RectTransform border;

    void Start()
    {
        border = GameObject.FindGameObjectWithTag("CanvasMaskPivot").GetComponent<RectTransform>();

        scrollMovement = FindObjectOfType<ScrollMovement>();

        StartCoroutine(RearangePiecesFirstTime());
    }

    IEnumerator RearangePiecesFirstTime() {

        // Wait for the pieces 
        yield return new WaitForSeconds(.2f);

        ShuffleChildren();

        PutPiecesInPosition();

        CreateBounds();
    }

    public void RearangePieces()
    {
        PutPiecesInPosition();

        CreateBounds();
    }

    private void PutPiecesInPosition()
    {
        // Total of pieces
        var numOfPieces = transform.childCount;

        // Get Cam size
        Camera cam = Camera.main;
        float camHeight = cam.orthographicSize;

        int gridIndex = PlayerPrefs.GetInt("gridIndex");

        // Let the magic begins
        for (int i = 0; i < numOfPieces; i++)
        {

            // Get Current Child
            curChild = transform.GetChild(i);
            
            switch(gridIndex)
            {
                case 0:
                    curChild.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    break;

                case 1:
                    curChild.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                    break;

                case 2:
                    curChild.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                    break;
            }

            //curChild.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            // Get object's current box collider size.y
            curChildColSize = (curChild.GetComponentInChildren<BoxCollider>().bounds.size.y );

            spacing = (2 * curChildColSize / 6);

            // Do the magic
            curChild.localPosition = new Vector3(0,
                (border.position.y - initialSpacing - (curChildColSize * i) - spacing * i), 0);
        }
    }

    void ShuffleChildren() {
        
        // Get Children
        List<Transform> children = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++) {
            children.Add(transform.GetChild(i));
        }
        
        // Shuffle
        List<Transform> shuffledList = children.OrderBy( x => Random.value ).ToList( );

        // Set new position in hierarchy
        for (int i = 0; i < shuffledList.Count; i++) {
            shuffledList[i].SetAsLastSibling();
        }
    }

    void CreateBounds() {
        
        // TODO: Create bounds using first and last child's position

        if(transform.childCount > 1)
        {
            firstChildPosY = transform.GetChild(0).transform.position.y;
            lastChildPosY = transform.GetChild(transform.childCount - 2).transform.localPosition.y;
        }

        scrollMovement.SetBoundaries(- lastChildPosY + spacing);
    } 
}
