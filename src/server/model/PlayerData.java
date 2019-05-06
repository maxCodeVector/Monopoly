package server.model;

import java.io.Serializable;

public class PlayerData implements Serializable {
    public int role;
    private float x;
    private float y;
    private float z;
    public String name;

    PlayerData(String name){
        this(name, -1);
    }

    private PlayerData(String name, int role){
        this.name = name;
        this.role = role;
    }

    public void setPos(float x, float y, float z){
        this.x = x;
        this.y = y;
        this.z = z;
    }

    @Override
    public String toString(){
        return String.format("%d, %s, %f, %f, %f", role, name, x, y, z);
    }

}
