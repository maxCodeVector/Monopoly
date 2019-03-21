using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class how : MonoBehaviour {

	public int pageNum = 1;
	public UIAtlas h;
	public UISprite s;

	public void button(){
		if(pageNum <= 2){
			s.spriteName = "page" + (pageNum+1);
			pageNum ++;
		}else{
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		h = Resources.Load("Textures/UI/how", typeof(UIAtlas)) as UIAtlas;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
