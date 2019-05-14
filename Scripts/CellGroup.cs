using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGroup : MonoBehaviour
{
    public int index;           // group index
    public List<Cell> cells;    // pieces in the group list
    public bool setted;			// group is set to the correct position
    [Space(10)]
    int widthPadding = 75;
    int heightPadding = 150;
    
    private bool canCombineOrBeSetted = false;

    private void Start()
    {
        StartCoroutine(ChangeGroupPos());
    }

    IEnumerator ChangeGroupPos()
    {
        int leftOrRight = Random.Range(0, 2);

        widthPadding = Camera.main.pixelWidth / 10; // 10% of screensize
        heightPadding = (3*Camera.main.pixelHeight) / 10; // 30% of screenheight

        yield return new WaitForSeconds(0.1f);

        switch (leftOrRight)
        {
            case 0://Left
//                transform.position = Camera.main.ScreenToWorldPoint(new Vector2(widthPadding, Random.Range(0, Camera.main.pixelHeight - heightPadding)));
                break;

            case 1://Right
//                transform.position = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth - widthPadding, Random.Range(0, Camera.main.pixelHeight - heightPadding)));
                break;

            case 2://Bottom
                //transform.position = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(widthPadding, Camera.main.pixelWidth - widthPadding), 0));
                break;

            case 3://Upper
                //transform.position = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(widthPadding, Camera.main.pixelWidth - widthPadding), Camera.main.pixelHeight));
                break;

            default:
                break;
        }

        transform.position = Camera.main.ScreenToWorldPoint(new Vector2(widthPadding, 0));
        transform.parent = GameObject.Find("Slider").transform;
        
        canCombineOrBeSetted = true;
    }

    public void SetParent(Transform newParent)
    {
        this.transform.parent= newParent;
    }

    // add a piece to the group
    public void AddCell(Cell cell)
    {
        cells.Add(cell);
        cell.transform.SetParent(transform);
        Vector3 pos = cell.transform.localPosition;
        pos.z = 0;
        cell.transform.localPosition = pos;
    }

    // adding a few pieces of the group
    public void AddCells(List<Cell> addCells)
    {
        foreach (Cell cell in addCells)
        {
            AddCell(cell);
        }
    }

    // moving groups according to the index layer
    public void UpdateIndex()
    {
        if (setted)
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -1);
        else
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -index * 2);
    }

    public void UpdateCellPos()
    {
        foreach (Cell cell in cells)
        {
            cell.curPos = cell.transform.localPosition + transform.localPosition;
            cell.curPos.z = 0;
        }
    }

    // attaching groups to the correct position
    public void SetToRightPosition(Vector3 delta)
    {
        if (canCombineOrBeSetted)
        {
            setted = true;
            transform.localPosition += delta;
            foreach (Cell cell in cells)
            {
                cell.mesh.GetComponent<BoxCollider>().enabled = false;
            }
            //TODO - COLOCAR SOM DE JUNTAR PEÇAS
            /*try
            {
                AudioManager.instance.PlaySound("SubMenu");
            }
            catch (System.Exception)
            {
                Debug.LogError("Erro: Audio Manager não iniciado!");
            }
            
            Debug.Log("Setou");
            */
        }
        
    }

    // setting group in a position to link to another group
    public void SetToLinkPosition(Vector3 delta)
    {
        
        if (canCombineOrBeSetted)
        {
            transform.localPosition += delta;
            foreach (Cell cell in cells)
            {
                cell.curPos += delta;
            }
            //TODO - COLOCAR SOM DE JUNTAR PEÇAS
            Debug.Log("Agrupou");
        }  
    }
}
