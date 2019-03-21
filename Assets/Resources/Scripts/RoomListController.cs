using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomListController : MonoBehaviour {
	public static NetManager netMgr = NetManager.getInstance();
	public static bool isBack = false;
	public static int roomID;
	// public static bool noConnection = true;

	// Use this for initialization
	void Start () {

		netMgr = NetManager.getInstance();
		NetManager.getInstance().RegistProtocoal("ROOMLIST",delegate(ProtocolBase p){
			if(SceneManager.GetActiveScene().name == "RoomList"){
				GameObject ScrollView = GameObject.Find("UI Root/AnchorCenter/Scroll View");
				GameObject label = GameObject.Find("UI Root/AnchorCenter/NoRoomLabel");
				GameObject connectionLabel = GameObject.Find("UI Root/AnchorCenter/NoConnectionLabel");

				connectionLabel.SetActive(false);
				// noConnection = false;
				GameObject reconnectionWindow = GameObject.Find("UI Root/netErrorWindow");
				if(reconnectionWindow != null){
					Destroy(reconnectionWindow);
				}
				List<Transform> lst = new List<Transform>();
				foreach (Transform child in ScrollView.transform){
					lst.Add(child); 
				}
				for(int i = 0;i < lst.Count;i++){
					Destroy(lst[i].gameObject);
				}
				string[] desc = p.GetDesc().Split(',');
				int roomNum = int.Parse(desc[1]);
				if(roomNum == 0){
					label.SetActive(true);
				}else{
					label.SetActive(false);
				}
				for(int i = 0; i < roomNum; i++){
					GameObject item = Resources.Load("Prefabs/UI/RoomList/RoomItem") as GameObject;
					item = NGUITools.AddChild(ScrollView, item);
					item.transform.localPosition = new Vector3(-183.3f+(i%2)*367.3f, 170-(i/2)*102,0);
					int id = int.Parse(desc[3*i+2].Trim());
					string name = desc[3*i+3].Trim();
					int num = int.Parse(desc[3*i+4].Trim());
					item.GetComponent<RoomItem>().setAndShowValues(id, name, num);
				}
			}
		});

		if(isBack){
			netMgr.SendMsg(string.Format("LEAVEROOM,{0},{1}",roomID,GlobalControl.clientID));
			isBack = false;
		}

		if(GameObject.Find("UI Root/AnchorCenter/NoConnectionLabel").activeInHierarchy){
			GameObject uiRoot = GameObject.Find("UI Root");
			GameObject messageBox = Resources.Load("prefabs/UI/netErrorWindow") as GameObject;
        	messageBox = NGUITools.AddChild(uiRoot, messageBox);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
