package logic;

import base.Protocol;
import model.Room;
import util.Constant;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketTimeoutException;
import java.util.HashMap;
import java.util.Map;

public class ServeNet {

    private static ServeNet instance;
    private ServerSocket serverSocket;
    private Conn[] connPool;
    private Protocol protocol;
//    private Handle handleRoom = new HandleRoomMsg();
    private Handle handleConn = new HandleConnMsg();
    private Handle handleVIP = new HandleVIP();
    private Map<String, Event> eventList;

    private ServeNet(int port) {
        try {
            serverSocket = new ServerSocket(port);
            connPool = new Conn[Constant.conf.maxConn];
            eventList = new HashMap<>();
            new RegisterProtocol(this);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public static ServeNet getInstance() {
        if (instance == null)
            instance = new ServeNet(Constant.conf.port);
        return instance;
    }

    void addEvent(String protocolName, Event event) {
        this.eventList.put(protocolName, event);
    }

    /**
     * broad cast in the room of player of this conn
     */
    void broadcast(Protocol protocol, Room room) {
        handleConn.sendToRoom(protocol, room);
    }

    /**
     * broad cast for all connection
     */
    void broadcastAll(Protocol protocol) {
        handleConn.broadcast(protocol);
    }

    void sendToPlayer(Protocol protocol, Conn conn) {
        handleConn.sendToPlayer(protocol, conn);
    }

    void handleMsg(Protocol protocol, Conn conn) {
        String protocolName = protocol.getName();
        if (eventList.containsKey(protocolName)) {
//            handleVIP.broadcast(protocol); // use to test server
            eventList.get(protocolName).handleEvent(protocol, conn);
        }else {
//            protocol.addBytes((", " + conn.getAddress()).getBytes());
            handleConn.sendToRoom(protocol, conn.player.getRoom());
        }
    }

    public void setProtocol(Protocol protocol) {
        this.protocol = protocol;
    }

    public Protocol getProtocol() {
        return this.protocol;
    }

    Conn[] getConnPool() {
        return connPool;
    }

    private int getConnIndex() {
        for (int i = 0; i < connPool.length; i++) {
            if (connPool[i] == null) {
                connPool[i] = new Conn();
                return i;
            }
            if (!connPool[i].isUse())
                return i;
        }
        return -1;
    }

    public void start() {
        System.out.println("等待远程连接，端口号为：" + serverSocket.getLocalPort() + "...");
        while (true) {
            try {
                Socket server = serverSocket.accept();
                int index = getConnIndex();
                if (index != -1) {
                    connPool[index].init(server);
                    new Thread(connPool[index]).start();
                    // if this is a VIP connection
                    if (connPool[index].getAddress().startsWith(Protocol.RootIP))
                        ((HandleVIP) handleVIP).setConnection(connPool[index]);
                }
            } catch (SocketTimeoutException s) {
                System.out.println("Socket timed out!");
                break;
            } catch (IOException e) {
                e.printStackTrace();
                break;
            }
        }
    }

}