using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class RandomUtils
{
    public static T GetRandomItem<T>(IEnumerable<T> sequence)
    {
        return sequence.ElementAt(UnityEngine.Random.Range(0, sequence.Count()));
    }
}
