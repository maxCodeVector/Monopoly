# Monopoly

This is the server for game `Sutech-Monopoly` originally. However, it can be refator so it can be used for other types of server. This work is try to make it to a framework for those round games that serveral player in a Room.
This is written by pure java use java thread and serverSocket. User can define there own protocol and message format.

### How to use it?

#### Client

To install this game, go to repository [SustechMonopoly](https://github.com/Instein98/SustechMonopoly/releases/tag/V1.0.0) to download it in release page. It support windows and android.

#### Server

If you want to run this server for the game `sustech-monopoly`, you can directly run compile.sh to compile it with one argument referring the class file output dirctory.
```
./build.sh [class dir]
```
And run start.sh to run it.
```
./start.sh [class dir]
```

Then ther server will start running and listen to the requests in port 12000 (you could change the port number in server.logic.MonoServer. As seen below).

### Extend Usage

This part tell you how to extend this server by adding more function of game or even building another type of server(may be file downloader, I hope so).

- Use serveNet.getInstance() to get the single-ton instance. 
Then you can set port, protocol type (default is byte protocol) and the following action. It also have default value which are defined in a configuration file. 
- Use server.connection.LaunchServer to lauch a server application.

**Quik Start**
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

### Todo
This server is not a save server alought it need to send heart beat to verify connection live and wrong message type will cause connection broken. However, until now, it use string to translate information. So there are something to do to improve it.
- 1. Implement the byte protocol, more savely as well as more small pakage need to transfer.
- 2. Currently, it only support 50 connection(may be more, but it is a constant). We could use asynio to replace the multithreading part (eg, netty).
- 3. Room function was hide in the low level code, it is not good either, could be abstract similar to the action event.



### Acknowledge

My partner.
