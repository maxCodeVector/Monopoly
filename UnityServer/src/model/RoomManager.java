package model;

import java.util.HashMap;

public class RoomManager {

    private static HashMap<Integer, Room> roomList = new HashMap<>();

    static void adjustRoomList(int roomId) {
        Room room = RoomManager.findRoom(roomId);
        if (room == null)
            return;
        if (room.getConnPlayer() == 0) {
            roomList.remove(roomId);
            Room.idPool.removeId(roomId);
        }
    }

    public static Room createRoom(String name) {
        Room room = new Room(name);
        roomList.put(room.id, room);
        return room;
    }

    public static Room findRoom(int id) {
        if (roomList.containsKey(id)) {
            return roomList.get(id);
        }
        return null;
    }

    public static String getRoomList() {
        StringBuilder sb = new StringBuilder();
        sb.append(roomList.size());
        for (Room room : roomList.values()) {
            int numPlayer;
            if (room.hasStarted())
                numPlayer = 3;
            else {
                numPlayer = room.getConnPlayer();
            }
            sb.append(String.format(", %d, %s, %d", room.id, room.name, numPlayer));
        }
        return sb.toString();
    }
}