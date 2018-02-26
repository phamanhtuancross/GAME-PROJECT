using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ParallaxLayer : MonoBehaviour
{
    public GameObject _ParallaxItemPrefab;
    public int _Count;
    public Vector3 _offset;

    public void CreateItems()
    {
        float itemWidth = _ParallaxItemPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float itemY = _ParallaxItemPrefab.transform.position.y;
        Vector3 itemPos = new Vector3(-((float)_Count / 2) * itemWidth, itemY, 0f);

        for (int i = 0; i < _Count; i++)
        {
            itemPos.x += itemWidth;
            Debug.Log(itemPos);
            GameObject newItem = GameObject.Instantiate(
                _ParallaxItemPrefab, itemPos,
                _ParallaxItemPrefab.transform.rotation,
                this.transform);
            newItem.GetComponent<ParallaxItem>().StartPos = itemPos;
        }
    }
}
