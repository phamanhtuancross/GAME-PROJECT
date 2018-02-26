using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>{

	public int GetHighestScore()
    {
        return PlayerPrefs.GetInt("HighestScore");
    }

    public void SubmitNewScore(int _score)
    {
        //if the submit score greater than HighestScore
        if (_score > GetHighestScore())
            PlayerPrefs.SetInt("HighestScore", _score); //set HighestScore
        
    }
}
