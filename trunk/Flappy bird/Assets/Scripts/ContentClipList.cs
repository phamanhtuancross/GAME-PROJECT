using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentClipList : MonoBehaviour {

    public GameObject Content;
    public ClipPool clipPool;
    public Color LastRecordColor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadClip()
    {
        RemoveClip();
        AddClip();
    }

    private void RemoveClip()
    {
        while(Content.transform.childCount > 0)
        {
            GameObject toRemove = Content.transform.GetChild(0).gameObject;
            toRemove.transform.GetChild(3).GetComponent<Image>().color = Color.clear;
            clipPool.ReturnObject(toRemove);

        }
    }

    private void AddClip()
    {
        RecordData[] records = GameData.Instance.GetAllRecords();
        int clipNo = 0;

        for (int i = 0; i < records.Length; i++)
        {

            clipNo++;
            GameObject newClip = clipPool.GetObject();
            newClip.transform.SetParent(Content.transform);
            newClip.transform.localScale = new Vector3(1, 1, 1);
            newClip.GetComponent<ClipItem>().SetUp(records[i], clipNo);
        }

        //highlight last record
        GameObject lastOne = Content.transform.GetChild(0).gameObject;
        for(int i = 1; i < Content.transform.childCount; i++)
        {
            if (lastOne.GetComponent<ClipItem>().Record.RecordTime < Content.transform.GetChild(i).gameObject.GetComponent<ClipItem>().Record.RecordTime)
            {
                lastOne = Content.transform.GetChild(i).gameObject;
            }
        }
        if(lastOne != null)
        {
            lastOne.transform.GetChild(3).GetComponent<Image>().color = LastRecordColor;
        }
    }
}
