using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeHolder : MonoBehaviour {

    public static PipeHolder instance;
    public float speed;
	private float widthCollider;

    void Awake()
    {  
        widthCollider = _getWidthCollider();
        _MakeInstance();
    }
	
    private void _MakeInstance()
    {
        if (instance == null)
            instance = this;
    }
	// Update is called once per frame
	void Update () {
        _PipeMovement();
	}
	
	float _getWidthCollider()
    {
        BoxCollider2D collider = (BoxCollider2D)this.gameObject.GetComponent<BoxCollider2D>();
        float unitsPerPixel = 2 * Camera.main.orthographicSize / Screen.height;
        return collider.size.x / unitsPerPixel;
    }

    void _PipeMovement()
    {
        if (BirdController.Instance.IsAlive)
        {
            Vector3 temp = transform.position;
            temp.x -= speed * Time.deltaTime;
            transform.position = temp;
        }
		
		Vector3 temp2 = Camera.main.WorldToScreenPoint(transform.position);
        if (temp2.x + widthCollider/2  < 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Destroy")
        {
            Destroy(gameObject);
        }
    }
}
