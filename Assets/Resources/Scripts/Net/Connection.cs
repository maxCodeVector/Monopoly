using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

//��������

public class Connection
{
    //����
    const int BUFFER_SIZE = 1024;
    //Socket
    private Socket socket;
    //Buff
    private byte[] readBuff;
    private int buffCount = 0;
    //մ���ְ�
    private Int32 msgLength = 0;
    private byte[] lenBytes;
    //Э��
    public ProtocolBase proto;
    public ProtocolBase failProto;
    //��Ϣ�ַ�
    public MsgDistribution msgDist;
    ///״̬
    public enum Status
    {
        None,
        Connected,
    };
    public Status status;

    public Connection(){
        readBuff = new byte[BUFFER_SIZE];
        lenBytes = new byte[sizeof(Int32)];
        status = Status.None;
        msgDist = new MsgDistribution();
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("NETERROR");
        failProto = protocol;
    }

    //���ӷ����
    public bool Connect(string host, int port)
    {
        try
        {
            //socket
            if(socket != null){
                Close();
            }
            socket = new Socket(AddressFamily.InterNetwork,
                      SocketType.Stream, ProtocolType.Tcp);
            socket.SetSocketOption(SocketOptionLevel.Socket,SocketOptionName.ReceiveTimeout,1000);
            // socket.ReceiveTimeout = 5;
            //Connect
            socket.Connect(host, port);
            //BeginReceive
            socket.BeginReceive(readBuff, buffCount,
                      BUFFER_SIZE - buffCount, SocketFlags.None,
                      ReceiveCb, readBuff);
            Debug.Log("connect success!");
            //״̬
            status = Status.Connected;
            return true;
        }
        catch (Exception e)
        {
             Debug.Log("connect failue"+e.Message);
              lock (msgDist.msgList)
            {
                msgDist.msgList.Add(failProto);
            }
             return false;
        }
    }

    //�ر�����
    public bool Close()
    {
        try
        {
            socket.Close();
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("close failure:" + e.Message);
            return false;
        }
    }

    //���ջص�
    private void ReceiveCb(IAsyncResult ar)
    {
        try
        {
            int count = socket.EndReceive(ar);
            buffCount = buffCount + count;
            ProcessData();
            socket.BeginReceive(readBuff, buffCount,
                     BUFFER_SIZE - buffCount, SocketFlags.None,
                     ReceiveCb, readBuff);
        }
        catch (Exception e)
        {
            status = Status.None;
            Debug.Log("Receive failure!"+e.Message);
             lock (msgDist.msgList)
            {
                msgDist.msgList.Add(failProto);
            }
        }
    }

    //��Ϣ����
    private void ProcessData()
    {
        //С�ڳ����ֽ�
        if (buffCount < sizeof(Int32))
            return;
        //��Ϣ����
        Array.Copy(readBuff, lenBytes, sizeof(Int32));

        msgLength = 0;
        //�ɸ�λ����λ
        for(int i = 0; i < 4; i++) {
            int shift= (4-1-i) * 8;
            msgLength +=(lenBytes[i] & 0x000000FF) << shift;//����λ��
        }

       // msgLength = BitConverter.ToInt32(lenBytes, 0);
        if (buffCount < msgLength + sizeof(Int32))
            return;
        //������Ϣ
        ProtocolBase protocol = proto.Decode(readBuff, sizeof(Int32), msgLength);
        Debug.Log("RECV: " + protocol.GetDesc());
        lock (msgDist.msgList)
        {
            msgDist.msgList.Add(protocol);
        }
        //����Ѵ������Ϣ
        int count = buffCount - msgLength - sizeof(Int32);
        Array.Copy(readBuff,sizeof(Int32)+ msgLength, readBuff, 0, count);
        buffCount = count;
        if (buffCount > 0)
        {
            ProcessData();
        }
    }


    public bool Send(ProtocolBase protocol)
    {
        if (status != Status.Connected)
        {
            Debug.Log("[Connection]��û���Ӿͷ��������ǲ��õ�, will re connect!");
            return true;
        }
        socket.Send(protocol.Encode());
        Debug.Log("send: " + protocol.GetDesc());
        return true;
    }

    public bool Send(ProtocolBase protocol, string cbName, MsgDistribution.Delegate cb)
    {
        if (status != Status.Connected)
            return false;
        msgDist.AddOnceListener(cbName, cb);
        return Send(protocol);
    }

    public bool Send(ProtocolBase protocol, MsgDistribution.Delegate cb)
    {
        string cbName = protocol.GetName();
        return Send(protocol, cbName, cb);
    }



}
