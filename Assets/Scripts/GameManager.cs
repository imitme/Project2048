using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DIRECTION
{
    UP = 0, DOWN, RIGHT, LIGHT, COUNT
};

public class GameManager : MonoBehaviour
{
    //public InputField count;
    public int count = 4;

    private DIRECTION dir;
    public int firstSettingLimitNum = 1;

    public float myCellSize;
    public List<CellNum> cellsNum;

    public GameObject cellsPanel;
    public GameObject cellsNumPanel;
    public GameObject cellPrefab;
    public GameObject cellNumPrefab;

    public GameObject endPanel;
    public GameObject playPanel;

    private Vector3 firstPos = Vector3.zero;

    private void Start()
    {
        Endgame();
    }

    private void Endgame()
    {
        endPanel.SetActive(true);
        playPanel.SetActive(false);
    }

    public void OnPlayButton()
    {
        PlayGameStart();
    }

    public void OnEndButton()
    {
        Endgame();
    }

    public void OnR_Button()
    {
        dir = DIRECTION.RIGHT;
        MovetoDir(dir);
    }

    public void OnL_Button()
    {
        dir = DIRECTION.LIGHT;
        //int col = -1;
        //int row = 0;
        //TestMovemove(col, row);
        MovetoDir(dir);
    }

    public void OnU_Button()
    {
        dir = DIRECTION.UP;
        MovetoDir(dir);
    }

    public void OnD_Button()
    {
        //int col = 0;
        //int row = -1;
        //TestMovemove(col, row);

        dir = DIRECTION.DOWN;
        MovetoDir(dir);
    }

    private void MovetoDir(DIRECTION dir)
    {
        //
        GetCellsDirLine(dir);
    }

    private void GetCellsDirLine(DIRECTION dir)
    {
        //List<CellNum> cellsDirList;
        switch (dir)
        {
            case DIRECTION.UP:
                MoveMoveCells(0, 1);
                break;

            case DIRECTION.DOWN:
                MoveMoveCells(0, -1);
                break;

            case DIRECTION.RIGHT:
                MoveMoveCells(1, 0);
                break;

            case DIRECTION.LIGHT:
                MoveMoveCells(-1, 0);
                break;

            case DIRECTION.COUNT:
                break;

            default:
                break;
        }
    }

    private void MoveMoveCells(int col, int row)
    {
        for (int i = 0; i < count; i++)
        {
            List<CellNum> celLine = new List<CellNum>();

            // 행마다 리스트뽑기
            foreach (var cel in cellsNum)
            {
                if (row == 0)
                {
                    if (i == cel.r)
                    {
                        var cellinline = GetCellNum(cel.c, cel.r);
                        celLine.Add(cellinline);
                    }
                }
                else
                {
                    if (i == cel.c)
                    {
                        var cellinline = GetCellNum(cel.c, cel.r);
                        celLine.Add(cellinline);
                    }
                }
            }

            foreach (var cel in celLine)
            {
                Debug.Log(cel.c + "," + cel.r);
            }
        }
    }

    private void SaveCurrLine(int col, int row)
    {
        if (row == 0)
        {
            for (int rowCount = 0; rowCount < count; rowCount++)
            {
                int sortCount = 1;
                foreach (var celNum in cellsNum)
                {
                    if (celNum.r == rowCount)
                    {
                        celNum.rowCount = sortCount;
                        sortCount++;
                    }
                }
            }
        }
        else
        {
            for (int colCount = 0; colCount < count; colCount++)
            {
                int sortCount = 1;
                foreach (var celNum in cellsNum)
                {
                    if (celNum.c == colCount)
                    {
                        celNum.colCount = sortCount;
                        sortCount++;
                    }
                }
            }
        }
    }

    private void PlayGameStart()
    {
        endPanel.SetActive(false);
        playPanel.SetActive(true);

        //RESET
        deleteCellsPanel();
        deleteCeelsNumPanel();
        cellsNum.Clear();   //리스트.Clear() ;
                            //

        SetGridMap(count);
        SetCells(count);
        DrawRandomCells(count, firstSettingLimitNum);
    }

    private void deleteCellsPanel()
    {
        RectTransform[] celsPanel = cellsPanel.GetComponentsInChildren<RectTransform>();
        for (int i = 1; i < celsPanel.Length; i++)
        {
            Destroy(celsPanel[i].gameObject);
        }
    }

    private void deleteCeelsNumPanel()
    {
        RectTransform[] celsNumPanel = cellsNumPanel.GetComponentsInChildren<RectTransform>();
        for (int i = 1; i < celsNumPanel.Length; i++)
        {
            Destroy(celsNumPanel[i].gameObject);
        }
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
                DrawCells(cellPrefab, cellsPanel, c, r, myCellSize, string.Format("Cell ({0}, {1})", c, r));
            }
        }
    }

    private void DrawCells(GameObject cellPrefab, GameObject CellsPanel, int c, int r, float cellSize, string cellname)
    {
        GameObject cel = Instantiate(cellPrefab, CellsPanel.transform);
        cel.GetComponent<RectTransform>().localPosition = PointToVector3(c, r);
        cel.name = cellname;
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
                DrawCellNum(cellNumPrefab, cellsNumPanel, col, row);
                limitCount++;
            }
        }
    }

    private bool IsEmpty(int col, int row)
    {
        foreach (CellNum cellNum in cellsNum)
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
        cellNum.name = string.Format("({0}, {1})", cellNum.c, cellNum.r);
        cellsNum.Add(cellNum);
    }

    private Vector3 PointToVector3(int col, int row)
    {
        return new Vector3(firstPos.x + col * myCellSize, firstPos.y + row * myCellSize, 0);
    }

    private CellNum GetCellNum(int col, int row)
    {
        foreach (CellNum cellNum in cellsNum)
        {
            if (cellNum.c == col && cellNum.r == row)
            {
                return cellNum;
            }
        }
        return null;
    }

    private void TestMovemove(int col, int row)
    {
        // GetCells(col, row);

        bool moveCellCheck = MoveCells(col, row);
        DrawOneCell(moveCellCheck);
    }

    private bool MoveCells(int col, int row)
    {
        int movingCheck = 0;
        foreach (var cell in cellsNum)
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