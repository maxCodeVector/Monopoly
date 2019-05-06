
package util;

import java.util.Arrays;

public class DEUtil {

    public static byte[] concat(byte[] first, byte[] second) {
        if(first==null)
            return second;
        byte[] result = Arrays.copyOf(first, first.length + second.length);
        System.arraycopy(second, 0, result, first.length, second.length);
        return result;
    }

    /**
     * int to byte[]
     * @param i
     * @return
     */
    public static byte[] intToByteArray(int i) {
        byte[] result = new byte[4];

        result[0] = (byte)((i >> 24) & 0xFF);
        result[1] = (byte)((i >> 16) & 0xFF);
        result[2] = (byte)((i >> 8) & 0xFF);
        result[3] = (byte)(i & 0xFF);
        return result;
    }

    /**
     * byte[]  to int
     * @param bytes
     * @return
     */
    public static int byteArrayToInt(byte[] bytes) {
        int value=0;

        for(int i = 0; i < 4; i++) {
            int shift= (4-1-i) * 8;
            value +=(bytes[i] & 0x000000FF) << shift;
        }
        return value;
    }

}
