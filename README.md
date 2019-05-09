# Monopoly

This is the server for game sutech-monopoly originally. However, it can be refator so It can be used for other games' server.
This is write by pure java use java thread and serverSocket. User can define there own protocol and message format.

### Usage
Use serveNet.getInstance() to get the single-ton instance. User can set port, protocol type (default is byte protocol) and the follow action.

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
        serveNet.setPort(12000);
        LaunchServer.launch(MonoServer.class, args);
    }

}
```

### Protocol
It has defined 4 types of protocol (String, byte, number).


### Inteface: `AfterDoingThis`

```java
void action(Conn conn);
void fail(Conn conn);
```

### Notation
It use Notation `@ServerApplication` to launch a server, use @Action can register some action (The register class need to implement interface AfterDoingThis) automatically. So it's easy to add more logic in server.

### Acknowledge

My partner.
