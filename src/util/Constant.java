package util;

import java.io.InputStream;
import java.util.Properties;
import java.util.Random;

public final class Constant {

    public enum Status{
        NORM(0, "in normal"),
        TURN(1, "in turn"),
        WEED(-1, "weed out"),
        LINE_OFF(-2, "line off");
        private int statusCode;
        private String desc;
        Status(int statusCode, String desc){
            this.statusCode = statusCode;
            this.desc = desc;
        }
    }

    public static final Random rand = new Random();
    public static final Configuration conf = new Configuration();

    public static BitMap getBitMap(int maxNum){
        return new BitMap(maxNum);
    }

    public static void main(String[] args) {
        BitMap bitMap = new BitMap(1024);
        int id;
        for (int i = 0; i < 6; i++) {
            id = rand.nextInt(63) + 1;
            while (bitMap.hasId(id)) {
                System.out.println("duplicated id:" + id);
                id = rand.nextInt(63) + 1;
            }
            bitMap.addId(id);
            System.out.println("add id:" + id + '\n');
        }
        System.out.println(bitMap);
    }

    public static class Configuration {
        public int port = 12000;
        public int maxConn = 50;
        public int heartBeat = 100000;
        public int maxPlayer = 3;
        public int yearRound = 10;
        public int maxYear = 4;

        private Properties props;
        private Configuration() {
            props = new Properties();
            try {
                InputStream in = Constant.class.getResourceAsStream("../configuration.properties");
//                new BufferedInputStream(new FileInputStream(filePath));
                props.load(in);
                port = Integer.parseInt(props.getProperty("server.port"));
                maxConn = Integer.parseInt(props.getProperty("connections"));
                heartBeat = Integer.parseInt(props.getProperty("heartbeat"));
                maxPlayer = Integer.parseInt(props.getProperty("players"));
                yearRound = Integer.parseInt(props.getProperty("year.round"));
                maxYear = Integer.parseInt(props.getProperty("total.year"));
            } catch (Exception e) {
                e.printStackTrace();
            }
        }

    }

    public static class BitMap {
        private int record[];

        private BitMap(int maxNum) {
            record = new int[(maxNum >>> 5) + 1];
        }

        public boolean hasId(int id) {
            int index = id >>> 5;
            if (index >= record.length)
                return false;
            return (record[index] & (1 << (id & 0x1F))) != 0;
        }

        public void removeId(int id) {
            int index = id >>> 5;
            if (index < record.length)
                record[index] &= ~(1 << (id & 0x1F));
        }

        public void addId(int id) {
            int index = id >>> 5;
            if (index < record.length)
                record[index] |= (1 << (id & 0x1F));
        }

        public String toString() {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < record.length; i++) {
                int re = record[i];
                int bias = 0;
                while (re != 0) {
                    if ((re & 0x1) == 1) {
                        sb.append((i << 5) + bias);
                        sb.append(',');
                    }
                    bias++;
                    re >>>= 1;
                }
            }
            return sb.toString();
        }

    }




}
