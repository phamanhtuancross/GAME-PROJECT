using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelData : MonoBehaviour {

    private int level;

    public int Level
    {
        set
        {
            level = value;
            LevelText.text = level.ToString();
            LevelText.gameObject.SetActive(true);
        }
    }

    public Text LevelText;
    public GameObject LockImage;
    public GameObject CheckImage;

    public void Lock()
    {
        LockImage.SetActive(true);
        CheckImage.SetActive(false);
        LevelText.gameObject.SetActive(false);
    }

    public void Unlock(int level)
    {
        LockImage.SetActive(false);
        CheckImage.SetActive(false);
        Level = level;
    }

    public void Complete(int level)
    {
        LockImage.SetActive(false);
        CheckImage.SetActive(true);
        Level = level;
    }
}
