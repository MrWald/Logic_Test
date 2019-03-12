using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DotController : MonoBehaviour 
{
    private float minY;
    
    private float maxY = 4.4f;

	// Use this for initialization
	void Start () 
    {
		minY = transform.position.y;
	}

	// Update is called once per frame
	void Update () 
    {
	    if (Input.touchCount > 0 && approximate(transform.position, Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position)))
        {
            print("ther is a touch");
            StartCoroutine(moveTouch(0.01f));
        }
        if(Input.GetMouseButtonDown(0) && approximate(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            print("ther is a click");
            StartCoroutine(moveClick(0.01f));
        }
	}

    bool approximate(Vector2 pos1, Vector2 pos2)
    {
        return Math.Abs(pos1.x-pos2.x)<=0.1 && Math.Abs(pos1.y-pos2.y)<=0.1;
    }

    IEnumerator moveClick(float waitTime)
    {
        while(Input.GetMouseButton(0))
        {
            float y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            if(y>=minY && y<=maxY)
                transform.position = new Vector2(transform.position.x, y);
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator moveTouch(float waitTime)
    {
        while(Input.touchCount > 0)
        {
            float y = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y;
            if(y>=minY && y<=maxY)
                transform.position = new Vector2(transform.position.x, y);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
