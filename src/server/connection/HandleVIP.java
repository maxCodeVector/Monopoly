package server.connection;

import server.base.Protocol;
import server.connection.Conn;
import server.connection.Handle;
import server.connection.ServeNet;

public class HandleVIP extends Handle {

    private Conn conn = null;

    @Override
    void broadcast(Protocol protocol) {
        if(conn != null && conn.isUse())
            conn.sendMsg(protocol);
        else
            conn = null;
    }

    public void setConnection(Conn conn){
        if(this.conn == null) {
            this.conn = conn;
            this.conn.sendMsg(ServeNet.getInstance().getProtocol().SendMsg("You are vip:"+conn.getAddress()));
        }
    }

}
