package server.logic;

import server.connection.AfterDoThis;
import server.connection.ServeNet;

public class MonoServer {

    public static void main(String args[]){
        ServeNet serveNet = ServeNet.getInstance();
        serveNet.setPort(12000);
        AfterDoThis afterAction = new RegisterProtocol(serveNet);
        serveNet.afterInstance(afterAction);
        serveNet.setAfterDoThis(afterAction);
        serveNet.start();
    }

}
