using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrowhead : MonoBehaviour {
	public GamePlayer player;
	public MapCell target;
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(target.transform.position.x, 
		target.transform.position.y + 2, target.transform.position.z);
	}

	void OnMouseDown(){
		// Debug.Log("鼠标按下了@！！");
		player.nextMapCell = target;
		// Time.timeScale = 1;
		player.destroyArrows();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
