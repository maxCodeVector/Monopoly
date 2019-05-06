package server.connection;

import server.base.Protocol;
import server.model.Player;
import server.model.Room;

abstract class Handle {

    abstract void broadcast(Protocol protocol);

    void sendToPlayer(Protocol protocol, Conn conn) {
        if (conn != null && conn.isUse())
            conn.sendMsg(protocol);
    }

    void sendToRoom(Protocol protocol, Room room) {
        for (Player p: room.getAllPlayer()){
            if(Player.isAccessible(p))
                p.getConnection().sendMsg(protocol);
        }
    }

}
