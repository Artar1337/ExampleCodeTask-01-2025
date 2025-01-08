using System;

public interface IGameScoreSystem
{
    event Action<int> OnScoreChanged;

    int Score { get; }
    int HiScore { get; }
    void ResetScore();
    bool TryUpdateHiScore();
    void AddScore(int increment = 1);
    void Init();
}

public class GameScoreSystem : IGameScoreSystem
{
    private const string HiScoreKey = "HiScore";

    public int Score { get; private set; }
    public int HiScore { get; private set; }

    public event Action<int> OnScoreChanged;

    public void AddScore(int increment = 1)
    {
        Score += increment;
        OnScoreChanged?.Invoke(Score);
    }

    public void Init()
    {
        HiScore = CryptographicPlayerPrefs.GetInt(HiScoreKey, 0);
    }

    public void ResetScore()
    {
        Score = 0;
        OnScoreChanged?.Invoke(Score);
    }

    public bool TryUpdateHiScore()
    {
        if (HiScore < Score)
        {
            HiScore = Score;
            CryptographicPlayerPrefs.SetInt(HiScoreKey, HiScore);
            return true;
        }

        return false;
    }
}
