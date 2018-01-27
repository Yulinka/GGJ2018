using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static T GetRandomItem<T>(this T[] self) where T : class
    {
        if (self.Length == 0)
            return null;
        
        return self[(int)Random.value * (self.Length - 1)];
    }
}