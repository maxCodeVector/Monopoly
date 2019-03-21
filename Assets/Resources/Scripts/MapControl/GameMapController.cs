using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMapController : MapController{
    private GamePlayer player;
	public GameObject uiRoot = GameObject.Find("UI Root");
	private static MapController instance = new GameMapController();

	private GameMapController(){}
    void Start () {
        uiRoot = GameObject.Find("UI Root");
	}
	public static MapController getInstance(){
        return instance;
    }
    public override void triggerEvents(){
        GameObject messageBox = Resources.Load("prefabs/UI/gameWindow") as GameObject;
        messageBox = NGUITools.AddChild(GameController.uiRoot, messageBox);
    }
    public override void setPlayer(GamePlayer player0){
        this.player = player0;
    }
}
