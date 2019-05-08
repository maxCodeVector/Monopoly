package server.logic;

import server.base.Action;
import server.base.Protocol;
import server.connection.AfterDoingThis;
import server.connection.Conn;
import server.connection.ServeNet;
import server.model.Player;
import server.model.Room;

@Action
public class RegisterMovement implements AfterDoingThis {

    public RegisterMovement() {
    }

    void registerStart(ServeNet serveNet) {
        serveNet.addEvent(Protocol.START, (protocol, conn) -> {
            Room room = conn.player.getRoom();
            int nextId = room.nextPlayer();
            switch (nextId) {
                case -1:// does not have any player
                    room.gameInit();
                    return;
                case -2: // game is to be over
                    Protocol overProto = serveNet.getProtocol().SendMsg(Protocol.GAME_OVER);
                    serveNet.handleMsg(overProto, conn);
                    break;
                default:
                    protocol.addBytes(String.format(", %d", nextId).getBytes());
                    serveNet.broadcast(protocol, room);
            }
            int queryOrder = room.getQueryOrder();
            if (queryOrder != 0) {
                Protocol queryProto = serveNet.getProtocol().SendMsg(
                        String.format("%s, %d", Protocol.QUERY, queryOrder));
                serveNet.broadcast(queryProto, room);
            }
        });
    }

    void registerFinish(ServeNet serveNet) {
        serveNet.addEvent(Protocol.FINISHED, (protocol, conn) -> {
            String[] desc = protocol.getDesc().split(",");
            int thisPlayer = Integer.parseInt(desc[1].trim());
            Room room = conn.player.getRoom();
            if (!room.isOver() && thisPlayer == room.currentPlayer()) {
                Protocol startProto = serveNet.getProtocol().SendMsg(Protocol.START);
                serveNet.handleMsg(startProto, conn);
            }
        });
    }

    void registerForward(ServeNet serveNet) {
        serveNet.addEvent(Protocol.FORWARD, (protocol, conn) -> {
            String[] desc = protocol.getDesc().split(",");
            int thisPlayerId = Integer.parseInt(desc[1].trim());
            float x = Float.parseFloat(desc[2]);
            float z = Float.parseFloat(desc[3]);
            Player[] players = conn.player.getRoom().getAllPlayer();
            for (Player player : players) {
                if (player == null)
                    continue;
                if (player.id == thisPlayerId) {
                    player.data.setPos(x, 0, z);
                    break;
                }
            }
            serveNet.broadcast(protocol, conn.player.getRoom());
        });
    }

    @Override
    public void action(ServeNet serveNet) {
        registerStart(serveNet);
        registerFinish(serveNet);
        registerForward(serveNet);
    }

    @Override
    public void fail(Conn conn) {

    }
}