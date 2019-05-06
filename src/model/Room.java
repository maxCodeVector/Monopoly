package model;

import util.Constant;

import java.util.Comparator;
import java.util.LinkedList;

public class Room {

    public final static int MAX_SCORE = 5;
    private final static int[] RANK = {4000, 3000, 2000, 1000};

    static Constant.BitMap idPool = Constant.getBitMap(1024);
    private static Comparator<ScoreDesc> myCompa = (o1, o2) -> Float.compare(o2.score, o1.score);

    public int id;
    String name;
    private Player[] allPlayer;
    private int order = -1;
    private int year = 0;
    private int round;
    private boolean query = false;
    private int[] record;

    public Player[] getAllPlayer() {
        return allPlayer;
    }

    Room(String name) {
        int id = Constant.rand.nextInt(1023) + 1;
        while (idPool.hasId(id))
            id = Constant.rand.nextInt(1023) + 1;
        idPool.addId(id);
        this.id = id;
        this.name = name;
        allPlayer = new Player[Constant.conf.maxPlayer];
        record = new int[Constant.conf.maxPlayer];
    }

    public String getAllMsg() {
        StringBuilder sb = new StringBuilder();
        for (Player p : this.allPlayer) {
            if (Player.isNeed(p))
                sb.append(String.format(", %d, %s", p.id, p.data.toString()));
        }
        return sb.toString();
    }

    private class ScoreDesc {
        float score;
        String desc;

        ScoreDesc(float score, String desc) {
            this.score = score;
            this.desc = desc;
        }
    }

    public String getOverMsg() {
        LinkedList<ScoreDesc> scoreDiscs = new LinkedList<>();
        for (Player anPlayer : allPlayer) {
            if (Player.isNeed(anPlayer)) {
                float score = (float) anPlayer.getScore();
                scoreDiscs.add(new ScoreDesc(score, String.format(
                        "%d, %d, %s", anPlayer.id, (int) score, anPlayer.tempData.toString())));
            } else {
                scoreDiscs.add(new ScoreDesc(0.0f, String.format(
                        "%d, %d, %s", anPlayer.id, 0, anPlayer.tempData.toString())));
            }
        }
        scoreDiscs.sort(myCompa);
        StringBuilder sb = new StringBuilder();
        for (ScoreDesc scoreDesc : scoreDiscs) {
            sb.append(", ").append(scoreDesc.desc);
        }
        return sb.toString();
    }

    public String getMoneyMsg() {
        LinkedList<ScoreDesc> scoreList = new LinkedList<>();
        for (Player anPlayer : allPlayer) {
            if (Player.isNeed(anPlayer)) {
                float score = (float) anPlayer.getScore();
                scoreList.add(new ScoreDesc(score, String.valueOf(anPlayer.id)));
            } else {
                scoreList.add(new ScoreDesc(0.0f, String.valueOf(anPlayer.id)));
            }
        }
        scoreList.sort(myCompa);
        StringBuilder sb = new StringBuilder();
        for (int rankNum = 0;
             rankNum < scoreList.size() && rankNum < RANK.length;
             rankNum++) {
            sb.append(String.format(", %s, %d", scoreList.get(rankNum).desc, RANK[rankNum]));
        }
        return sb.toString();
    }

    /**
     * @return STATE CODE: -1: doesn't have any player, game over <br>
     * -2: attain max year or only has one player left game over <br>
     * others: the next player turn
     */
    public int nextPlayer() {
        if (order != -1 && allPlayer[order] != null && allPlayer[order].state == Constant.Status.TURN)
            this.allPlayer[order].state = Constant.Status.NORM;
        switch (getLivePlayer()) {
            case 0:
                return -1;
            case 1:
                return -2;
        }
        for (int i = 0; i < allPlayer.length; i++) {
            order++;
            if (order >= allPlayer.length) {
                order = 0;
                round += 1;
            }
            if (round > Constant.conf.yearRound) {
                year += 1;
                this.query = true;
                round = 0;
            }
            if (year > Constant.conf.maxYear) {
                return -2;
            }
            if (Player.isNeed(allPlayer[order])) {
                allPlayer[order].state = Constant.Status.TURN;
                return allPlayer[order].id;
            }
        }
        return -1;
    }

    public boolean isOver() {
        return year >= Constant.conf.maxYear;
    }

    public int currentPlayer() {
        if (hasStarted())
            return this.allPlayer[order].id;
        else
            return this.allPlayer[0].id;
    }

    public boolean hasStarted() {
        return order != -1 && !isOver();
    }

    public boolean hasRole(int role) {
        for (Player p : this.allPlayer) {
            if (Player.isNeed(p) && p.data.role == role)
                return true;
        }
        return false;
    }

    public int getQueryOrder() {
        if (this.query) {
            this.query = false;
            return year;
        }
        return 0;
    }

    public void gameInit() {
        for (int i = 0; i < allPlayer.length; i++)
            allPlayer[i] = null;
        RoomManager.adjustRoomList(id);
    }

    private int getLivePlayer() {
        int livePlayer = 0;
        for (Player p : this.allPlayer) {
            if (Player.isNeed(p))
                livePlayer++;
        }
        return livePlayer;
    }

    int getConnPlayer() {
        int livePlayer = 0;
        for (Player p : this.allPlayer) {
            if (Player.isAccessible(p))
                livePlayer++;
        }
        return livePlayer;
    }

    public void setAnswered(int i, int queryOrder) {
        this.record[i] = queryOrder;
    }

    public boolean hasAnswer(int queryOrder) {
        for (int i = 0; i < record.length; i++) {
            if (Player.isNeed(allPlayer[i]) && queryOrder != record[i])
                return false;
        }
        return true;
    }


}
