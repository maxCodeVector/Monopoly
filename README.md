# Monopoly

This is the server for game sutech-monopoly originally. However, it can be refator so It can be used for other games' server.
This is write by pure java use java thread and serverSocket. User can define there own protocol and message format.

### Usage
Use serveNet.getInstance() to get the single-ton instance. User can set port, protocol type (default is byte protocol) and the follow action.

**Example**
```java

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
