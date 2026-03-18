using UnityEngine;

public class GameplaySettingsManager : MonoBehaviour
{
    public static GameplaySettingsManager Instance;

    public Difficulty difficulty = Difficulty.Normal;
    public bool permadeath = true;
    public StartingBonus startingBonus = StartingBonus.None;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetDifficulty(Difficulty newDifficulty)
    {
        difficulty = newDifficulty;

        if (difficulty == Difficulty.Nightmare)
        {
            permadeath = true;
            startingBonus = StartingBonus.None;
        }
    }

    public void SetPermadeath(bool value)
    {
        if (difficulty == Difficulty.Nightmare)
            return;

        permadeath = value;
    }

    public void SetStartingBonus(StartingBonus bonus)
    {
        if (difficulty == Difficulty.Nightmare)
            return;

        startingBonus = bonus;
    }

    public bool IsCustomGame()
    {
        return startingBonus != StartingBonus.None || !permadeath;
    }
}