using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryController : MapController{
    private GamePlayer player;
	public GameObject uiRoot = GameObject.Find("UI Root");
	private static MapController instance = new LibraryController();

	private LibraryController(){}
    void Start () {
        uiRoot = GameObject.Find("UI Root");
	}
	public static MapController getInstance(){
        return instance;
    }
    public override void triggerEvents(){
    }
    public override void setPlayer(GamePlayer player0){
        this.player = player0;
    }
}
