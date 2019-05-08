package server.logic;

import server.base.Action;
import server.base.Protocol;
import server.connection.AfterDoingThis;
import server.connection.Conn;
import server.connection.ServeNet;
import server.model.Player;
import server.model.Room;
import server.model.RoomManager;

import java.util.Objects;

@Action
public class RegisterProtocol implements AfterDoingThis {

    private void registAll(ServeNet serveNet) {
        serveNet.addEvent(Protocol.CONNECTED, (protocol, conn) -> {
            Room room = conn.player.getRoom();
            Player[] players = room.getAllPlayer();
            protocol.addBytes(String.format(
                    ", %d%s", room.id, getAllMsg(players)).getBytes());
            serveNet.broadcast(protocol, room);
        });
        registerConn(serveNet);
        registerCreateRoom(serveNet);
        registerJoinRoom(serveNet);
        registerLeaveRoom(serveNet);

        registerRole(serveNet);
        registerReConn(serveNet);
    }

    /**
     * deal with CONNECTION request, give id and others
     *
     * @param serveNet
     */
    private void registerConn(ServeNet serveNet) {
        serveNet.addEvent(Protocol.CONNECTION, (protocol, conn) -> {
            Protocol proto = serveNet.getProtocol().SendMsg(
                    String.format("%s, %s", Protocol.ROOM_LIST, RoomManager.getRoomList()));
            serveNet.sendToPlayer(proto, conn);
        });
    }

    private void registerCreateRoom(ServeNet serveNet) {
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

    private void registerJoinRoom(ServeNet serveNet) {
        serveNet.addEvent(Protocol.JOIN_ROOM, (protocol, conn) -> {
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
        });
    }

    private void registerLeaveRoom(ServeNet serveNet) {
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

    private void registerRole(ServeNet serveNet) {
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

    private void registerReConn(ServeNet serveNet) {
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

    public void action(ServeNet serveNet) {
        registAll(serveNet);
    }

    public void fail(Conn conn) {
        ServeNet serveNet = ServeNet.getInstance();
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
