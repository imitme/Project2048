using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public InputField count;

    public float myCellSize;

    public GameObject cellsPanel;
    public GameObject cellPrefab;

    private Vector3 firstPos = Vector3.zero;

    public void OnStartButton()
    {
    }

    private void Start()
    {
        int count = 4;
        SetGridMap(count);
        SetCells(count);
    }

    private void SetGridMap(int count)
    {
        var myPanel = cellsPanel.GetComponent<RectTransform>();
        Vector2 myPanelSize = myPanel.sizeDelta;

        var firstPositionX = myPanelSize.x / count / 2;
        var firstPositionY = myPanelSize.y / count / 2;
        Vector2 firstPosition = new Vector2(firstPositionX, firstPositionY);
        firstPos = firstPosition;
        Debug.Log(firstPosition + "----" + firstPos);

        var cellSize = myPanelSize.x / count;
        myCellSize = cellSize;
    }

    private void SetCells(int count)
    {
        Vector2 span = new Vector2(0, 0);
        for (int c = 0; c < count; c++)
        {
            for (int r = 0; r < count; r++)
            {
                MakeCells(cellPrefab, cellsPanel, c, r, myCellSize);
            }
        }
    }

    private void MakeCells(GameObject cellPrefab, GameObject CellsPanel, int c, int r, float cellSize)
    {
        GameObject cel = Instantiate(cellPrefab, CellsPanel.transform);
        cel.GetComponent<RectTransform>().localPosition = PointToVector3(c, r);
    }

    private Vector3 PointToVector3(int col, int row)
    {
        return new Vector3(firstPos.x + col * myCellSize, firstPos.y + row * myCellSize, 0);
    }
}