package server.model;

import server.connection.Conn;
import server.util.Constant;

public class Player {

    public static Constant.BitMap idPool = Constant.getBitMap(1024);

    public int id;
    private Conn conn;
    private String sessionId;
    private Room room;


    public Constant.Status state;
    public PlayerData data;
    PlayerTempData tempData;

    public Player(Conn conn, Room room, String name, String sessionId) {
        int id = Constant.rand.nextInt(1023) + 1;
        while (idPool.hasId(id))
            id = Constant.rand.nextInt(1023) + 1;
        idPool.addId(id);
        this.id = id;
        this.sessionId = sessionId;
        this.conn = conn;
        this.room = room;
        tempData = new PlayerTempData();
        data = new PlayerData(name);
        state = Constant.Status.NORM;
    }

    public void setConn(Conn conn) {
        this.conn = conn;
    }

    public Conn getConnection() {
        return conn;
    }

    public Room getRoom() {
        return room;
    }

    public void logout() {
        this.conn = null;
        idPool.removeId(id);
        state = Constant.Status.LINE_OFF;
        RoomManager.adjustRoomList(room.id);
    }

    /**
     * @param player the player that need to judge
     * @return if this player can be include (connection is accessible & does not line off or wed out)
     */
    public static boolean isNeed(Player player) {
        return isAccessible(player) &&
                (player.state == Constant.Status.NORM || player.state == Constant.Status.TURN);
    }

    /**
     * @param player the player that need to judge
     * @return if this player's connection is accessible
     */
    public static boolean isAccessible(Player player) {
        return player != null && player.getConnection() != null && player.getConnection().isUse();
    }

    public void setTempData(int money, int healthy, float GPA) {
        this.tempData.setAttribute(money, healthy, GPA);
    }

    double getScore() {
        return this.tempData.getScore();
    }

    public String getSessionId() {
        return sessionId;
    }


}
