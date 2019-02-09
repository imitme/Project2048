using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellNum : MonoBehaviour
{
    public int c = 0;   //열
    public int r = 0;   //행

    private int _num;
    private Text txt;

    private Animator cellNumAnim = null;

    public int num
    {
        get { return _num; }
        set
        {
            _num = value;
            //txt.text = value.ToString();
            txt.text = _num.ToString();
        }
    }

    private void Awake()
    {
        cellNumAnim = GetComponent<Animator>();
        txt = GetComponentInChildren<Text>();
        num = 2;
    }

    public void StartMergeAnim()
    {
        cellNumAnim.SetTrigger("Merge");
    }
}