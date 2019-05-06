package server.base;

import server.model.Player;
import server.model.PlayerData;
import server.util.DBUtil;

public class DataMgrImp extends DataMgr {
    @Override
    public boolean createPlayer(String id) {
        return false;
    }

    @Override
    public boolean checkPasaword(String id, String pw) {
        return false;
    }

    @Override
    public PlayerData GetPlayerData(String id) {
        return DBUtil.getPlayerData(id);
    }

    @Override
    public boolean savePlayer(Player player) {
        DBUtil.savePlayer(player.data);
        return true;
    }
}
