package server.base;

import server.util.DEUtil;

import java.util.Arrays;

public class ProtocolBase implements Protocol {

    private byte[] bytes;

    @Override
    public Protocol decode(byte[] readbuff, int start, int length) {
        ProtocolBase proto = new ProtocolBase();
        byte[] len = DEUtil.intToByteArray(length);
        byte[] pak = Arrays.copyOf(len, len.length + length);
        if (length >= 0) System.arraycopy(readbuff, start, pak, len.length, length);
        proto.bytes = pak;
        return proto;
    }

    @Override
    public void addBytes(byte[] bytes) {
        this.bytes = DEUtil.concat(this.bytes, bytes);
    }

    @Override
    public Protocol SendMsg(String msg) {
        byte[] pkt = msg.getBytes();
        return this.decode(pkt, 0, pkt.length);
    }

    @Override
    public byte[] encode() {
        int i = bytes.length - 4;
        bytes[0] = (byte)((i >> 24) & 0xFF);
        bytes[1] = (byte)((i >> 16) & 0xFF);
        bytes[2] = (byte)((i >> 8) & 0xFF);
        bytes[3] = (byte)(i & 0xFF);
        return bytes;
    }

    @Override
    public String getName() {
        return getDesc().split(",")[0].trim();
    }

    @Override
    public String getDesc() {
        return new String(bytes, 4, bytes.length - 4);
    }


}
