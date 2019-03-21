package base;

public class ProtocolByte extends ProtocolBase{
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
        for(int i=0;i<bytes.length;i++){
            str.append((char)bytes[i]);
        }
        return str.toString();
    }

    @Override
    public void addBytes(byte[] bytes) {

    }


}
