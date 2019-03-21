package base;

import model.Player;
import model.PlayerData;
import util.DBUtil;

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
