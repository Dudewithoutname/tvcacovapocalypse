using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static void Randomize<T>(this IList<T> list)  
    {  
        var n = list.Count;
        
        while (n > 1) {  
            n--;  
            var k = Random.Range(0, n+1);  
            (list[k], list[n]) = (list[n], list[k]);
        }  
    }
}