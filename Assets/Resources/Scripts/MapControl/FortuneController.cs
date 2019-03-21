using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortuneController : MapController{
    private GamePlayer player;
    public UIAtlas itemAtlas;
    public UISprite sprite;
    public GameObject messageBox;
    
	public GameObject uiRoot = GameObject.Find("UI Root");
	private static MapController instance = new FortuneController();

	private FortuneController(){}
    void Start () {
        uiRoot = GameObject.Find("UI Root");
	}
	public static MapController getInstance(){
        return instance;
    }
    public override void triggerEvents(){
        // itemAtlas = Resources.Load("UI/Window Atals", typeof(UIAtlas)) as UIAtlas;
        System.Random rand = new System.Random();
        int x = rand.Next(1, 5);
        switch (x){
            case 1: //
                messageBox = Resources.Load("prefabs/UI/luck&doom") as GameObject;
                messageBox = NGUITools.AddChild(GameController.uiRoot, messageBox);
                sprite = messageBox.GetComponent<UISprite>();
                sprite.spriteName = "lucky-efficient";
                GameController.clientPlayer.efficientRatio = 1.1f;
                GameController.clientPlayer.Finished();
                break;
            case 2:
                messageBox = Resources.Load("prefabs/UI/luck&doom") as GameObject;
                messageBox = NGUITools.AddChild(GameController.uiRoot, messageBox);
                sprite = messageBox.GetComponent<UISprite>();
                sprite.spriteName = "lucky-health";
                GameController.clientPlayer.health += 5;
                GameController.checkAndNotify();
                GameController.clientPlayer.Finished();
                break;
            case 3:
                messageBox = Resources.Load("prefabs/UI/luck&doom") as GameObject;
                messageBox = NGUITools.AddChild(GameController.uiRoot, messageBox);
                sprite = messageBox.GetComponent<UISprite>();
                sprite.spriteName = "lucky-intel";
                GameController.clientPlayer.intell += 5;
                GameController.checkAndNotify();
                GameController.clientPlayer.Finished();
                break;
            case 4:
                messageBox = Resources.Load("prefabs/UI/luck&doom") as GameObject;
                messageBox = NGUITools.AddChild(GameController.uiRoot, messageBox);
                sprite = messageBox.GetComponent<UISprite>();
                sprite.spriteName = "lucky-money";
                GameController.clientPlayer.wealth += 1000;
                GameController.checkAndNotify();
                GameController.clientPlayer.Finished();
                break;
        }
        
    }
    public override void setPlayer(GamePlayer player0){
        this.player = player0;
    }
}
