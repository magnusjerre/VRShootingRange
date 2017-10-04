using UnityEngine;

public class MJRandom
{
    public int nextSeed;

    public MJRandom(int seed)
    {
        nextSeed = seed;
    }

    public int Next(int min, int max)
    {
        Random.InitState(nextSeed++);
        return Random.Range(min, max);
    }
}