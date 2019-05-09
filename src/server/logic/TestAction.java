package server.logic;

import server.base.Action;
import server.base.Protocol;
import server.connection.Conn;
import server.connection.Event;

@Action("Hello")
public class TestAction implements Event {
    @Override
    public void handleEvent(Protocol protocol, Conn conn) {
        System.out.println("Hello, this is my test action");
        conn.sendMsg(protocol.SendMsg("Hello, I have receive your message\n"));
    }
}
