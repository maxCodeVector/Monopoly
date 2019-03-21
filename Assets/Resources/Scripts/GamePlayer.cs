using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer : MonoBehaviour {
	public List<CardItems> cards = new List<CardItems>();
	public string ipInfo;
	public string id;
	public int role;
	public string nickName;
	public bool isClient;
	private bool moveCoor;
	private Vector3 targetCoor;
	public float speed = 20;
	public float efficientRatio = 1;
	public int wealth = 5000;
	public int deposit = 0;
	public int credit = 0;
	public int energy = 90;
	public int intell = 30;
	public int health = 80;
	public double GPA = 0;
	public bool isSick = false;
	public bool inDark = false;
	public bool firstMove = true;
	public bool newNextPosition = false;
	// public int luck = 0; // luck 上限为10


// **********以下为client专用属性（方法）！***********
	bool walk = false; //用此属性来决定是否走路,投骰子时启用，走完关闭
	int stepCount;
	Vector3 dir;
	public MapCell standingCell;
	public MapCell nextMapCell = null;
	public Vector3 nextPostion;
	public Dice dice;
	private bool pause = false;
	private List<GameObject> arrowheads = new List<GameObject>();

	public void diced(){ // 处理回合数逻辑
		deposit = (int) 1.1 * deposit;
		GameController.checkAndNotify();
		startWalk(dice.diceNum);
	}
	public void startWalk(int count){
		walk = true;
		// ani.SetBool("walk",true);
		stepCount = count;
	}

	public void chooseNextCell(){
		// todo
		if(pause == false){
			foreach (GameObject cell in standingCell.nextCell){
				GameObject arrowhead = Resources.Load("prefabs/arrowhead") as GameObject;
				Arrowhead ah = arrowhead.GetComponent<Arrowhead>();
				ah.player = this;
				ah.target = cell.GetComponent<MapCell>();
				GameObject a = Instantiate(arrowhead);
				Vector3 d = cell.transform.position - standingCell.transform.position;
				a.transform.rotation = Quaternion.LookRotation(new Vector3(0, -1, 0),d);
				arrowheads.Add(a);
			}
			pause = true;
		}
	}

	public void destroyArrows(){
		foreach (GameObject arrow in arrowheads){
			Destroy(arrow);
		}
	}
// **********以上为client专用属性（方法）！***********

	public void moveToCoor(float x, float z){
		moveCoor = true;
		firstMove = true;
		targetCoor = new Vector3(x, transform.position.y, z);
	}

	public void Finished(){
		GlobalControl.sendFinishedMessage(id);
		dice.disableDice();
	}

	public void sendForwardMessage(float x, float z){
		GlobalControl.sendForwardMessage(id, x, z);
	}
	
	public void sendTeleportMessage(float x, float z){
		GlobalControl.sendTeleportMessage(id,x,z);
	}

	public void stopMoveCoor(){
		moveCoor = false;
	}
	void Start () {
		GameController.buyCards(1,0);
	}
	
	// Update is called once per frame
	void Update () {
		if(moveCoor == true){
			if(firstMove){
				dir = targetCoor - transform.position;
				transform.rotation = Quaternion.LookRotation(-dir);
				firstMove = false;
			}
			int count = 0; // the nunmber of players that stand on the nextMapCell.
			foreach(GamePlayer player in GameController.gamePlayers){
				if(player.transform.position.x<targetCoor.x+4
					&& player.transform.position.x>targetCoor.x-4
					&& player.transform.position.z<targetCoor.z+4
					&& player.transform.position.z>targetCoor.z-4
					&& player.id.Equals(id) == false){
					count += 1;
				}
			}
			if(count == 0){
				nextPostion = targetCoor;
			}else if(count == 1){
				nextPostion = targetCoor + 
					Quaternion.Euler(0,90,0)*dir.normalized*2;
			}else if(count == 2){
				nextPostion = targetCoor + 
					Quaternion.Euler(0,-90,0)*dir.normalized*2;
			}
			float step = speed * Time.deltaTime;
			if((transform.position = Vector3.MoveTowards(
			transform.position, nextPostion, step)) == nextPostion){
				moveCoor = false;
				firstMove = true;
			}
		}

		// **********以下为client专用属性（方法）！***********
		if(walk && stepCount > 0){
			float step = speed * Time.deltaTime;
			
			// 首先确定下一个cell，然后开始走。
			if(nextMapCell == null){
				if(standingCell.nextCell.Count > 1){
					chooseNextCell();
				}else
					nextMapCell = standingCell.nextCell[0].GetComponent<MapCell>();
				//调整人物朝向
			}

			if(nextMapCell != null && !newNextPosition){
				pause = false;
				dir = nextMapCell.transform.position - transform.position;
				transform.rotation = Quaternion.LookRotation(-dir);
				int count = 0; // the nunmber of players that stand on the nextMapCell.
				foreach(GamePlayer player in GameController.gamePlayers){
					if(player.transform.position.x<nextMapCell.transform.position.x+4
						&& player.transform.position.x>nextMapCell.transform.position.x-4
						&& player.transform.position.z<nextMapCell.transform.position.z+4
						&& player.transform.position.z>nextMapCell.transform.position.z-4
						&& player.id.Equals(id) == false){
						count += 1;
					}
				}
				if(count == 0){
					nextPostion = nextMapCell.transform.position;
				}else if(count == 1){
					nextPostion = nextMapCell.transform.position + 
						Quaternion.Euler(0,90,0)*dir.normalized*2;
				}else if(count == 2){
					nextPostion = nextMapCell.transform.position + 
						Quaternion.Euler(0,-90,0)*dir.normalized*2;
				}
				newNextPosition = true;
			}

			// 开始前进
			if(nextMapCell != null && newNextPosition && (transform.position = Vector3.MoveTowards(
				transform.position, nextPostion, step)) == nextPostion){
				newNextPosition = false;	
				stepCount--;
				standingCell = nextMapCell;
				nextMapCell = null;
				sendForwardMessage(transform.position.x, transform.position.z);
				if(stepCount == 0){
					walk = false;
					dice.disableDice();
					// ani.SetBool("walk",false);
					if(standingCell.bomb != null){
						Destroy(standingCell.bomb);
						standingCell.bomb = null;
						GlobalControl.noBomb(standingCell.gameObject.name);
						GameController.sendHospital();
						GameController.setInfor("You stand on a bomb!!\nYou are sent to hospital!");
						Finished();
						return;
					}
					standingCell.setPlayer(this); 
					standingCell.triggerEvents();
				}
			}
		}
	}
}
