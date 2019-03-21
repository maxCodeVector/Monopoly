package util;

import model.PlayerData;
import java.io.*;
import java.sql.*;

public class DBUtil {
    private static Connection connection = null;

    public static Connection getConnection() throws Exception {
        String diverClass = "com.mysql.jdbc.Driver";
        String url = "jdbc:mysql://localhost:3306/monoply";
        String name = "root";
        String password = "11610303";
        Class.forName(diverClass);
        connection = DriverManager.getConnection(url, name, password);
        return connection;
    }

    public static void closeDBResource(Connection connection, PreparedStatement preparedStatement,
                                       ResultSet resultSet) throws Exception {
        if (resultSet != null) {
            try {
                resultSet.close();
            } finally {
                resultSet = null;
            }
        }
        if (preparedStatement != null) {
            try {
                preparedStatement.close();
            } finally {
                preparedStatement = null;
            }
        }
        if (connection != null) {
            try {
                connection.close();
            } finally {
                connection = null;
            }
        }
    }



    public static void savePlayer(PlayerData data) {
        String sql = "insert into player(data) values(?)";
        try {
            PreparedStatement pres = connection.prepareStatement(sql);
            pres.setObject(1, data);
            pres.executeBatch();
            if (pres != null)
                pres.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }


    public static PlayerData getPlayerData(String id) {
        PlayerData data = null;
        String sql = "select data from player where id=?";
        try {
            PreparedStatement pres = connection.prepareStatement(sql);
            ResultSet res = pres.executeQuery();
            while (res.next()) {
                Blob inBlob = res.getBlob(1);
                InputStream is = inBlob.getBinaryStream();
                BufferedInputStream bis = new BufferedInputStream(is);
                byte[] buff = new byte[(int) inBlob.length()];
                while (-1 != (bis.read(buff, 0, buff.length))) {
                    ObjectInputStream in = new ObjectInputStream(new ByteArrayInputStream(buff));
                    data = (PlayerData) in.readObject();
                }
            }
        } catch (SQLException | IOException | ClassNotFoundException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        return data;
    }


}