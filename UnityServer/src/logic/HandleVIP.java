package logic;

import base.Protocol;

public class HandleVIP extends Handle{

    private Conn conn = null;

    @Override
    void broadcast(Protocol protocol) {
        if(conn != null && conn.isUse())
            conn.sendMsg(protocol);
        else
            conn = null;
    }

    void setConnection(Conn conn){
        if(this.conn == null) {
            this.conn = conn;
            this.conn.sendMsg(ServeNet.getInstance().getProtocol().SendMsg("You are vip:"+conn.getAddress()));
        }
    }

}
