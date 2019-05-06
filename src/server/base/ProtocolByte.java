package server.base;

import server.base.ProtocolBase;

public class ProtocolByte extends ProtocolBase {
    private byte[] bytes;

    @Override
    public Protocol decode(byte[] readbuff, int start, int length) {
        ProtocolByte pro = new ProtocolByte();
        pro.bytes = new byte[length];
        return pro;
    }

    @Override
    public byte[] encode() {
        return bytes;
    }

    @Override
    public String getName() {
        return null;
    }

    @Override
    public String getDesc() {
        StringBuilder str = new StringBuilder();
        if(bytes==null)
            return str.toString();
        for (byte aByte : bytes) {
            str.append((char) aByte);
        }
        return str.toString();
    }

    @Override
    public void addBytes(byte[] bytes) {

    }


}
