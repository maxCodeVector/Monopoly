package logic;

import base.Protocol;
import model.Player;
import model.RoomManager;

public class HandleRoomMsg extends Handle {

    @Override
    public void broadcast(Protocol protocol) {
        Player[] players = RoomManager.createRoom("null").getAllPlayer();
        for(Player player: players){
            if(player==null)
                continue;
            Conn conn = player.getConnection();
            if(conn!=null && conn.isUse()) {
                conn.sendMsg(protocol);
            }
        }
    }


}
