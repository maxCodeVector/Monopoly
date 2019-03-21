package base;

import model.Player;
import model.PlayerData;
import util.DBUtil;

import java.sql.*;


public abstract class DataMgr {

    private boolean isSafeStr(String str) {
        //to be continue
        return true;
    }

    //是否存在该用户, if exist, return 1, else return 0, if id is invalid or has exception, return -1
    private int CanRegister(String id) {
        //防sql注入
        Connection conn = null;
        PreparedStatement cmd = null;
        ResultSet res = null;
        if (!isSafeStr(id))
            return -1;
        //查询id是否存在
        String cmdStr = "select * from user where id=?;";
        try {
            conn = DBUtil.getConnection();
            cmd = conn.prepareStatement(cmdStr);
            cmd.setString(1, id);
            res = cmd.executeQuery();
            return res.next() ? 1 : 0;
        } catch (Exception e) {
            return -1;
        } finally {
            try {
                DBUtil.closeDBResource(conn, cmd, res);
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    public boolean Register(String id, String pw) {
        //防sql注入
        if (!isSafeStr(id) || !isSafeStr(pw)) {
            return false;
        }
        //能否注册
        if (CanRegister(id) != 1) {
            System.out.println("[DataMgr]Register !CanRegister");
            return false;
        }
        //写入数据库User表
        String cmdStr = "insert into user set id =? ,pw =?;";
        Connection conn = null;
        PreparedStatement cmd = null;
        ResultSet res = null;
        try {
            conn = DBUtil.getConnection();
            cmd = conn.prepareStatement(cmdStr);
            if (cmd.executeUpdate() == 1)
                return true;
        } catch (Exception e) {
            e.printStackTrace();
        }
        return false;
    }

    //创建角色
    public abstract boolean createPlayer(String id);

    //检测用户名密码
    public abstract boolean checkPasaword(String id, String pw);

    //获取玩家数据
    public abstract PlayerData GetPlayerData(String id);

    //保存角色
    public abstract boolean savePlayer(Player player);


}

