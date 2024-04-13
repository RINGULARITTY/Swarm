using System;

public class StatsComponent {
    public int Health { get; set; }
    public int Damage { get; set; }

    public StatsComponent(int health, int damage) {
        Health = health;
        Damage = damage;
    }
}