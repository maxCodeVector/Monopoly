package server.logic;

import server.base.Protocol;
import server.connection.AfterDoingThis;
import server.connection.Conn;
import server.connection.ServeNet;
import server.model.Player;
import server.model.Room;
import server.model.RoomManager;
import server.util.Constant;
import java.util.Objects;

class RegisterProtocol implements AfterDoingThis {

    private ServeNet serveNet;

    RegisterProtocol(ServeNet serveNet) {
        this.serveNet = serveNet;
    }

    private void registAll(){
        serveNet.addEvent(Protocol.CONNECTED, (protocol, conn) -> {
            Room room = conn.player.getRoom();
            Player[] players = room.getAllPlayer();
            protocol.addBytes(String.format(
                    ", %d%s", room.id, getAllMsg(players)).getBytes());
            serveNet.broadcast(protocol, room);
        });
        registerConn();
        registerCreateRoom();
        registerJoinRoom();
        registerLeaveRoom();

        registerRole();
        registerReConn();

        registerStart();
        registerFinish();
        registerForward();

        registerAnswer();
        registerScholarship();
        registerOver();

        registerPoor();
        // registerScore();
        registerWarn();
    }

    /**
     * deal with CONNECTION request, give id and others
     */
    private void registerConn() {
        serveNet.addEvent(Protocol.CONNECTION, (protocol, conn) -> {
            Protocol proto = serveNet.getProtocol().SendMsg(
                    String.format("%s, %s", Protocol.ROOM_LIST, RoomManager.getRoomList()));
            serveNet.sendToPlayer(proto, conn);
        });
    }

    private void registerCreateRoom() {
        serveNet.addEvent(Protocol.CREATE_ROOM, (protocol, conn) -> {
            String[] desc = protocol.getDesc().split(",");
            Room room = RoomManager.createRoom(desc[1].trim());
            Player p = new Player(conn, room, desc[3].trim(), desc[2].trim());
            room.getAllPlayer()[0] = p;
            if (conn.player != null)
                conn.player.logout();
            conn.player = p;
            Protocol protoPlayer = serveNet.getProtocol().SendMsg(
                    String.format("%s, %s, %d", Protocol.PLAYER, p.getSessionId(), p.id));
            serveNet.sendToPlayer(protoPlayer, conn);
            Protocol protoConn = serveNet.getProtocol().SendMsg(Protocol.CONNECTED);
            serveNet.handleMsg(protoConn, conn);
            Protocol proto = serveNet.getProtocol().SendMsg(
                    String.format("%s, %s", Protocol.ROOM_LIST, RoomManager.getRoomList()));
            serveNet.broadcastAll(proto);
        });
    }

    private void registerJoinRoom() {
        serveNet.addEvent(Protocol.JOIN_ROOM, this::handleJoinRoomEvent);
    }

    private void registerLeaveRoom() {
        serveNet.addEvent(Protocol.LEAVE_ROOM, (protocol, conn) -> {
            String[] desc = protocol.getDesc().split(",");
            int playerId = Integer.parseInt(desc[2].trim());
            if (conn.player != null && conn.player.id == playerId)
                conn.player.logout();
            Protocol proto = serveNet.getProtocol().SendMsg(
                    String.format("%s, %s", Protocol.ROOM_LIST, RoomManager.getRoomList()));
            serveNet.broadcastAll(proto);
            Protocol protoConn = serveNet.getProtocol().SendMsg(Protocol.CONNECTED);
            serveNet.handleMsg(protoConn, conn);
        });
    }

    private void registerRole() {
        serveNet.addEvent(Protocol.ROLE, (protocol, conn) -> {
            String[] desc = protocol.getDesc().split(",");
            int thisPlayerId = Integer.parseInt(desc[1].trim());
            int targetRole = Integer.parseInt(desc[2].trim());
            synchronized (conn.player.getRoom()) {
                Room room = conn.player.getRoom();
                if (room.hasRole(targetRole)) {
                    Protocol proto = serveNet.getProtocol().SendMsg(Protocol.ROLE_FAIL);
                    serveNet.sendToPlayer(proto, conn);
                    return;
                }
                for (Player player : room.getAllPlayer()) {
                    if (player == null)
                        continue;
                    if (player.id == thisPlayerId) {
                        player.data.role = targetRole;
                        break;
                    }
                }
                Protocol proto = serveNet.getProtocol().SendMsg(Protocol.CONNECTED);
                serveNet.handleMsg(proto, conn);
                if (canStart(room)) {
                    Protocol startProto = serveNet.getProtocol().SendMsg(
                            String.format("%s, %d", Protocol.START, room.nextPlayer()));
                    serveNet.broadcast(startProto, room);
                }
            }
        });
    }

    private void registerReConn() {
        serveNet.addEvent(Protocol.RECONNECTION, (protocol, conn) -> {
            Room room = conn.player.getRoom();
            String[] desc = protocol.getDesc().split(",");
            Player[] players = room.getAllPlayer();
            for (Player player : players) {
                if (player != null && Integer.parseInt(desc[1].trim()) == player.id) {
                    if (conn.player != null)
                        conn.player.logout();
                    player.setConn(conn);
                    conn.player = player;
                    break;
                }
            }
            Protocol proto = serveNet.getProtocol().SendMsg(Protocol.RECONNECTED);
            proto.addBytes(room.getAllMsg().getBytes());
            serveNet.sendToPlayer(protocol, conn);
            conn.sendMsg(proto);
        });
    }


    private void registerStart() {
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

    private void registerFinish() {
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

    private void registerForward() {
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

    private void registerPoor() {
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

    private void registerWarn() {
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

//    private void registerScore() {
//        serveNet.addEvent(Protocol.SCORE, (protocol, conn) -> {
//            String[] desc = protocol.getDesc().split(",");
//            int playerId = Integer.parseInt(desc[1].trim());
//            playerOff(playerId);
//            serveNet.broadcast(protocol);
//        });
//    }

    private void registerAnswer() {
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

    private void registerScholarship() {
        serveNet.addEvent(Protocol.SCHOLARSHIP, (protocol, conn) -> {
            Room room = conn.player.getRoom();
            String msg = room.getMoneyMsg();
            protocol.addBytes(msg.getBytes());
            serveNet.broadcast(protocol, room);
        });
    }

    private void registerOver() {
        serveNet.addEvent(Protocol.GAME_OVER, (protocol, conn) -> {
            Room room = conn.player.getRoom();
            String msg = room.getOverMsg();
            protocol.addBytes(msg.getBytes());
            serveNet.broadcast(protocol, room);
            room.gameInit();
        });
    }


    /**
     * @return all message about this connection's ip address, id, role, name of players will be get
     */
    private String getAllMsg(Player[] players) {
        StringBuilder sb = new StringBuilder();
        for (Player player : players) {
            if (Player.isNeed(player)) {
                sb.append(String.format(", %s, %d, %d, %s",
                        player.getSessionId(), player.id, player.data.role, player.data.name));
            }
        }
        return sb.toString();
    }

    private boolean canStart(Room room) {
        Player[] players = room.getAllPlayer();
        for (Player player : players) {
            if (!Player.isAccessible(player) || player.data.role == -1) {
                return false;
            }
        }
        return true;
    }

    private void handleJoinRoomEvent(Protocol protocol, Conn conn) {
        String[] desc = protocol.getDesc().split(",");
        int roomId = Integer.parseInt(desc[1].trim());
        Room room = RoomManager.findRoom(roomId);
        if (room == null || room.hasStarted())
            return;
        synchronized (Objects.requireNonNull(RoomManager.findRoom(roomId))) {
            Player[] players = room.getAllPlayer();
            for (int i = 0; i < players.length; i++) {
                if (!Player.isAccessible(players[i])) {
                    Player player = new Player(conn, room, desc[3].trim(), desc[2].trim());
                    if (conn.player != null)
                        conn.player.logout();
                    conn.player = player;
                    players[i] = player;
                    Protocol protoPlayer = serveNet.getProtocol().SendMsg(
                            String.format("%s, %s, %d", Protocol.PLAYER, player.getSessionId(), player.id));
                    serveNet.sendToPlayer(protoPlayer, conn);
                    Protocol protoConn = serveNet.getProtocol().SendMsg(Protocol.CONNECTED);
                    serveNet.handleMsg(protoConn, conn);
                    Protocol protoRoom = serveNet.getProtocol().SendMsg(
                            String.format("%s, %s", Protocol.ROOM_LIST, RoomManager.getRoomList()));
                    serveNet.broadcastAll(protoRoom);
                    break;
                }
            }
        }
    }

    @Override
    public void action(Conn conn) {
        registAll();
    }

    @Override
    public void fail(Conn conn) {
        afterConnectionClosed(conn);
    }

    private void afterConnectionClosed(Conn conn) {
        Player player = conn.player;
        if (player != null) {
            if (player.getRoom().hasStarted()) {
                Protocol startProto = serveNet.getProtocol().SendMsg(Protocol.START);
                serveNet.handleMsg(startProto, conn);
            } else {
                Protocol proto = serveNet.getProtocol().SendMsg(Protocol.CONNECTED);
                serveNet.handleMsg(proto, conn);
            }
        }
    }
}
