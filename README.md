# Monopoly

This is the server for game `Sutech-Monopoly` originally. However, it can be refator so It can be used for other games' server. This work is try to make it to a framework for those round games that serveral player in a Room.
This is written by pure java use java thread and serverSocket. User can define there own protocol and message format.

### Usage
Use serveNet.getInstance() to get the single-ton instance. Then you can set port, protocol type (default is byte protocol) and the following action. It also have default value which are defined in a configuration file. It use server.connection.LaunchServer to lauch a server application.

**Example**
```java
package server.logic;

import server.base.ServerApplication;
import server.connection.LaunchServer;
import server.connection.ServeNet;

@ServerApplication
public class MonoServer {

    public static void main(String args[]){
        ServeNet serveNet = ServeNet.getInstance();
        serveNet.setPort(12000);  // set port, also can set it in configuration file.
        /*the above two line can be omit then it will just use the default value */
        LaunchServer.launch(MonoServer.class, args);
    }

}
```

```java
package server.logic;

import server.base.Action;
import server.base.Protocol;
import server.connection.Conn;
import server.connection.Event;


/* By doing this, when client send `Hello, xxxxxx`, 
 * server will invoke the handleEvent() method
 */
@Action("Hello")
public class TestAction implements Event {
    @Override
    public void handleEvent(Protocol protocol, Conn conn) {
        System.out.println("Hello, this is my test action");
        conn.sendMsg(protocol.SendMsg("Hello, I have receive your message\n"));
    }
}
```



### Protocol
It has defined 3 types of protocol (String, byte, number).


### Inteface: 
- `AfterDoingThis`

```java
void action(ServeNet, serveNet);
void fail(Conn conn);
```

- `Event`
```java
void HandleEvent(Protocol protocol, Conn conn);
```

### Annotation
- `@ServerApplication`:
to launch a server, if the main class does not have this annotaion, Lancher will do nothing.

- `@Action`:
Use it to register some action automatically. So it's easy to add more logic in server (The register class need to implement interface `AfterDoingThis` or `Event`). The class with annotaion `@Action` need to set in the same pakage with Main class.

### Acknowledge

My partner.
