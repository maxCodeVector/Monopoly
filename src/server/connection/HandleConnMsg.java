package server.connection;

import server.base.Protocol;

public class HandleConnMsg extends Handle {

    @Override
    public void broadcast(Protocol protocol) {
        Conn[] conns = ServeNet.getInstance().getConnPool();
        for(Conn conn: conns){
            if(conn!=null && conn.isUse()) {
                conn.sendMsg(protocol);
            }
        }
    }

}
