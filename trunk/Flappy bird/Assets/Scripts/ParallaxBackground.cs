using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : Singleton<ParallaxBackground> {
    public ParallaxLayer[] _layers;

    private bool _isRunning;

    // Use this for initialization
    void Start () {
        _isRunning = true;

        foreach(ParallaxLayer layer in _layers)
        {
            layer.CreateItems();
        }
	}

    public void setRunningState(bool enabled)
    {
        foreach (ParallaxLayer layer in _layers)
        {
            for (int i = 0; i < layer.transform.childCount; i++)
            {
                layer.transform.GetChild(i).gameObject.GetComponent<ParallaxItem>().enabled = enabled;
            }
        }
    }
}
