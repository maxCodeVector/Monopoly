using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
public class NetRoot : MonoBehaviour {

	// Use this for initialization
	void Start () {
		NetManager netMgr = NetManager.getInstance();
		netMgr.srvConn.Connect("10.21.94.14", 12000);
		netMgr.srvConn.proto = new ProtocolBytes();
		netMgr.SendMsg("CONNECTION");
	}
	
	// Update is called once per frame
	void Update () {
		NetManager netMgr = NetManager.getInstance();
		try{
			netMgr.Update();
		}catch (SocketException e)
        {
            Debug.Log("line off:" + e.Message);
			netMgr.srvConn.Connect("10.21.94.14", 12000);
        }
	}
}
