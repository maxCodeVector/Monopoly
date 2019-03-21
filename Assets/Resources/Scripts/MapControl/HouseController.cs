using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HouseController : MapController{
    public MapCell mapCell;
    public Vector3 hatPosition;
    public GamePlayer player; 
	public GameObject uiRoot = GameObject.Find("UI Root");
    public GamePlayer owner; // ��������Ϸ�е�������ҡ�
    public int level = 0; // 0��ʾ����

    public HouseController(MapCell mapCell1, Vector3 po){
        mapCell = mapCell1;
        hatPosition = po;
    }
    void Start () {
        uiRoot = GameObject.Find("UI Root");
	}
// �ߵ���������ֻ���� clientPlayer (���˲���)��������������˵ķ��ӣ����������򷿣�client�Ŀ�������
    public override void triggerEvents(){
        if(level != 0){ // �����Ѿ�����
            if(owner.id.Equals(player.id)){ // �Լ��ķ���
                GameObject messageBox = Resources.Load("Prefabs/UI/upgradeHouseWindow") as GameObject;
                messageBox = NGUITools.AddChild(GameController.uiRoot, messageBox);
                messageBox.GetComponent<upgradeHouse>().setController(this);
            }else{ // ���˵ķ���
                GameObject messageBox = Resources.Load("Prefabs/UI/tollHouseWindow") as GameObject;
                messageBox = NGUITools.AddChild(GameController.uiRoot, messageBox);
                messageBox.GetComponent<tollHouse>().setController(this);
            }
        }else{ // �����ķ���
            GameObject messageBox = Resources.Load("Prefabs/UI/buyHouseWindow") as GameObject;
            messageBox = NGUITools.AddChild(GameController.uiRoot, messageBox);
            messageBox.GetComponent<buyHouse>().setController(this);
        }
    }
    public override void setPlayer(GamePlayer player0){
        this.player = player0;
    }
    public int getToll(){
        return 500 * level;
    }
    // �õ� ����˷���Ǯ������ʱ������ �����˷���Ǯ���Լ��ķ���
    public int getUpgradeCost(){
        if(level == 0)
            return 1500;
        return 1200;
    }
}
