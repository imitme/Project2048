using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellNum : MonoBehaviour
{
    public int c = 0;   //행
    public int r = 0;   //열

    private int num;
    private Text txt;

    public int Num
    {
        get { return num; }
        set
        {
            num = value;
            txt.text = value.ToString();
        }
    }

    private void Awake()
    {
        txt = GetComponentInChildren<Text>();
        Num = 2;
    }
}