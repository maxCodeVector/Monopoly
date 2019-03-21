using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomController : MonoBehaviour {
	public UIAtlas characterAtlas;
	public UIAtlas MAO;
	public static string roomName;
	public UILabel nameLabel;
	private RoomController(){}
	public void chooseCharacter(int i){
		GlobalControl.sendChooseCharacterMessage(i);
	}

	public void showPlayers(List<Player> players){
		Transform characterBox = GameObject.Find("/UI Root/Room/CharacterBox").transform;
		Transform characterLabel = GameObject.Find("/UI Root/Room/CharacterLabel").transform;
		Transform characterButton = GameObject.Find("/UI Root/Room/RoomButtons").transform;
		returnAllRoleButtons();

		for(int k = 1; k <= 3; k++){
			UISprite box = characterBox.Find("role"+k).GetComponent<UISprite>();
			UILabel label = characterLabel.Find("Label"+k).GetComponent<UILabel>();
			box.atlas = MAO;
			box.spriteName = "Rectangle 3";
			label.text = "";
		}

		int i = 1;
		foreach(Player player in players){
			UISprite box = characterBox.Find("role"+i).GetComponent<UISprite>();
			UILabel label = characterLabel.Find("Label"+i).GetComponent<UILabel>();
			if(player.role != -1){
				UIButton button = characterButton.Find(""+player.role).GetComponent<UIButton>();
				button.GetComponent<RoomRoleButton>().isSelected = true;
				button.state = UIButton.State.Disabled;
				button.GetComponent<BoxCollider>().enabled = false;
				// RoomButtonMethods.buttons.Remove(button);
			}
			box.atlas = characterAtlas;
			box.spriteName = string.Format("0{0}_body",player.role);
			label.text = player.nickName;
			i++;
		}
	}

	public void returnRole(int roleID){
		if(roleID != -1){
			Transform characterButton = GameObject.Find("/UI Root/Room/RoomButtons").transform;
			UIButton button = characterButton.Find(""+roleID).GetComponent<UIButton>();
			button.state = UIButton.State.Normal;
			button.GetComponent<BoxCollider>().enabled = true;
			button.GetComponent<RoomRoleButton>().isSelected = false;
			// RoomButtonMethods.buttons.Add(button);
		}
	}

	public void returnAllRoleButtons(){
		RoomButtonMethods.initCharacterButtons();
		foreach(UIButton button in RoomButtonMethods.buttons){
			button.state = UIButton.State.Normal;
			button.GetComponent<BoxCollider>().enabled = true;
			button.GetComponent<RoomRoleButton>().isSelected = false;
		}
	}

	public void clickBackButton(){
		RoomListController.isBack = true;
		RoomListController.roomID = GlobalControl.roomID;
		SceneManager.LoadScene("RoomList");
		RoomButtonMethods.buttons.Clear();
	}
	// Use this for initialization
	void Start () {
		nameLabel.text = roomName;
		characterAtlas = Resources.Load("Textures/Characters/Characters", typeof(UIAtlas)) as UIAtlas;
		MAO = Resources.Load("UI/MAP", typeof(UIAtlas)) as UIAtlas;
		GlobalControl.roomController = this;
		showPlayers(GlobalControl.players);

		NetManager.getInstance().RegistProtocoal("ROLEFAIL",delegate(ProtocolBase p){
			//TODO
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
