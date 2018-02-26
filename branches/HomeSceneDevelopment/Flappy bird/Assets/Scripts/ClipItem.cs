using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClipItem : MonoBehaviour {

    public Text NoText, DateText, ScoreText;

    private RecordData _Record;

    public RecordData Record
    {
        get
        {
            return _Record;
        }

        set
        {
        }
    }

    public void SetUp(RecordData record, int no)
    {
        NoText.text = no + "";
        DateText.text = DateTime.FromFileTimeUtc(record.RecordTime).ToString();
        ScoreText.text = record.Score + "";
        _Record = record;
    }

    public void Onclick()
    {
        UIManager.Instance.StartButton();
        UIManager.Instance.ShowInstruction(false);
        RecordPlayer.Instance.StartPlayingRecord(_Record);
    }
}
