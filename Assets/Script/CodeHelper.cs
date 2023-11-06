using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CodeHelper : MonoBehaviour
{
    private static System.Random random;
    public static int GetGuidSeed()
    {
        Guid randomGuid = Guid.NewGuid();

        // Convert the GUID to a byte array to use as a seed
        byte[] bytes = randomGuid.ToByteArray();
        int seed = BitConverter.ToInt32(bytes, 0);
        return seed;
    }
}
