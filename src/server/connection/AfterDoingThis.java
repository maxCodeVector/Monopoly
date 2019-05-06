package server.connection;

public interface AfterDoingThis {
    void action(Conn conn);
    void fail(Conn conn);
}
