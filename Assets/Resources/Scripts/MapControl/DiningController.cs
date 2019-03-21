using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiningController : MapController{
    private GamePlayer player;
	public GameObject uiRoot = GameObject.Find("UI Root");
	private static MapController instance = new DiningController();

	private DiningController(){}
    void Start () {
        uiRoot = GameObject.Find("UI Root");
	}
	public static MapController getInstance(){
        return instance;
    }
    public override void triggerEvents(){
        GameObject messageBox = Resources.Load("prefabs/UI/diningWindow") as GameObject;
        messageBox = NGUITools.AddChild(GameController.uiRoot, messageBox);
    }
    public override void setPlayer(GamePlayer player0){
        this.player = player0;
    }
}
