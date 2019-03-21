using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;
using System.IO; 
using System.Reflection; 
using System.Text;

public class GlobalControl : MonoBehaviour {
	// private Player clientPlayer;
	private int clientSessionInfo;
	private static string clientNickName;
	public static int clientID;
	public static int startID;
	public static List<Player> players = new List<Player>();
	public static NetManager netMgr = NetManager.getInstance();
	public static RoomController roomController;
	public static int roomID;
	public static float volume;
	public static AudioSource music;

	private GlobalControl(){}

	public static void sendChooseCharacterMessage(int i){
		netMgr.SendMsg(string.Format("ROLE,{0},{1}",clientID,i));
	}

	public static void sendForwardMessage(string id, float x, float z){
		netMgr.SendMsg(string.Format("FORWARD,{0},{1},{2}",id,x,z));
	}

	public static void sendTeleportMessage(string id, float x, float z){
		netMgr.SendMsg(string.Format("TELEPORT,{0},{1},{2}",id,x,z));
	}

	public static void sendFinishedMessage(string id){
		netMgr.SendMsg(string.Format("FINISHED,{0}",id));
	}

	public static void sendPoorFailed(){
		netMgr.SendMsg(string.Format("POOR,{0}",clientID));
	}

	public static void sendBuyHouse(string cellName){
		netMgr.SendMsg(string.Format("BUYHOUSE,{0},{1}",clientID,cellName));
	}

	public static void sendUpgradeHouse(int level, string cellName){
		netMgr.SendMsg(string.Format("UPGRADEHOUSE,{0},{1},{2}",clientID,level,cellName));
	}

	public static void sendToll(string id2, int money){
		netMgr.SendMsg(string.Format("TOLL,{0},{1},{2}",clientID,id2,money));
	}

	public static void setBomb(string cellName){
		netMgr.SendMsg(string.Format("SETBOMB,{0},{1}",clientID,cellName));
	}

	public static void noBomb(string cellName){
		netMgr.SendMsg(string.Format("NOBOMB,{0}",cellName));
	}

	public static void sendCreateRoom(string roomName, int SessionID){
		netMgr.SendMsg(string.Format("CREATEROOM,{0},{1},{2}",roomName,SessionID,clientNickName));
	}

	public static void sendJoinRoom(int roomID0, int sessionID){
		// roomID = roomID0;
		netMgr.SendMsg(string.Format("JOINROOM,{0},{1},{2}",roomID0,sessionID,clientNickName));
	}

	public static void setVolume(){
		if(music != null){
			music.volume = volume;
		}
	}
	void Start () {
		Object.DontDestroyOnLoad(gameObject);

		//ע��Э�鷽��
		NetManager.getInstance().RegistProtocoal("CONNECTED",delegate(ProtocolBase p){
			//���л���Room��ͼ
			if(SceneManager.GetActiveScene().name == "RoomList"){
				switch2Room();
			}
			players.Clear();
			string[] desc = p.GetDesc().Split(',');
			roomID = int.Parse(desc[1].Trim());
			for(int i = 2; i < desc.Length; i+=4){
				Player newPlayer = new Player(desc[i].Trim(), desc[i+1].Trim(), int.Parse(desc[i+2]),desc[i+3].Trim());
				players.Add(newPlayer);
			}
			if(roomController != null){
				roomController.showPlayers(players);
			}
			




			// if(SceneManager.GetActiveScene().name != "Game"){
			// 	string[] desc = p.GetDesc().Split(',');
			// 	for(int i = 1; i < desc.Length; i+=4){
			// 		bool found = false;
			// 		foreach(Player player in players){
			// 			if(player.id.Equals(desc[i+1].Trim())){
			// 				found = true;
			// 				player.role = int.Parse(desc[i+2]);
			// 			}
			// 		}
			// 		if(found == false){
			// 			Player newPlayer = new Player(desc[i].Trim(), desc[i+1].Trim(), int.Parse(desc[i+2]),desc[i+3].Trim());
			// 			if(clientSessionInfo == int.Parse(newPlayer.ipInfo)){
			// 				newPlayer.isClient = true;
			// 				// clientPlayer = newPlayer;
			// 				clientID = int.Parse(newPlayer.id);
			// 			}
			// 			players.Add(newPlayer);
			// 		}
			// 	}
			// 	for(int j = 0; j < players.Count; j++){
			// 		bool found = false;
			// 		for(int i = 1; i < desc.Length; i+=4){
			// 			if(players[j].id.Equals(desc[i+1].Trim()))
			// 				found = true;
			// 		}
			// 		if(found == false){
			// 			roomController.returnRole(players[j].role);
			// 			players.Remove(players[j]);
			// 		}
			// 	}
			// 	roomController.showPlayers(players);
				// TODO ��roomcontroller ���ƽ���ĸı�??
			// }


		});

		NetManager.getInstance().RegistProtocoal("START",delegate(ProtocolBase p){
			string[] desc = p.GetDesc().Split(',');
			startID = int.Parse(desc[1].Trim());
			GameController.setStartPlayer(startID);
			if(SceneManager.GetActiveScene().name == "Room"){
				switch2GameScene();
			}
		});

		NetManager.getInstance().RegistProtocoal("PLAYER",delegate(ProtocolBase p){
			string[] desc = p.GetDesc().Split(',');
			clientID = int.Parse(desc[2].Trim());
		});

		NetManager.getInstance().RegistProtocoal("NETERROR",delegate(ProtocolBase p){
			GameObject uiRoot = GameObject.Find("UI Root");
			GameObject messageBox = Resources.Load("prefabs/UI/netErrorWindow") as GameObject;
        	messageBox = NGUITools.AddChild(uiRoot, messageBox);
		});
	}

	// Update is called once per frame
	void Update () {
		try{
			netMgr.Update();
		}catch (SocketException e){
            Debug.Log("line off:" + e.Message);
			// netMgr.srvConn.Connect("101.132.169.242", 12000);
			// netMgr.srvConn.Connect("10.21.94.14", 12000);
			// netMgr.srvConn.Connect("127.0.0.1", 12000);
			// netMgr.srvConn.Connect("192.168.0.100", 12000);
        }
	}

	public void sendConnection(){
		//���ӷ�����
		UIInput input = GameObject.Find("UI Root/Input").GetComponent<UIInput>();
		clientNickName = input.value;
		Debug.Log("hello, I am here");
		//netMgr.srvConn.Connect("101.132.169.242", 12000);
		//netMgr.srvConn.Connect("10.21.94.14", 12000);
		netMgr.srvConn.Connect("127.0.0.1", 12000);
		// netMgr.srvConn.Connect("192.168.0.100", 12000);
		netMgr.srvConn.proto = new ProtocolBytes();
		netMgr.SendMsg("CONNECTION"); // �ǳƣ���
	}

	public void switch2Room(){
		SceneManager.LoadScene("Room");
	}

	public void switch2GameScene(){
		SceneManager.LoadScene("Game");
	}

	public void switch2RoomList(){
		SceneManager.LoadScene("RoomList");
	}

	public void how(){
		GameObject uiRoot = GameObject.Find("UI Root");
		GameObject messageBox = Resources.Load("prefabs/UI/page") as GameObject;
        messageBox = NGUITools.AddChild(uiRoot, messageBox);
	}
	

	// public string getExternalIP(){
	// 	var webClient = new WebClient();
	// 	string tempip = "";
	// 	try{
	// 		webClient.Credentials = CredentialCache.DefaultCredentials;
	// 		byte[] pageDate = webClient.DownloadData("http://pv.sohu.com/cityjson?ie=utf-8");
	// 		string ip = Encoding.UTF8.GetString(pageDate);
	// 		webClient.Dispose();

	// 		Match rebool = Regex.Match(ip, @"\d{2,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
	// 		return rebool.Value;
	// 	}catch {
	// 		return "no ip";
	// 	}
	// }

	// public string getExternalIP2(){
	// 	string tempip = "";
	// 	try{
	// 		WebRequest wr = WebRequest.Create("http://www.ip138.com/ips138.asp");
	// 		Stream s = wr.GetResponse().GetResponseStream();
	// 		StreamReader sr = new StreamReader(s, Encoding.Default);
	// 		string all = sr.ReadToEnd(); //��ȡ��վ������
	// 		int start = all.IndexOf("����IP��ַ�ǣ�[") + 9;
	// 		int end = all.IndexOf("]", start);
	// 		tempip = all.Substring(start, end - start);
	// 		sr.Close();
	// 		s.Close();
	// 		return tempip;
	// 	}catch {
	// 		return "no ip";
	// 	}
	// }
}
