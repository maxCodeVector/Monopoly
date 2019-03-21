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

public class reconnectWindow : MonoBehaviour {

	public void OnClick(){
		// netMgr.srvConn.Connect("101.132.169.242", 12000);
		// netMgr.srvConn.Connect("10.21.94.14", 12000);
		GlobalControl.netMgr.srvConn.Connect("127.0.0.1", 12000);
		// netMgr.srvConn.Connect("192.168.0.100", 12000);
		GlobalControl.netMgr.srvConn.proto = new ProtocolBytes();
		GlobalControl.netMgr.SendMsg("CONNECTION"); // �ǳƣ���
		SceneManager.LoadScene("RoomList");
		Destroy(transform.parent.gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
