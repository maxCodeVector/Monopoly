using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkController : MapController{
    private GamePlayer player;
	public GameObject uiRoot = GameObject.Find("UI Root");
	private static MapController instance = new DarkController();

	private DarkController(){}
    void Start () {
        uiRoot = GameObject.Find("UI Root");
	}
	public static MapController getInstance(){
        return instance;
    }
    public override void triggerEvents(){
        GameObject messageBox = Resources.Load("prefabs/UI/darkWindow") as GameObject;
        messageBox = NGUITools.AddChild(GameController.uiRoot, messageBox);
        GameController.clientPlayer.inDark = true;
        GameController.clientPlayer.Finished();
    }
    public override void setPlayer(GamePlayer player0){
        this.player = player0;
    }
}
