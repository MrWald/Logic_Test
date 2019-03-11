using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0){
         print("ther is a touch");
         
             if(Input.GetTouch(0).phase == TouchPhase.Began){
                 print("Touch has Began");
             }
             if(Input.GetTouch(0).phase == TouchPhase.Ended){
                 print("Touch has Ended");
             }
      }
	}
}
