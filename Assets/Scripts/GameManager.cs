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
        MovetoDir(dir);
    }

    public void OnU_Button()
    {
        dir = DIRECTION.UP;
        MovetoDir(dir);
    }

    public void OnD_Button()
    {
        dir = DIRECTION.DOWN;
        MovetoDir(dir);
    }

    private void MovetoDir(DIRECTION dir)
    {
        DrawOneCell(GetCellsDirLine(dir));
    }

    private bool GetCellsDirLine(DIRECTION dir)
    {
        bool checkMove = false;
        switch (dir)
        {
            case DIRECTION.UP:
                checkMove = MoveMoveCells(dir, 0, 1, count - 1);
                break;

            case DIRECTION.DOWN:
                checkMove = MoveMoveCells(dir, 0, -1, 0);
                break;

            case DIRECTION.RIGHT:
                checkMove = MoveMoveCells(dir, 1, 0, count - 1);
                break;

            case DIRECTION.LIGHT:
                checkMove = MoveMoveCells(dir, -1, 0, 0);
                break;

            case DIRECTION.COUNT:
                break;

            default:
                break;
        }

        return checkMove;
    }

    private bool MoveMoveCells(DIRECTION dir, int col, int row, int startPoint)
    {
        bool checkMove = false;

        for (int i = 0; i < count; i++)
        {
            List<CellNum> celLine = new List<CellNum>();

            GetJustOneLineList(celLine, i, row);

            //줄 내 리스트 정렬! > " 계산하고 " >  이동!
            int movePoint = startPoint;    //초기화*************************************
            switch (dir)
            {
                case DIRECTION.UP:
                    celLine.Sort((a, b) => b.r.CompareTo(a.r));
                    foreach (var cel in celLine)
                    {
                        checkMove = MovingCell(checkMove, cel, cel.c, movePoint);
                        Debug.Log(checkMove);
                        movePoint--;
                    }
                    break;

                case DIRECTION.DOWN:
                    celLine.Sort((a, b) => a.r.CompareTo(b.r));
                    foreach (var cel in celLine)
                    {
                        checkMove = MovingCell(checkMove, cel, cel.c, movePoint);
                        Debug.Log(checkMove);
                        movePoint++;
                    }
                    break;

                case DIRECTION.RIGHT:
                    celLine.Sort((a, b) => b.c.CompareTo(a.c));
                    foreach (var cel in celLine)
                    {
                        checkMove = MovingCell(checkMove, cel, movePoint, cel.r);
                        Debug.Log(checkMove);
                        movePoint--;
                    }

                    break;

                case DIRECTION.LIGHT:
                    celLine.Sort((a, b) => a.c.CompareTo(b.c));
                    foreach (var cel in celLine)
                    {
                        checkMove = MovingCell(checkMove, cel, movePoint, cel.r);
                        Debug.Log(checkMove);
                        movePoint++;
                    }

                    break;

                case DIRECTION.COUNT:
                    break;

                default:
                    break;
            }
        }

        return checkMove;
    }

    private void GetJustOneLineList(List<CellNum> cellLine, int i, int row)
    {
        // 행마다 리스트뽑기
        foreach (var cel in cellsNum)
        {
            if (row == 0)
            {
                if (i == cel.r)
                {
                    var cellinline = GetCellNum(cel.c, cel.r);
                    cellLine.Add(cellinline);
                }
            }
            else
            {
                if (i == cel.c)
                {
                    var cellinline = GetCellNum(cel.c, cel.r);
                    cellLine.Add(cellinline);
                }
            }
        }
    }

    private void MergeCells(List<CellNum> cellNum)
    {
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

    private bool MovingCell(bool checkMove, CellNum cell, int col, int row)
    {
        Debug.Log(string.Format("현재 : {0},{1} > 변경 : {2},{3}", cell.c, cell.r, col, row));
        if (cell.c == col && cell.r == row)
        {
            checkMove = false;
            return checkMove;
        }

        cell.c = col;
        cell.r = row;

        cell.GetComponent<RectTransform>().localPosition = PointToVector3(col, row);
        checkMove = true;
        return checkMove;
    }

    private Vector3 PointToVector3(int col, int row)
    {
        return new Vector3(firstPos.x + col * myCellSize, firstPos.y + row * myCellSize, 0);
    }

    private void DrawOneCell(bool isMove)
    {
        if (!isMove)
            return;
        else if (isMove)
        {
            DrawRandomCells(count, 1);
        }
    }
}