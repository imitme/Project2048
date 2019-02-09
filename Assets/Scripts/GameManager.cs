using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DIRECTION
{
    UP = 0, DOWN, RIGHT, LEFT, COUNT
};

public class GameManager : MonoBehaviour
{
    //public InputField count;
    public int totalCount = 4;

    private DIRECTION dir;
    public int firstSettingLimitNum = 1;

    public float myCellSize;
    public List<CellNum> cellsNum;

    public GameObject cellsPanel;
    public GameObject cellsNumPanel;
    public GameObject cellPrefab;
    public GameObject cellNumPrefab;

    public GameObject lobbyPanel;
    public GameObject playPanel;
    public Text score_Text;

    public int _score = 0;

    public Animator inGameCanvasAnim;
    public Animator lobbyCanvasAnim;
    public Animator playButtonAnim;

    private int score
    {
        get { return _score; }
        set { _score = value; score_Text.text = string.Format("Score : {0}", _score); }
    }

    private Vector3 firstPos = Vector3.zero;

    private void Start()
    {
        OnGotoMenu();
    }

    private void sendScoreNum(int addScore)
    {
        int currentScore = score;
        currentScore += addScore;
        score = currentScore;
    }

    private void OnGotoMenu()
    {
        StartCoroutine(GotoMenuAnim());
        OnResetPlayerInfo();
    }

    private IEnumerator GotoMenuAnim()
    {
        inGameCanvasAnim.SetTrigger("Stop");
        lobbyCanvasAnim.SetTrigger("Start");
        yield return new WaitForSeconds(0.5f);
        playPanel.SetActive(false);
        lobbyPanel.SetActive(true);

        lobbyCanvasAnim.SetTrigger("Start");
    }

    public void OnPlayButton()
    {
        StartCoroutine(GotoPlay());
    }

    private IEnumerator GotoPlay()
    {
        playButtonAnim.SetTrigger("Press");
        yield return new WaitForSeconds(0.3f);

        PlayGameStart();
    }

    public void OnGotoMenuButton()
    {
        OnGotoMenu();
    }

    private void OnResetPlayerInfo()
    {
        ResetScore();
    }

    private void ResetScore()
    {
        score = 0;
    }

    public void OnR_Button()
    {
        dir = DIRECTION.RIGHT;
        MovetoDir(dir);
    }

    public void OnL_Button()
    {
        dir = DIRECTION.LEFT;
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
        CheckEmpthOriginalList();
        bool isMove = GetCellsDirLine(dir);

        DrawOneCell(isMove);
    }

    private bool GetCellsDirLine(DIRECTION dir)
    {
        bool checkMove = false;
        int dirCol = 0;
        int dirRow = 0;
        int startPoint = 0;

        switch (dir)
        {
            case DIRECTION.UP:
                dirRow = 1;
                startPoint = totalCount - 1;

                break;

            case DIRECTION.DOWN:
                dirRow = -1;

                break;

            case DIRECTION.RIGHT:
                dirCol = 1;
                startPoint = totalCount - 1;
                //  TestMergeCells(dir, dirCol, dirRow, startPoint);
                //   checkMove = MoveCells(dir, 1, 0, totalCount - 1);
                break;

            case DIRECTION.LEFT:
                dirCol = -1;

                break;

            case DIRECTION.COUNT:
                break;

            default:
                break;
        }

        checkMove = MoveCells(dir, dirCol, dirRow, startPoint);

        return checkMove;
    }

    private bool MoveCells(DIRECTION dir, int dirCol, int dirRow, int startPoint)
    {
        bool checkMove = false;
        bool checkMerge = false;
        int checkMergeCount = 0;
        int checkMoveCount = 0;

        for (int lineCount = 0; lineCount < totalCount; lineCount++)
        {
            int movePoint = startPoint;
            List<CellNum> celLine = new List<CellNum>();

            GetJustOneLineList(celLine, lineCount, dirRow);

            switch (dir)
            {
                case DIRECTION.UP:
                    celLine.Sort((a, b) => b.r.CompareTo(a.r));
                    checkMerge = MergeCellNum(celLine);
                    if (checkMerge == true)
                        checkMergeCount++;
                    checkMove = UpMove(celLine, checkMove, movePoint);
                    if (checkMove == true)
                        checkMoveCount++;
                    break;

                case DIRECTION.DOWN:
                    celLine.Sort((a, b) => a.r.CompareTo(b.r));
                    checkMerge = MergeCellNum(celLine);
                    if (checkMerge == true)
                        checkMergeCount++;
                    checkMove = DownMove(celLine, checkMove, movePoint);
                    if (checkMove == true)
                        checkMoveCount++;
                    break;

                case DIRECTION.RIGHT:
                    celLine.Sort((a, b) => b.c.CompareTo(a.c));
                    checkMerge = MergeCellNum(celLine);
                    if (checkMerge == true)
                        checkMergeCount++;
                    checkMove = RightMove(celLine, checkMove, movePoint);
                    if (checkMove == true)
                        checkMoveCount++;
                    break;

                case DIRECTION.LEFT:
                    celLine.Sort((a, b) => a.c.CompareTo(b.c));
                    checkMerge = MergeCellNum(celLine);
                    if (checkMerge == true)
                        checkMergeCount++;
                    checkMove = LeftMove(celLine, checkMove, movePoint);
                    if (checkMove == true)
                        checkMoveCount++;
                    break;

                case DIRECTION.COUNT:
                    break;

                default:
                    break;
            }
        }

        if (checkMergeCount > 0)
        {
            checkMerge = true;
        }
        if (checkMoveCount > 0)
        {
            checkMove = true;
        }

        Debug.Log("합쳐졌니? " + checkMerge + "  움직였니? " + checkMove);
        return checkMerge || checkMove;
    }

    private bool MergeCellNum(List<CellNum> celLine)
    {
        bool checkMove = false;
        ///정렬된 celLine에 있는 것의 숫자를 비교해!
        for (int cellPoint = 0; cellPoint < celLine.Count; cellPoint++)
        {
            int currentCell = cellPoint;
            int nextCell = cellPoint + 1;

            if (nextCell >= celLine.Count)
                break;

            if (celLine[currentCell].num == celLine[nextCell].num)
            {
                ///점수 보내주고
                sendScoreNum(celLine[currentCell].num);

                ///합쳐주고
                int mergeNum = celLine[currentCell].num;
                mergeNum += mergeNum;
                celLine[currentCell].num = mergeNum;

                ///i+1 없앤다
                DestroyImmediate(celLine[nextCell].gameObject);
                celLine.RemoveAt(nextCell);

                //움직임체크!
                checkMove = true;
            }
        }

        CheckEmpthOriginalList();

        return checkMove;
    }

    private bool UpMove(List<CellNum> celLine, bool checkMove, int movePoint)
    {
        foreach (var cel in celLine)
        {
            if (cel == null)
                continue;

            checkMove = SetMovingPointofCell(checkMove, cel, cel.c, movePoint);
            movePoint--;
        }

        return checkMove;
    }

    private bool DownMove(List<CellNum> celLine, bool checkMove, int movePoint)
    {
        foreach (var cel in celLine)
        {
            if (cel == null)
                continue;

            checkMove = SetMovingPointofCell(checkMove, cel, cel.c, movePoint);
            movePoint++;
        }
        return checkMove;
    }

    private bool RightMove(List<CellNum> celLine, bool checkMove, int movePoint)
    {
        foreach (var cel in celLine)
        {
            if (cel == null)
                continue;

            checkMove = SetMovingPointofCell(checkMove, cel, movePoint, cel.r);
            movePoint--;
        }
        return checkMove;
    }

    private bool LeftMove(List<CellNum> celLine, bool checkMove, int movePoint)
    {
        foreach (var cel in celLine)
        {
            if (cel == null)
                continue;

            checkMove = SetMovingPointofCell(checkMove, cel, movePoint, cel.r);
            movePoint++;
        }
        return checkMove;
    }

    private void GetJustOneLineList(List<CellNum> cellLine, int lineCount, int checkLineAsRow)
    {
        foreach (var cel in cellsNum)
        {
            if (checkLineAsRow == 0)    //행 단위로 줄 묶기 //좌우 버튼을 눌렀다는 뜻
            {
                if (lineCount == cel.r)    //행이 같은 애들 찾아
                {
                    var cellinline = GetCellNum(cel.c, cel.r);
                    cellLine.Add(cellinline);
                }
            }
            else    //열 단위로 줄 묶기 //위아래 버튼을 눌렀다는 뜻
            {
                if (lineCount == cel.c)     //열이 같은 애들 찾아.
                {
                    var cellinline = GetCellNum(cel.c, cel.r);
                    cellLine.Add(cellinline);
                }
            }
        }
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
        lobbyPanel.SetActive(false);
        playPanel.SetActive(true);

        inGameCanvasAnim.SetTrigger("Start");

        //RESET
        deleteCellsPanel();
        deleteCellsNumPanel();
        cellsNum.Clear();   //리스트.Clear() ;

        SetGridMap(totalCount);
        SetCells(totalCount);
        DrawRandomCells(totalCount, firstSettingLimitNum);
    }

    private void deleteCellsPanel()
    {
        RectTransform[] celsPanel = cellsPanel.GetComponentsInChildren<RectTransform>();
        for (int i = 1; i < celsPanel.Length; i++)
        {
            Destroy(celsPanel[i].gameObject);
        }
    }

    private void deleteCellsNumPanel()
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

    private bool SetMovingPointofCell(bool checkMove, CellNum cell, int col, int row)
    {
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

    private void CheckEmpthOriginalList()
    {
        cellsNum.RemoveAll(cn => cn == null);
    }

    private void DrawOneCell(bool isMove)
    {
        Debug.Log(isMove);
        if (!isMove)
            return;
        else if (isMove)
        {
            DrawRandomCells(totalCount, 1);
        }
    }
}