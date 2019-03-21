using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
	public string ipInfo;
	public string id;
	public int role;
	public string nickName;
	public bool isClient;
	// public float speed = 20;
	// bool walk = false; //用此属性来决定是否走路,投骰子时启用，走完关闭
	// int stepCount;
	// Vector3 dir;
	// public Animator ani;
	// public MapCell standingCell;
	// public MapCell nextMapCell = null;
	// public Dice dice;
	// private bool pause = false;
	// private List<GameObject> arrowheads = new List<GameObject>();
	// Use this for initialization

	public Player(string ipInfo, string id, int role, string nickName){
		this.ipInfo = ipInfo;
		this.id = id;
		this.role = role;
		this.nickName = nickName;
	}
	// public void diced(){
	// 	startWalk(dice.diceNum);
	// }
	// public void startWalk(int count){
	// 	walk = true;
	// 	ani.SetBool("walk",true);
	// 	stepCount = count;
	// }

	// public void chooseNextCell(){
	// 	// todo
	// 	if(pause == false){
	// 		foreach (GameObject cell in standingCell.nextCell){
	// 			GameObject arrowhead = Resources.Load("prefabs/arrowhead") as GameObject;
	// 			Arrowhead ah = arrowhead.GetComponent<Arrowhead>();
	// 			ah.player = this;
	// 			ah.target = cell.GetComponent<MapCell>();
	// 			GameObject a = Instantiate(arrowhead);
	// 			Vector3 d = cell.transform.position - transform.position;
	// 			a.transform.rotation = Quaternion.LookRotation(new Vector3(0, -1, 0),d);
	// 			arrowheads.Add(a);
	// 		}
	// 		pause = true;
	// 	}
	// }

	// public void destroyArrows(){
	// 	foreach (GameObject arrow in arrowheads){
	// 		Destroy(arrow);
	// 		}
	// }
	// void Start () {
	// 	transform.position = standingCell.transform.position;
	// 	NetManager.getInstance().SendMsg("");
	// 	NetManager.getInstance().RegistProtocoal("MOV",delegate(ProtocolBase p){
	// 		string[] desc = p.GetDesc().Split(',');
	// 		if(int.Parse(desc[1])==1)
	// 			return;
	// 		int move = int.Parse(desc[2]);
	// 			startWalk(move);
	// 		});
		
	// }
	
	// // Update is called once per frame
	// void Update () {
	// 	if(walk && stepCount > 0){
	// 		float step = speed * Time.deltaTime;
			
	// 		// 首先确定下一个cell，然后开始走。
	// 		if(nextMapCell == null){
	// 			if(standingCell.nextCell.Count > 1){
	// 				chooseNextCell();
	// 			}else
	// 				nextMapCell = standingCell.nextCell[0].GetComponent<MapCell>();
	// 			//调整人物朝向
	// 			if(nextMapCell != null){
	// 				pause = false;
	// 				dir = nextMapCell.transform.position - transform.position;
	// 				transform.rotation = Quaternion.LookRotation(dir);
	// 			}
	// 		}

	// 		// 开始前进
	// 		if(nextMapCell != null && (transform.position = Vector3.MoveTowards(
	// 			transform.position, nextMapCell.transform.position, step)) == nextMapCell.transform.position){
	// 			stepCount--;
	// 			standingCell = nextMapCell;
	// 			nextMapCell = null;
	// 			if(stepCount == 0){
	// 				walk = false;
	// 				dice.enableDice();
	// 				ani.SetBool("walk",false);
	// 				standingCell.triggerEvents();
	// 			}
	// 		}
	// 	}
	// }
}
