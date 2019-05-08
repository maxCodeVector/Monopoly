package server.logic;

import server.base.ServerApplication;
import server.connection.LaunchServer;
import server.connection.ServeNet;

@ServerApplication
public class MonoServer {

    public static void main(String args[]){
        ServeNet serveNet = ServeNet.getInstance();
        serveNet.setPort(12000);
        LaunchServer.launch(MonoServer.class, args);
    }

}
