using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeachController : MapController{
    private GamePlayer player;
	public GameObject uiRoot = GameObject.Find("UI Root");
	private static MapController instance = new TeachController();

	private TeachController(){}
    void Start () {
        uiRoot = GameObject.Find("UI Root");
	}
	public static MapController getInstance(){
        return instance;
    }
    public override void triggerEvents(){
        GameObject messageBox = Resources.Load("prefabs/UI/teachWindow") as GameObject;
        messageBox = NGUITools.AddChild(GameController.uiRoot, messageBox);
    }
    public override void setPlayer(GamePlayer player0){
        this.player = player0;
    }
}
