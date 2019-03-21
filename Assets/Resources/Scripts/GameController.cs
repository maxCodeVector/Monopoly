using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public static List<GamePlayer> gamePlayers = new List<GamePlayer>();
	public static GamePlayer startPlayer;
	public static GamePlayer clientPlayer;
	public static GameObject uiRoot;
	public static Dice dice;

	public float showTime = 4;
    public float timePast = 0;
	public float fadeTime = 3;
	public static bool isGameOver;

// *************************************
	public static UILabel money;
	public static UILabel energy;
	public static UILabel health;
	public static UILabel intell;
	public static UILabel credits;
	public static UILabel deposit;
	public static UILabel time;
	public static UILabel messages;
	public static float changeTime = 3f;
	public static bool dynamicChange = false;
	public static float changePastTime = 0;
	public static int moneyChange;
	public static int energyChange;
	public static int healthChange;
	public static int intellChange;
	public static int creditsChange;
	public static float depositChange;
	public static float moneyI;
	public static float energyI;
	public static float healthI;
	public static float intellI;
	public static float creditsI;
	public static float depositI;
// *************************************
	public static void setStartPlayer(int startID){
		if(time != null){
			time.text = "20";
		}
		CamRotate.timePast = 0;
		GlobalControl.netMgr.timer.setTimer(20);
		foreach(GamePlayer gp in gamePlayers){
			if(int.Parse(gp.id) == startID){
				startPlayer = gp;
				if(startID == int.Parse(clientPlayer.id)){
					if(gp.isSick){
						GlobalControl.sendFinishedMessage(gp.id);
						gp.isSick = false;
						return;
					}
					if(gp.inDark){
						GlobalControl.sendFinishedMessage(gp.id);
						gp.inDark = false;
						return;
					}
					dice.enableDice();
				}else{
					dice.disableDice();
				}
				startWalk();
				return;
			}
		}
	}

	private static void startWalk(){
		if(int.Parse(startPlayer.id) != GlobalControl.clientID){
			// 等待服务器发送 Forward, [id], [x], [z]
		}else{
			// client 开始与UI的交互

		}
	}
	public static void checkAndNotify(){
		if(clientPlayer.wealth <= 0){
			messages.text = "You have ran out of money!\nYou are out!";
			messages.alpha = 1;
			GlobalControl.sendPoorFailed();
		}
		if(clientPlayer.health >= 100)
			clientPlayer.health = 100;
		if(clientPlayer.energy > 100)
			clientPlayer.energy = 100;
		if(clientPlayer.health <= 0){
			messages.text = "You have ran out of health!\nYou are sent to hospital!";
			messages.alpha = 1;
			sendHospital();
		}
		if(clientPlayer.energy <= 0){
			messages.text = "You have ran out of energy!\nYou are sent to hospital!";
			messages.alpha = 1;
			sendHospital();
		}
		dynamicChange = true;
		changePastTime = 0;
		moneyChange = clientPlayer.wealth - int.Parse(money.text.Remove(0,1));
		energyChange = clientPlayer.energy - int.Parse(energy.text);
		healthChange = clientPlayer.health - int.Parse(health.text);
		intellChange = clientPlayer.intell - int.Parse(intell.text);
		creditsChange = clientPlayer.credit - int.Parse(credits.text);
		depositChange = clientPlayer.deposit - int.Parse(deposit.text.Remove(0,1));
		moneyI = int.Parse(money.text.Remove(0,1));
		energyI = int.Parse(energy.text);
		healthI = int.Parse(health.text);
		intellI = int.Parse(intell.text);
		creditsI = int.Parse(credits.text);
		depositI = int.Parse(deposit.text.Remove(0,1));
	}

	public static void sendHospital(){
		MapCell mc1 = GameObject.Find("MapCell/cell (30)").GetComponent<MapCell>();
		clientPlayer.standingCell = mc1;
		clientPlayer.nextMapCell = null;
		clientPlayer.transform.position = 
			new Vector3(mc1.transform.position.x, 5, mc1.transform.position.z);
		// clientPlayer.sendForwardMessage(mc1.transform.position.x, mc1.transform.position.z);
		clientPlayer.sendTeleportMessage(mc1.transform.position.x, mc1.transform.position.z);
		clientPlayer.isSick = true;
		clientPlayer.health += 10;
		checkAndNotify();
	}

	public static void sendDark(){
		MapCell mc1 = GameObject.Find("MapCell/cell (50)").GetComponent<MapCell>();
		clientPlayer.standingCell = mc1;
		clientPlayer.nextMapCell = null;
		clientPlayer.transform.position = 
			new Vector3(mc1.transform.position.x, 5, mc1.transform.position.z);
		// clientPlayer.sendForwardMessage(mc1.transform.position.x, mc1.transform.position.z);
		clientPlayer.sendTeleportMessage(mc1.transform.position.x, mc1.transform.position.z);
		clientPlayer.inDark = true;
		clientPlayer.health += 10;
		checkAndNotify();
	}
	public static void buyCards(int id, int cost){
		if(clientPlayer.cards.Count > 3){
			GameObject uiRoot = GameObject.Find("UI Root");
			GameObject messageWindow =  Resources.Load("Prefabs/UI/InforWindow") as GameObject;
			messageWindow = NGUITools.AddChild(uiRoot, messageWindow);
			messageWindow.GetComponent<infoWindow>().setMessage("You have had three cards now, you can not buy more cards.");	
			return;
		}
		string prefabName = "Prefabs/Cards/";
		switch (id){
			case 1:
				prefabName += "diceCard";
				break;
			case 2:
				prefabName += "mineCard";
				break;
			case 3:
				prefabName += "exerciseCard";
				break;
			// case 4:
			// 	prefabName += "buyHouseCard";
			// 	break;
			// case 5:
			// 	prefabName += "efficiencyCard";
			// 	break;
			// case 6:
			// 	prefabName += "frameCard";
			// 	break;
		}

		GameObject card = Resources.Load(prefabName) as GameObject;
		clientPlayer.cards.Add(card.GetComponent<CardItems>());
		clientPlayer.wealth -= cost;
		checkAndNotify();
		showCards();
	}
	public static void showCards(){
		GameObject father = GameObject.Find("UI Root/GameMainUI/Anchor_Right/card_back");
		string slotName = "cardSlot";
		for(int i = 1; i <= 3; i++){
			Transform slot = father.transform.Find(slotName+i);
			foreach (Transform child in slot){
            	Destroy(child.gameObject);
         	}
			if(i <= clientPlayer.cards.Count){
				NGUITools.AddChild(slot.gameObject, clientPlayer.cards[i-1].getGameObject());	
			} else {
				GameObject emptyCard = Resources.Load("Prefabs/Cards/emptyCard") as GameObject;
				NGUITools.AddChild(slot.gameObject, emptyCard);
			}
		}
	}

	public static void enterAreas(string name){
		if(messages == null){
			messages = GameObject.Find("UI Root/GameMainUI/Anchor_Center/Messages").GetComponent<UILabel>();
		}
		switch (name){
			case "LakesideTeaching":
				messages.alpha = 1;
				messages.text = "Welcome to \nthe Lakeside Teaching Area!";
				break;
			case "TeachersApartment":
				messages.alpha = 1;
				messages.text = "Welcome to \nthe Teachers' Apartment Area!";
				break;
			case "JoyHighland":
				messages.alpha = 1;
				messages.text = "Welcome to \nthe Joy Highland!";
				break;
			case "InnovationPark":
				messages.alpha = 1;
				messages.text = "Welcome to \nthe Innovation Park!";
				break;
			case "LycheeHill":
				messages.alpha = 1;
				messages.text = "Welcome to \nthe Lychee Hill!";
				break;
			case "LakesideDormitory":
				messages.alpha = 1;
				messages.text = "Welcome to \nthe Lakeside Dormitory Area!";
				break;
		}
	}

	private void initLabels(){
		string father = "UI Root/GameMainUI/";
		money = GameObject.Find(father+"Anchor_Top/money/wealth").GetComponent<UILabel>();
		energy = GameObject.Find(father+"Anchor_Top/energy/energy").GetComponent<UILabel>();
		health = GameObject.Find(father+"Anchor_Left/Status/health").GetComponent<UILabel>();
		intell = GameObject.Find(father+"Anchor_Left/Status/INT").GetComponent<UILabel>();
		credits = GameObject.Find(father+"Anchor_Left/Status/credits").GetComponent<UILabel>();
		deposit = GameObject.Find(father+"Anchor_Left/Status/deposit").GetComponent<UILabel>();
		messages = GameObject.Find(father+"Anchor_Center/Messages").GetComponent<UILabel>();
		time = GameObject.Find(father+"Anchor_Top/timer/time").GetComponent<UILabel>();
		time.text = "15";
	}

	public static void setInfor(string content){
		messages.alpha = 1;
		messages.text = content;

	}

	// Use this for initialization
	void Start () {
		gamePlayers.Clear();
		uiRoot = GameObject.Find("UI Root");
		isGameOver = false;
		dice = GameObject.Find("UI Root/GameMainUI/Anchor_BR/Dice").GetComponent<Dice>();
		MapCell standingCell = GameObject.Find("MapCell/StartCell").GetComponent<MapCell>();
		GameObject gameController = GameObject.Find("GameController");
		float height = 0.4f;
		int Z = -180;
		foreach(Player player in GlobalControl.players){
			GameObject character = Resources.Load("Prefabs/Characters/C" + player.role) as GameObject;
			character = Instantiate(character, new Vector3(-61, height, Z), gameController.transform.rotation);
			GamePlayer gp = character.GetComponent<GamePlayer>();
			gp.ipInfo = player.ipInfo;
			gp.id = player.id;
			if(int.Parse(gp.id) == GlobalControl.startID){
				startPlayer = gp;
			}
			gp.role = player.role;
			gp.nickName = player.nickName;
			gp.isClient = player.isClient;
			gp.dice = dice;
			gp.standingCell = standingCell;
			// height += 5;
			Z -= 4;
			gamePlayers.Add(gp);
			if(int.Parse(gp.id) == GlobalControl.clientID){
				clientPlayer = gp;
			}
		}

		NetManager.getInstance().RegistProtocoal("FORWARD",delegate(ProtocolBase p){
			string[] desc = p.GetDesc().Split(',');
			int forwardID = int.Parse(desc[1]);
			// 如果是收到自己发过去的，则什么也不做。
			if(forwardID != GlobalControl.clientID){
				float x = float.Parse(desc[2]);
				float z = float.Parse(desc[3]);
				startPlayer.moveToCoor(x, z);
			}
		});

		NetManager.getInstance().RegistProtocoal("TELEPORT",delegate(ProtocolBase p){
			string[] desc = p.GetDesc().Split(',');
			int teleID = int.Parse(desc[1]);
			// 如果是收到自己发过去的，则什么也不做。
			if(teleID != GlobalControl.clientID){
				float x = float.Parse(desc[2]);
				float z = float.Parse(desc[3]);
				startPlayer.stopMoveCoor();
				startPlayer.transform.position = 
					new Vector3(x, startPlayer.transform.position.y, z);
			}
		});

		NetManager.getInstance().RegistProtocoal("BUYHOUSE",delegate(ProtocolBase p){
			string[] desc = p.GetDesc().Split(',');
			int ID = int.Parse(desc[1]);
			string cell = desc[2].Trim();
			if(ID != GlobalControl.clientID){
				MapCell mp = GameObject.Find("MapCell/" + cell).GetComponent<MapCell>();
				HouseController hc = (HouseController)mp.controller;
				foreach(GamePlayer player in gamePlayers){
					if(ID == int.Parse(player.id)){
						hc.owner = player;
						hc.level += 1;
						string hatname = string.Format("C{0}hat",player.role);
						GameObject hat = Resources.Load("Prefabs/Characters/"+hatname) as GameObject;
						hat = Instantiate(hat, new Vector3(mp.transform.position.x, 5, mp.transform.position.z)
						, player.transform.rotation);
						return;
					}
				}
			}
		});

		NetManager.getInstance().RegistProtocoal("UPGRADEHOUSE",delegate(ProtocolBase p){
			string[] desc = p.GetDesc().Split(',');
			int ID = int.Parse(desc[1]);
			string cell = desc[3].Trim();
			if(ID != GlobalControl.clientID){
				MapCell mp = GameObject.Find("MapCell/" + cell).GetComponent<MapCell>();
				HouseController hc = (HouseController)mp.controller;
				foreach(GamePlayer player in gamePlayers){
					if(ID == int.Parse(player.id)){
						hc.owner = player;
						hc.level += 1;
						string hatname = string.Format("C{0}hat",player.role);
						GameObject hat = Resources.Load("Prefabs/Characters/"+hatname) as GameObject;
						hat = Instantiate(hat, new Vector3(mp.transform.position.x, 5, mp.transform.position.z)
						, player.transform.rotation);
					}
				}
			}
		});

		NetManager.getInstance().RegistProtocoal("TOLL",delegate(ProtocolBase p){
			string[] desc = p.GetDesc().Split(',');
			int id1 = int.Parse(desc[1].Trim());
			int id2 = int.Parse(desc[2].Trim());
			int money = int.Parse(desc[3].Trim());
			if(id2 == int.Parse(clientPlayer.id)){
				clientPlayer.wealth += money;
				checkAndNotify();
			}
		});

		NetManager.getInstance().RegistProtocoal("SETBOMB",delegate(ProtocolBase p){
			string[] desc = p.GetDesc().Split(',');
			int id = int.Parse(desc[1]);
			string cellName = desc[2].Trim();
			if(id != int.Parse(clientPlayer.id)){
				MapCell mp = GameObject.Find("MapCell/"+cellName).GetComponent<MapCell>();
				GameObject bomb = Resources.Load("Prefabs/Characters/boom") as GameObject;
				bomb = Instantiate(bomb, new Vector3(mp.transform.position.x, 5,mp.transform.position.z), 
				mp.transform.rotation);
				mp.bomb = bomb;
				// GameController.clientPlayer.standingCell.bomb = bomb;
			}
			
		});

		NetManager.getInstance().RegistProtocoal("NOBOMB",delegate(ProtocolBase p){
			string[] desc = p.GetDesc().Split(',');
			string cellName = desc[1].Trim();
			MapCell mp = GameObject.Find("MapCell/"+cellName).GetComponent<MapCell>();
			if(mp.bomb != null)
				Destroy(mp.bomb);
			mp.bomb = null;
		});


		NetManager.getInstance().RegistProtocoal("GAMEOVER",delegate(ProtocolBase p){
			isGameOver = true;
			GameObject uiRoot = GameObject.Find("UI Root");
			GameObject messageBox = Resources.Load("prefabs/UI/gameoverWindow") as GameObject;
       	 	messageBox = NGUITools.AddChild(uiRoot, messageBox);
			// messageBox.transform.localScale *= 2f;
			string[] desc = p.GetDesc().Split(',');
			for(int i = 1; i<desc.Length;i+=5){
				foreach(GamePlayer player in gamePlayers){
					if(player.id.Equals(desc[i].Trim())){
						int rank = i/5 + 1;
						int index = (rank-1)*5+1;
						UILabel label= messageBox.transform.Find("table/"+index).GetComponent<UILabel>();
						label.text = player.nickName;
						label= messageBox.transform.Find("table/"+(index+1)).GetComponent<UILabel>();
						label.text = desc[i+1]; // score
						label= messageBox.transform.Find("table/"+(index+2)).GetComponent<UILabel>();
						label.text = desc[i+2]; // money
						label= messageBox.transform.Find("table/"+(index+3)).GetComponent<UILabel>();
						label.text = desc[i+3]; // health
						label= messageBox.transform.Find("table/"+(index+4)).GetComponent<UILabel>();
						label.text = desc[i+4]; // GPA
					}
				}
			}
			gamePlayers.Clear();
		});

		NetManager.getInstance().RegistProtocoal("SCHOLARSHIP",delegate(ProtocolBase p){
			GameObject uiRoot = GameObject.Find("UI Root");
			GameObject messageBox = Resources.Load("prefabs/UI/schoWindow") as GameObject;
       	 	messageBox = NGUITools.AddChild(uiRoot, messageBox);
			messageBox.transform.localScale *= 1.5f;
			string[] desc = p.GetDesc().Split(',');
			for(int i = 1; i<desc.Length;i+=2){
				foreach(GamePlayer player in gamePlayers){
					if(player.id.Equals(desc[i].Trim())){
						UILabel label= messageBox.transform.Find("table/"+i).GetComponent<UILabel>();
						label.text = player.nickName;
						label= messageBox.transform.Find("table/"+(i+1)).GetComponent<UILabel>();
						label.text = desc[i+1];
					}
				}
				if(desc[i].Trim().Equals(clientPlayer.id)){
					int money = int.Parse(desc[i+1].Trim());
					clientPlayer.wealth += money;
					GameController.checkAndNotify();
				}
			}
		});

		NetManager.getInstance().RegistProtocoal("QUERY",delegate(ProtocolBase p){
			string[] desc = p.GetDesc().Split(',');
			int o = int.Parse(desc[1]);
			double temp = (clientPlayer.credit + clientPlayer.intell*0.05)*4/(2.75+o*4);
			temp = System.Math.Round(temp,3);
			clientPlayer.GPA = Mathf.Min((float)temp, 4);
			NetManager.getInstance().SendMsg(string.Format("ANSWER,{0},{1},{2},{3},{4},{5}"
				,o,clientPlayer.id,clientPlayer.wealth,clientPlayer.health,clientPlayer.GPA,clientPlayer.credit));
		});
		
		initLabels();
		checkAndNotify();
		showCards();
		GlobalControl.music = GameObject.Find("WorldCenter/Main Camera/Audio Source").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(messages == null){
			messages = GameObject.Find("UI Root/GameMainUI/Anchor_Center/Messages").GetComponent<UILabel>();
		}
		if(!isGameOver && messages.alpha > 0){
			timePast += Time.deltaTime;       
			if (timePast > showTime){
				messages.alpha -= (Time.deltaTime / fadeTime);
			}
		}
		int leftTime = GlobalControl.netMgr.timer.getLeaveTime();
		time.text = leftTime.ToString();
		if(leftTime==0 && startPlayer==clientPlayer && isGameOver==false){
			startPlayer.Finished();
			messages.text = "Timeout! Round finished.";
			messages.alpha = 1;
		}
		if(dynamicChange){
			float deltaT = Time.deltaTime;
			float ratio = deltaT / changeTime;
			changePastTime += deltaT;
			moneyI += moneyChange*ratio;
			money.text = "$" + (int)moneyI;
			energyI += energyChange*ratio;
			energy.text = "" + (int)energyI;
			healthI += healthChange*ratio;
			health.text = "" + (int)healthI;
			intellI += intellChange*ratio;
			intell.text = "" + (int)intellI;
			creditsI += creditsChange*ratio;
			credits.text = "" + (int)creditsI;
			depositI += depositChange*ratio;
			deposit.text = "$" + (int)depositI;
			if(changePastTime > changeTime){
				dynamicChange = false;
				money.text = "$" + clientPlayer.wealth;
				energy.text = "" + clientPlayer.energy;
				health.text = "" + clientPlayer.health;
				intell.text = "" + clientPlayer.intell;
				credits.text = "" + clientPlayer.credit;
				deposit.text = "$" + clientPlayer.deposit;
			}
		}
	}
}
