using System;
using System.Linq;
using UnityEngine;

namespace SeedQuest.Utils
{
    public class RandomUtils
    {
        static System.Random random = new System.Random();

        // Get a random hex number of given number of digits
        public static string GetRandomHexNumber(int digits)
        {
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);

            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");
        }

        // Shuffle array elements with Fisher-Yates algorthm
        public static void Shuffle<T> (T[] array) {
            int n = array.Length;
            while (n > 1) {
                int k = random.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        public static int[] GetRandomIndexArray(int size) {
            int[] arrayList = new int[size];
            for (int i = 0; i < size; i++)
            {
                arrayList[i] = i;
            }
            RandomUtils.Shuffle<int>(arrayList);
            return arrayList;
        }

        public static Color[] GetRandomColorArray() {
            Color[] colors = new Color[6];
            colors[0] = Color.red;
            colors[1] = Color.cyan;
            colors[2] = Color.green;
            colors[3] = new Color(255, 165, 0);
            colors[4] = Color.yellow;
            colors[5] = Color.magenta;
            Shuffle<Color>(colors);
            return colors;
        }
    }
}