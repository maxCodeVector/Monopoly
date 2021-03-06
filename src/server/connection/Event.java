package server.connection;

import server.base.Protocol;

public interface Event {

    /**
     * @param protocol the protocol that will be handled
     * @param conn special which connection that sent this protocol
     */
    void handleEvent(Protocol protocol, Conn conn);

}
