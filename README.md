# Monopoly

This is the server for game sutech-monopoly originally. However, it can be refator so It can be used for other games' server.
This is write by pure java use java thread and serverSocket. User can define there own protocol and message format.

### Usage
Use serveNet.getInstance() to get the single-ton instance. User can set port, protocol type (default is byte protocol) and the following action.

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
        LaunchServer.launch(MonoServer.class, args);
    }

}
```

### Protocol
It has defined 3 types of protocol (String, byte, number).


### Inteface: 
- `AfterDoingThis`

```java
void action(Conn conn);
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
Use it to register some action automatically. So it's easy to add more logic in server (The register class need to implement interface `AfterDoingThis` or `Event`).

### Acknowledge

My partner.
