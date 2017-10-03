using System;

public struct Hit
{
    public bool IsHit;
    public int Score;

    public Hit(bool isHit, int score)
    {
        IsHit = isHit;
        Score = score;
    }

    public override string ToString()
    {
        return String.Format("{isHit: {0}, score: {1}}", IsHit, Score);
    }

    public static Hit Miss()
    {
        return new Hit(false, 0);
    }
}