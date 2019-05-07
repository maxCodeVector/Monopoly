# Monopoly

This is the server for game sutech-monopoly originally. However, it can be refator so It can be used for other games' server.
This is write by pure java use java thread and serverSocket. User can define there own protocol and message format.

### Usage
Use serveNet.getInstance() to get the single-ton instance. User can set port, protocol type (default is byte protocol) and the follow action.

**Example**
```java
package server.logic;

import server.connection.AfterDoingThis;
import server.connection.ServeNet;

public class MonoServer {

    public static void main(String args[]){
        ServeNet serveNet = ServeNet.getInstance();
        serveNet.setPort(12000);   // user can only set port once unless throw exception
        AfterDoingThis afterAction = new RegisterProtocol(serveNet);  // implement inteface AfterDoingThis
        serveNet.afterInstance(afterAction);    // will invoke action() immdiately usually used for register new protocol.
        serveNet.setAfterDoThis(afterAction);   // will invoke fail() when connection closed.
        serveNet.start();   // server start running
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

### Acknowledge

My partner.
