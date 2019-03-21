using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour {

	public UISprite img;
    public float showTime = 2;
    public float timePast = 0;
	public float fadeTime = 3;
    void Start(){
		img = gameObject.GetComponent<UISprite>();
    }
    // Update is called once per frame
    void Update(){
        timePast += Time.deltaTime;       
        if (timePast > showTime){
			img.alpha -= (Time.deltaTime / fadeTime);
		}

		if(timePast > showTime + fadeTime){
			Destroy(gameObject);
		}
	}
}       

