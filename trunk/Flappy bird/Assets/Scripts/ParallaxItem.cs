using UnityEngine;
using System.Collections;

[System.Serializable]
public class ParallaxItem : MonoBehaviour
{
    public float _speed;

    private float _itemWidth;
    private Vector3 _startPos;

    [HideInInspector]
    public float ItemWidth
    {
        get
        {
            return _itemWidth;
        }

        set
        {
        }
    }

    public Vector3 StartPos
    {
        get
        {
            return _startPos;
        }

        set
        {
            _startPos = value;
        }
    }

    // Use this for initialization
    void Awake()
    {
        StartPos = transform.position;
        _itemWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * _speed, ItemWidth);
        transform.position = StartPos + Vector3.left * newPosition;
    }
}
