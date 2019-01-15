using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //public InputField count;
    public int count = 4;

    public int firstSettingLimitNum = 1;

    public float myCellSize;
    public List<CellNum> cellNums;

    public GameObject cellsPanel;
    public GameObject cellPrefab;
    public GameObject cellNumPanel;
    public GameObject cellNumPrefab;

    private Vector3 firstPos = Vector3.zero;

    private void Start()
    {
        SetGridMap(count);
        SetCells(count);

        DrawRandomCells(count, firstSettingLimitNum);
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
        for (int c = 0; c < count; c++)
        {
            for (int r = 0; r < count; r++)
            {
                DrawCells(cellPrefab, cellsPanel, c, r, myCellSize, string.Format("Cell ({0} {1})", c, r));
            }
        }
    }

    private void DrawRandomCells(int count, int totalCellNum)
    {
        int limitCount = 0;

        while (limitCount < totalCellNum)
        {
            int col = Random.Range(0, count);
            int row = Random.Range(0, count);

            if (IsEmpty(col, row))
            {
                DrawCellNum(cellNumPrefab, cellNumPanel, col, row);
                limitCount++;
            }
        }
    }

    private bool IsEmpty(int col, int row)
    {
        foreach (CellNum cellNum in cellNums)
        {
            if (cellNum.c == col && cellNum.r == row)
            {
                return false;
            }
        }
        return true;
    }

    private void DrawCellNum(GameObject cellNumPrefab, GameObject cellNumPanel, int col, int row)
    {
        GameObject cel = Instantiate(cellNumPrefab, cellNumPanel.transform);
        cel.GetComponent<RectTransform>().localPosition = PointToVector3(col, row);

        //cel.GetComponent<Image>().color = Random.ColorHSV();

        var cellNum = cel.GetComponent<CellNum>();
        cellNum.c = col;
        cellNum.r = row;
        cellNums.Add(cellNum);
    }

    private void DrawCells(GameObject cellPrefab, GameObject CellsPanel, int c, int r, float cellSize, string cellname)
    {
        GameObject cel = Instantiate(cellPrefab, CellsPanel.transform);
        cel.GetComponent<RectTransform>().localPosition = PointToVector3(c, r);
        cel.name = cellname;
    }

    private Vector3 PointToVector3(int col, int row)
    {
        return new Vector3(firstPos.x + col * myCellSize, firstPos.y + row * myCellSize, 0);
    }

    private CellNum GetTile(int col, int row)
    {
        foreach (CellNum cellNum in cellNums)
        {
            if (cellNum.c == col && cellNum.r == row)
            {
                return cellNum;
            }
        }
        return null;
    }

    public void OnR_Button()
    {
        int col = +1;

        bool moveCellCheck = MoveCells(col, 0);
        DrawOneCell(moveCellCheck);
    }

    public void OnL_Button()
    {
        int col = -1;
        bool moveCellCheck = MoveCells(col, 0);
        DrawOneCell(moveCellCheck);
    }

    public void OnT_Button()
    {
        int row = +1;
        bool moveCellCheck = MoveCells(0, row);
        DrawOneCell(moveCellCheck);
    }

    public void OnB_Button()
    {
        int row = -1;
        bool moveCellCheck = MoveCells(0, row);
        DrawOneCell(moveCellCheck);
    }

    private bool MoveCells(int col, int row)
    {
        int movingCheck = 0;
        foreach (var cell in cellNums)
        {
            var nextR = cell.r;
            var nextC = cell.c;
            nextR += row;
            nextC += col;
            if (true)
            {
                cell.r = nextR;
                cell.c = nextC;
                MovingCells(cell, cell.c, cell.r);
                movingCheck++;
            }
        }

        if (movingCheck > 0)
        {
            return true;
        }
        else
            return false;
    }

    private void MovingCells(CellNum cell, int col, int row)
    {
        cell.GetComponent<RectTransform>().localPosition = PointToVector3(col, row);
    }

    private void DrawOneCell(bool isMove)
    {
        if (isMove)
        {
            DrawRandomCells(count, 1);
            Debug.Log("Draw One Cell");
        }
    }
}