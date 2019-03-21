using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//网络管理
public class NetManager
{

    private static NetManager instance = new NetManager();

    public Connection srvConn;
    private ProtocolBase heatBeatprotocol;
    public UnityTimer timer;
    //心跳时间
    private float lastTickTime = 0;
    private float heartBeatTime = 5;


    public class UnityTimer{
        private float endTime;
    // private int timeLength;

        public void setTimer(int time){
            endTime = Time.time + time;
        }

        public int getLeaveTime(){
            float leaveTime =endTime - Time.time;
            if(leaveTime > 0){
                return (int)leaveTime;
            }
            return 0;
        }

    }


    public void Update()
    {
        srvConn.msgDist.Update();
        //platformConn.Update();
        //心跳
        if (srvConn.status == Connection.Status.Connected)
        {
            if (Time.time - lastTickTime > heartBeatTime)
            {
                srvConn.Send(heatBeatprotocol);
                lastTickTime = Time.time;
            }
        }
    }

    private NetManager(){
        srvConn = new Connection();
        timer = new UnityTimer();
         //具体的发送内容根据服务端设定改动
        heatBeatprotocol = new ProtocolBytes();
        ((ProtocolBytes)heatBeatprotocol).AddString("HeatBeat");
    }

    public static NetManager getInstance(){
        return instance;
    }

    public void RegistProtocoal(string name, MsgDistribution.Delegate cb){
        MsgDistribution msg = srvConn.msgDist;
        msg.AddListener(name, cb);
    }

     public void RegistOneceProtocoal(string name, MsgDistribution.Delegate cb){
        MsgDistribution msg = srvConn.msgDist;
        msg.AddOnceListener(name, cb);
    }
    
    public void SendMsg(string p){
        byte[] byetes = System.Text.Encoding.UTF8.GetBytes (p);
        ProtocolBase protocol = srvConn.proto.Decode(byetes, 0, byetes.Length);
        srvConn.Send(protocol);
    }

}