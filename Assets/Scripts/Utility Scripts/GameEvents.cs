using System;

public static class GameEvents
{
    // Define an event that takes an int parameter for the score
    public static event Action<int> OnEnemyKilled;
    public static event Action OnEnemyDestroyed;

    // Method to trigger the event
    public static void EnemyKilled(int score)
    {
        OnEnemyKilled?.Invoke(score);
    }

    //This fires when an enemy is destroyed without being killed, e.g. by the endpoint
    public static void EnemyDestroyed()
    {
        OnEnemyDestroyed?.Invoke();
    }
}
