package model;

public class PlayerTempData {
    private int money;
    private int healthy;
    private float GPA;

    void setAttribute(int money, int healthy, float GPA){
        this.money = money;
        this.healthy = healthy;
        this.GPA = GPA;
    }

    public String toString(){
        return String.format("%d, %d, %.2f", money, healthy, GPA);
    }

    double getScore(){
        return 0.1 * money + 10 * healthy + 300 * GPA;
    }

}
