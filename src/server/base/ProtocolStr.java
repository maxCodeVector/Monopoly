package server.base;

public class ProtocolStr extends ProtocolBase {

    @Override
    public Protocol decode(byte[] readbuff, int start, int length) {
        return null;
    }

    @Override
    public byte[] encode() {
        return new byte[0];
    }

    @Override
    public String getName() {
        return null;
    }

    @Override
    public String getDesc() {
        return null;
    }

    @Override
    public void addBytes(byte[] bytes) {

    }
}
