package client;

import base.ProtocolBase;
import logic.ServeNet;

public class StartSever{

    private static void start(){
        ServeNet server = ServeNet.getInstance();
        server.setProtocol(new ProtocolBase());
        server.start();
    }

    public static void main(String[] args){
        System.out.println("serving...");
        StartSever.start();
    }
}
