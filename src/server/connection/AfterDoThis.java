package server.connection;

public interface AfterDoThis {
    void action(Conn conn);
    void fail(Conn conn);
}
