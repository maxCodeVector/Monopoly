package server.logic;

import server.base.Action;
import server.base.Protocol;
import server.base.ServerApplication;
import server.connection.Conn;
import server.connection.Event;
import server.connection.LaunchServer;
import server.connection.ServeNet;


@Action("query")
@ServerApplication
public class MonoServer implements Event {

    @Override
    public void handleEvent(Protocol protocol, Conn conn) {
        String queryResult = conn.getIdPoolMsg();
        conn.sendMsg(protocol.SendMsg(queryResult));
    }

    public static void main(String args[]) {
        ServeNet serveNet = ServeNet.getInstance();
        serveNet.setPort(12000);
        LaunchServer.launch(MonoServer.class, args);
    }

}
