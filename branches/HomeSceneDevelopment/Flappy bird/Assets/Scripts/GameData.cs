using UnityEngine;
//using UnityEditor;
using System.Collections.Generic;

public class GameData : Singleton<GameData>
{
    private AllRecordData AllRecords;

    public void LoadRecords()
    {
        AllRecords = JsonUtility.FromJson<AllRecordData>(PlayerPrefs.GetString("Records"));
    }

    public void AddRecord(RecordData newRecord)
    {
        List<RecordData> newRecords = new List<RecordData>();
        if (AllRecords != null)
        {
            if (AllRecords.Records != null)
                newRecords.AddRange(AllRecords.Records);
        }
        else
            AllRecords = new AllRecordData();

        newRecords.Add(newRecord);

        //keep only record in top 10 score
        //eleminate others
        if (newRecords.Count >= 10)
        {
            int numberEliminate = newRecords.Count - 10;

            //sap xep
            for(int i = 0; i < newRecords.Count - 1; i++)
            {
                for (int j = 0; j < newRecords.Count - i - 1; j++)
                {
                    if (newRecords[j].Score < newRecords[j + 1].Score)
                    {
                        Debug.Log(i + "\t" + j);
                        //swap
                        RecordData tmp = newRecords[j];
                        newRecords[j] = newRecords[j + 1];
                        newRecords[j + 1] = tmp;
                    }
                }
            }

            //delete smallest
            newRecords.RemoveRange(10, numberEliminate);
        }

        AllRecords.Records = newRecords.ToArray();
        SaveRecords();
    }

    private void SaveRecords()
    {
        string result = JsonUtility.ToJson(AllRecords);
        PlayerPrefs.SetString("Records", result);
    }

    public RecordData[] GetAllRecords()
    {
        if (AllRecords == null)
        {
            LoadRecords();
        }
        return AllRecords.Records;
    }
}