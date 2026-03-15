public static class RunState
{
    public static Difficulty startDifficulty;
    public static bool difficultyChangedMidRun = false;
    public static bool customGame = false;

    public static void StartRun(Difficulty difficulty, bool usedStartingBonus)
    {
        startDifficulty = difficulty;
        difficultyChangedMidRun = false;
        customGame = usedStartingBonus;
    }
}