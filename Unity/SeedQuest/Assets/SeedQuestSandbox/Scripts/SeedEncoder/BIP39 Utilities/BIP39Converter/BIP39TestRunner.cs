using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BIP39TestRunner : MonoBehaviour
{
    
    void Start()
    {
        BIP39Tests bpt = new BIP39Tests();
        bpt.runAllTests();
    }

}
