using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeedQuest.Utils {
    public class StringUtils
    {
        static public bool CheckIfValidHex(string input) {
            foreach(char c in input) {
                bool valid = (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9' || c == 'a' || c == 'A' || c == 'b' || c == 'B' || c == 'c' || c == 'C' || c == 'd' || c == 'D' || c == 'e' || c == 'E' || c == 'f' || c == 'F');
                if (!valid)
                    return false;
            }

            return true;
        }
    }
}
