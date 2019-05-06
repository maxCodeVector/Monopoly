package base;

/**
* Those below are some name about protocol and details about it along with method.
*/
public interface Protocol {

    /**
     * format: CONNECTION
     * send by client to request to create CONNECTION to server <br>
     */
    String CONNECTION = "CONNECTION";

    /**
     * format: ROOMLIST, [#room], [room id], [room name], [#player],
     *                            [room id], [room name], [#player],
     *                            [room id], [room name], [#player],
     *                              ... <br>
     * send by server to send room list for this connection or all connection <br>
     */
    String ROOM_LIST = "ROOMLIST";

    /**
     * format: CREATEROOM, [room name], [session id], [player name] <br>
     * send by client to create a room with specializing the name <br>
     */
    String CREATE_ROOM = "CREATEROOM";

    /**
     * format: JOINROOM, [room id], [session id], [player name] <br>
     * send by client to join a room <br>
     */
    String JOIN_ROOM = "JOINROOM";

    /**
     * format: CONNECTED, [room id], [session Id], [player id], [role], [name], <br>
     *                               [session Id], [player id], [role], [name], <br>
     *                               [session Id], [player id], [role], [name], <br>
     *                               ... <br>
     * send by server to reply JOINROOM or CREARTEROOM message and
     * it will be broad cast to all player in this room <br>
     */
    String CONNECTED = "CONNECTED";

    /**
     * format: PLAYER, [session Id], [player id] <br>
     * send by server to reply JOINROOM or CREARTEROOM message and
     * it will only be send to this connection, session id is used to authorities <br>
     */
    String PLAYER = "PLAYER";

    /**
     * format: LEAVEROOM, [room id], [player id] <br>
     * send by client to tell server it leaves this room <br>
     */
    String LEAVE_ROOM = "LEAVEROOM";

    /**
     * request all other player's information from client <br>
     * format: RECONNECTION, [id], [role], [name]
     */
    String RECONNECTION = "RECONNECTION";

    /**
     * reply for "RECONNECTION" from server <br>
     * format: RECONNECTED, [id], [role], [name], [x], [y], [z], <br>
     *                      [id], [role], [name], [x], [y], [z], <br>
     *                      [id], [role], [name], [x], [y], [z], <br>
     *                      ... <br>
     */
    String RECONNECTED = "RECONNECTED";

    /**
     * request for get role from client if the client successfully choose this role,
     * server would broad cast "CONNECTED" protocol again <br>
     * format: ROLE, [id], [role] <br>
     */
    String ROLE = "ROLE";

    /**
     * reply for get this role from server, send to this player where means it failed to choose this role <br>
     * format: ROLEFAIL <br>
     */
    String ROLE_FAIL = "ROLEFAIL";

    /**
     * send by server, tells player which has this id can start playing game in this round <br>
     * format: START, [id] <br>
     */
    String START = "START";

    /**
     * send by client, means this player is finished i's round <br>
     * format: FINISHED, [id] <br>
     */
    String FINISHED = "FINISHED";

    /**
     * sent by clients, and broad casted (and saved) by server. <br>
     * format: FORWARD, [id], [x], [z] (hint: y is a constant since they are in a same plane) <br>
     */
    String FORWARD = "FORWARD";

    /**
     * format: QUERY, [o]
     * <br>send by server to query all information of one player</br>
     * <br>[o] is the order of query, there are 4's query in total, last is to know the winner, </br>
     * <br>others is to distribute money</br>
     */
    String QUERY = "QUERY";

    /**
     * format: ANSWER, [o], [id], [money], [healthy], [GPA], [score] <br>
     * send by client to give all information of it to server <br>
     */
    String ANSWER = "ANSWER";

    /**
     * send by server to tell all players that game is finished and the result <br>
     * format: GAMEOVER, [id], [total], [money], [healthy], [GPA] <br>
     *                  [id], [total], [money], [healthy], [GPA] <br>
     *                  [id], [total], [money], [healthy], [GPA] <br>
     *                  ... <br>
     */
    String GAME_OVER = "GAMEOVER";

    /**
     * POOR, [id] <br>
     */
    String POOR = "POOR";

    /**
     * let this player to know he has low SCORE <br>
     * WARN, [id], [num], [detail] <br>
     */
    String WARN = "WARN";

    /**
     * SCORE, [id] <br>
     */
    String SCORE = "SCORE";

    /**
     * SCHOLARSHIP, [id], [money] <br>
     *              [id], [money] <br>
     *              [id], [money] <br>
     *              ... <br>
     */
    String SCHOLARSHIP = "SCHOLARSHIP";

    String RootIP = "/127.0.0.1";
    String HEART_BEAT = "HeatBeat";

    /**
     * return a new Protocol that have the special message
     * @param readbuff the byte array which stores message
     * @param start the position to start recording
     * @param length the message length
     */
    Protocol decode(byte[] readbuff, int start, int length);

    /**
     * @return a byte array which is encoded form msg along with a length header
     */
    byte[] encode();

    /**
     * @return protocol type, the type is defined in interface
     */
    String getName();

    /**
     * @return raw data from message
     */
    String getDesc();

    /**
     * add some bytes to tail of this protocol message.
     */
    void addBytes(byte[] bytes);

    /**
     * @return a Protocol that has information about msg.
     */
    Protocol SendMsg(String msg);

}
