using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBus : MonoBehaviour {
	public UIPopupList list;
	private GamePlayer player;
	private GameObject father;

	public void setPlayer(GamePlayer player1){
		player = player1;
	}

	public void GO(){
		switch (list.items.IndexOf(list.value))
		{
			case 0:
				MapCell mc = GameObject.Find("MapCell/cell (71)").GetComponent<MapCell>();
				player.standingCell = mc;
				player.nextMapCell = null;
				player.transform.position = new Vector3(-67, 5, -89);
				GlobalControl.sendTeleportMessage(player.id,-67,-89);
				player.Finished();
				Destroy(gameObject);
				break;
			case 1:
				MapCell mc1 = GameObject.Find("MapCell/cell (78)").GetComponent<MapCell>();
				player.standingCell = mc1;
				player.nextMapCell = null;
				player.transform.position = new Vector3(-13, 5, -58);
				GlobalControl.sendTeleportMessage(player.id,-13,-58);
				// player.sendForwardMessage(-13,-58);
				player.Finished();
				Destroy(gameObject);
				break;
			case 2:
				MapCell mc2 = GameObject.Find("MapCell/cell (32)").GetComponent<MapCell>();
				player.standingCell = mc2;
				player.nextMapCell = null;
				player.transform.position = new Vector3(-61, 5, -130);
				GlobalControl.sendTeleportMessage(player.id,-61,-130);
				// player.sendForwardMessage(-61,-130);
				player.Finished();
				Destroy(gameObject);
				break;
			case 3:
				MapCell mc3 = GameObject.Find("MapCell/cell (6)").GetComponent<MapCell>();
				player.standingCell = mc3;
				player.nextMapCell = null;
				player.transform.position = new Vector3(-61, 5, -172);
				GlobalControl.sendTeleportMessage(player.id,-61,-172);
				// player.sendForwardMessage(-61,-172);
				player.Finished();
				Destroy(gameObject);
				break;
			case 4:
				MapCell mc4 = GameObject.Find("MapCell/cell (18)").GetComponent<MapCell>();
				player.standingCell = mc4;
				player.nextMapCell = null;
				player.transform.position = new Vector3(-12, 5, -160);
				GlobalControl.sendTeleportMessage(player.id,-12,-160);
				// player.sendForwardMessage(-12,-160);
				player.Finished();
				Destroy(gameObject);
				break;
		}
	}

	public void leave(){
		player.Finished();
		Destroy(gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
