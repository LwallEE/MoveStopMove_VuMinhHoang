using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Cache
{
    private static Dictionary<Collider, Character> dictChar = new Dictionary<Collider, Character>();

    public static Character GenCharacters(Collider collider)
    {
        if (!dictChar.ContainsKey(collider))
        {
            Character character = collider.GetComponent<Character>();
            
            dictChar.Add(collider, character);
        }

        return dictChar[collider];
    }
}
