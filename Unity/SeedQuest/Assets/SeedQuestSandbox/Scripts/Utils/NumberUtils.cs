using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeedQuest.Utils
{
    public class NumberUtils
    {
        
        public static int[] GetIndexArray(int size) {
            int[] arrayList = new int[size];
            for (int i = 0; i < size; i++)
            {
                arrayList[i] = i;
            }
            return arrayList;
        }
    }
}
