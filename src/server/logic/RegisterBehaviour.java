package server.logic;

import server.base.Action;
import server.base.Protocol;
import server.connection.AfterDoingThis;
import server.connection.Conn;
import server.connection.ServeNet;
import server.model.Player;
import server.model.Room;
import server.util.Constant;

@Action
public class RegisterBehaviour implements AfterDoingThis {
    public RegisterBehaviour() {
    }

    void registerPoor(ServeNet serveNet) {
        serveNet.addEvent(Protocol.POOR, (protocol, conn) -> {
            Room room = conn.player.getRoom();
            String[] desc = protocol.getDesc().split(",");
            int playerId = Integer.parseInt(desc[1].trim());
            if (conn.player != null && playerId == conn.player.id) {
                conn.player.state = Constant.Status.WEED;
                //playerOff(room, playerId);
                serveNet.broadcast(protocol, room);
            }
        });
    }

    void registerWarn(ServeNet serveNet) {
        serveNet.addEvent(Protocol.WARN, (protocol, conn) -> {
            Room room = conn.player.getRoom();
            String[] desc = protocol.getDesc().split(",");
            int playerId = Integer.parseInt(desc[1].trim());
            int warnNum = Integer.parseInt(desc[2].trim());
            if (warnNum >= 3 && conn.player != null && playerId == conn.player.id)
                conn.player.state = Constant.Status.WEED;
            //playerOff(room, playerId);)
            serveNet.broadcast(protocol, room);
        });
    }

    void registerAnswer(ServeNet serveNet) {
        serveNet.addEvent(Protocol.ANSWER, (protocol, conn) -> {
            String[] desc = protocol.getDesc().split(",");
            int queryOrder = Integer.parseInt(desc[1].trim());
            int playerId = Integer.parseInt(desc[2].trim());
            int money = Integer.parseInt(desc[3].trim());
            int healthy = Integer.parseInt(desc[4].trim());
            float GPA = Float.parseFloat(desc[5].trim());
            int score = Integer.parseInt(desc[6].trim());
            Room room = conn.player.getRoom();
            Player[] players = room.getAllPlayer();
            for (int i = 0; i < players.length; i++) {
                if (Player.isNeed(players[i]) && players[i].id == playerId) {
                    players[i].setTempData(money, healthy, GPA);
                    room.setAnswered(i, queryOrder);
                }
            }
            if (room.hasAnswer(queryOrder)) {
                if (room.isOver()) {
                    Protocol overProto = serveNet.getProtocol().SendMsg(
                            String.format("%s", Protocol.GAME_OVER));
                    serveNet.handleMsg(overProto, conn);
                } else {
                    Protocol overProto = serveNet.getProtocol().SendMsg(
                            String.format("%s", Protocol.SCHOLARSHIP));
                    serveNet.handleMsg(overProto, conn);
                }
            }
//            if (score < Room.MAX_SCORE) {
//                Protocol scoreProto = serveNet.getProtocol().SendMsg(
//                        String.format("%s, %d", Protocol.SCORE, playerId));
//                serveNet.broadcast(scoreProto, room);
//            }
        });
    }

    void registerScholarship(ServeNet serveNet) {
        serveNet.addEvent(Protocol.SCHOLARSHIP, (protocol, conn) -> {
            Room room = conn.player.getRoom();
            String msg = room.getMoneyMsg();
            protocol.addBytes(msg.getBytes());
            serveNet.broadcast(protocol, room);
        });
    }

    void registerOver(ServeNet serveNet) {
        serveNet.addEvent(Protocol.GAME_OVER, (protocol, conn) -> {
            Room room = conn.player.getRoom();
            String msg = room.getOverMsg();
            protocol.addBytes(msg.getBytes());
            serveNet.broadcast(protocol, room);
            room.gameInit();
        });
    }

    @Override
    public void action(ServeNet serveNet) {
        registerAnswer(serveNet);
        registerOver(serveNet);
        registerPoor(serveNet);
        registerScholarship(serveNet);
        registerWarn(serveNet);
    }

    @Override
    public void fail(Conn conn) {

    }
}