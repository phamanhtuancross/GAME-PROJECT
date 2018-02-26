using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyScale : MonoBehaviour {
	void Start () {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector3 tempScale = transform.localScale;

        float height = sr.bounds.size.y;
        float width = sr.bounds.size.x;

        float mapHeight = Camera.main.orthographicSize * 2f;
        float mapWidth = mapHeight * Screen.width / Screen.height;

        tempScale.y = mapHeight / height;
        tempScale.x = mapWidth / width;

        transform.localScale = tempScale;
    }
}
