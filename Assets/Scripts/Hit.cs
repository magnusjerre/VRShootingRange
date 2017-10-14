using System;
using UnityEngine;

public struct Hit
{
    public bool IsHit;
    public int Score;

    public Vector3 HitLocation;

    public Hit(bool isHit, int score, Vector3 hitLocation)
    {
        IsHit = isHit;
        Score = score;
        HitLocation = hitLocation;
    }

    public override string ToString()
    {
        return String.Format("{isHit: {0}, score: {1}}", IsHit, Score);
    }

    public static Hit Miss()
    {
        return new Hit(false, 0, Vector3.zero);
    }
}