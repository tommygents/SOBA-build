using System;

public static class GameEvents
{
    // Define an event that takes an int parameter for the score
    public static event Action<int> OnEnemyKilled;

    // Method to trigger the event
    public static void EnemyKilled(int score)
    {
        OnEnemyKilled?.Invoke(score);
    }
}
