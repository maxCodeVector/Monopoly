package server.client;

import server.base.Protocol;
import server.util.Constant;
import server.util.DEUtil;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.nio.charset.StandardCharsets;
import java.util.Date;
import java.util.Scanner;

public class VIPClient {
    private static final int BUFFER_SIZE = 1024;
    //Buff
    private static byte[] readBuff = new byte[1024];
    private static int buffCount = 0;
    //store the package length
    private static byte[] lenBytes = new byte[4];

    private static int buffRemain() {
        return BUFFER_SIZE - buffCount;
    }

    private static void run(InputStream is) {
        try {
            while (true) {
                int len = is.read(lenBytes);
                int msgLength = DEUtil.byteArrayToInt(lenBytes);
                if (msgLength > 1024 || len < 4) {
                    return;
                }
                while (msgLength - buffCount > 0) {
                    int length = Math.min(msgLength - buffCount, buffRemain());
                    int n = is.read(readBuff, buffCount, length);
                    buffCount += n;
                }
                System.out.println(new String(readBuff, 0, msgLength));
                buffCount = 0;
            }
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            try {
                is.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
    }

    private static void sendHeart(OutputStream os) {
        long lastTickTime = System.currentTimeMillis();
        byte[] pak = Protocol.HEART_BEAT.getBytes(StandardCharsets.UTF_8);
        byte[] len = DEUtil.intToByteArray(pak.length);
        int heartSeg = Constant.conf.heartBeat >> 1;
        try {
            while (true) {
                if(System.currentTimeMillis() - lastTickTime > heartSeg) {
                    os.write(len);
                    os.write(pak);
                    lastTickTime = System.currentTimeMillis();
                }
                Thread.sleep(2000);
            }
        } catch (IOException | InterruptedException e) {
            e.printStackTrace();
        } finally {
            try {
                os.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
    }

    public static void main(String[] args) throws IOException {
        System.out.println("I am a client " + new Date());
        Socket client;
        if (args.length == 2) {
            client = new Socket(args[0], Integer.parseInt(args[1]));
            new Thread(() -> {
                try {
                    sendHeart(client.getOutputStream());
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }).start();
            System.out.println("connected to " + args[0] + " " + args[1]);
        } else
            client = new Socket("127.0.0.1", 12000);
        OutputStream os = client.getOutputStream();
        InputStream is = client.getInputStream();
        new Thread(() -> {
            run(is);
        }).start();
        Scanner input = new Scanner(System.in);
        while (input.hasNext()) {
            String data = input.nextLine();
            byte[] pak = data.getBytes(StandardCharsets.UTF_8);
            os.write(DEUtil.intToByteArray(pak.length));
            System.out.println("msglen: " + pak.length);
            os.write(pak);
        }
        client.close();
    }

}
