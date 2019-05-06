package logic;

import base.Protocol;
import model.Player;
import util.Constant;
import util.DEUtil;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.util.Date;

public class Conn implements Runnable {

    private static final int BUFFER_SIZE = 1024;
    private static final int MAX_HEART = Constant.conf.heartBeat;
    //Socket
    private Socket socket;

    private boolean isUse = false;
    //Buff
    private byte[] readBuff;
    private int buffCount = 0;
    //store the package length
    private byte[] lenBytes = new byte[4];
    private int msgLength = 0;

    private long lastTickTime = Long.MIN_VALUE;

    public Player player;

    Conn() {
        readBuff = new byte[BUFFER_SIZE];
    }


    void init(Socket socket) {
        this.socket = socket;
        isUse = true;
        buffCount = 0;
        lastTickTime = System.currentTimeMillis();
    }

    private int buffRemain() {
        return BUFFER_SIZE - buffCount;
    }

    String getAddress() {
        if (!isUse)
            return "can not gain address!";
        return socket.getRemoteSocketAddress().toString();
    }

    /**
     * @return if this connection has been used ,return true, otherwise false;
     */
    public boolean isUse() {
        return isUse;
    }

    private void close() {
        try {
            if (!isUse)
                return;
            if (player != null) {
                player.logout();
            }
            socket.close();
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            System.out.println("no heart, try close:" + getAddress());
            isUse = false;
            ServeNet servNet = ServeNet.getInstance();
            if (player != null) {
                if (player.getRoom().hasStarted()) {
                    Protocol startProto = servNet.getProtocol().SendMsg(Protocol.START);
                    servNet.handleMsg(startProto, this);
                } else {
                    Protocol proto = servNet.getProtocol().SendMsg(Protocol.CONNECTED);
                    servNet.handleMsg(proto, this);
                }
            }
        }
    }

    @Override
    public void run() {
        try {
            if (!getAddress().startsWith(Protocol.RootIP))
                socket.setSoTimeout(MAX_HEART);
            InputStream is = socket.getInputStream(); // to read data
            while (true) {
                int len = is.read(lenBytes);
                msgLength = DEUtil.byteArrayToInt(lenBytes);
                if (msgLength > 1024 || len < 4) {
                    return;
                }
                while (msgLength - buffCount > 0) {
                    if (!getAddress().startsWith(Protocol.RootIP)
                            && System.currentTimeMillis() - this.lastTickTime > MAX_HEART) {
                        System.out.println(new Date() + " " + getAddress() + " is not live");
                        return;
                    }
                    int length = Math.min(msgLength - buffCount, buffRemain());
                    int n = is.read(readBuff, buffCount, length);
                    buffCount += n;
                }
                processData();
                buffCount = 0;
            }
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            close();
        }
    }

    private void processData() {
        ServeNet servNet = ServeNet.getInstance();
        Protocol protocol = servNet.getProtocol().decode(readBuff, 0, msgLength);
        if (protocol.getName().equals(Protocol.HEART_BEAT)) {
            this.lastTickTime = System.currentTimeMillis();
//            System.out.println(new Date() + " receive heart beat:" + getAddress());
//            if(Player.isAccessible(player) && player.state == Constant.Status.TURN){
//                playerTimer++;
//                if(playerTimer==3){
//                    Protocol startProto = servNet.getProtocol().SendMsg(Protocol.START);
//                    servNet.handleMsg(startProto, this);
//                    playerTimer = 0;
//                }
//            }else
//                playerTimer = 0;
        } else {
            System.out.println("RECV:" + getAddress() + " " + protocol.getDesc());
            servNet.handleMsg(protocol, this);
        }
    }

    void sendMsg(Protocol protocol) {
        try {
            OutputStream os = socket.getOutputStream(); // to send data
            System.out.printf("%s response to:%s %s\n", new Date(), getAddress(), protocol.getDesc());
            os.write(protocol.encode());
        } catch (IOException e) {
            close();
            e.printStackTrace();
        }
    }

}
