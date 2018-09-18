using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedManager : MonoBehaviour {

    public int siteBits = 2;            // Number of bits used to determine location
    public int spotBits = 1;            // Number of bits used to determine spots for each action
    public int actionBits = 1;          // Number of bits used to determine action choice
    public int actionCount = 1;         // Total actions the player needs to take at each location
    public int siteCount = 4;           // Total number of locations the player needs to visit
    public string inputSeed = "41A5";
    public string recoveredSeed = null;

    static private SeedManager instance = null;
    static public SeedManager Instance {
        get {
            if (instance == null)
                instance = GameObject.FindObjectOfType<SeedManager>();
            return instance;
        }
    }

    static public int SiteBits { get { return Instance.siteBits; } }
    static public int SpotBits { get { return Instance.spotBits; } }
    static public int ActionBits { get { return Instance.actionBits; } }
    static public int ActionCount { get { return Instance.actionCount; } }
    static public int SiteCount { get { return Instance.siteCount; } }

    static public string InputSeed
    {
        get { return Instance.inputSeed; }
    }
    static public string RecoveredSeed
    {
        get { return Instance.recoveredSeed; }
    }
}
