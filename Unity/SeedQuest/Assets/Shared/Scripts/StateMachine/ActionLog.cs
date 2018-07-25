using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLog : MonoBehaviour {

    public List<Interactable> iLog = new List<Interactable>();
    public List<int> aLog = new List<int>();
    public GameStateData gameState;
    private SeedToByte seedToByte;

    public void Start()
	{
        seedToByte = GetComponent<SeedToByte>();
	}

	public void Add(Interactable interactable, int actionID) {
        iLog.Add(interactable);
        aLog.Add(actionID);
    }

    public bool ActionsComplete() {
        return iLog.Count >= gameState.SiteCount * gameState.ActionCount;   
    }

    public int ActionCount() {
        return iLog.Count;
    }

    public int[] EncodeActionLog() {
        
        int ActionCount = gameState.ActionCount;
        int SiteCount = gameState.SiteCount;
        int totalInt = (2 * ActionCount) + SiteCount;
        int counter = 0;
        List<int> actionLog = new List<int>();

        for (int j = 0; j < SiteCount; j++)
        {
            actionLog.Add(iLog[(counter)].siteID);
            for (int i = 0; i < ActionCount; i++)
            {
                actionLog.Add(iLog[counter].spotID);
                actionLog.Add(aLog[counter]);
                counter += 1;
            }
        }

        int[] actionArray = actionLog.ToArray();
        return actionArray;
    }

    public string RecoverSeed(int[] actionArray) {
        
        string recoveredSeed = seedToByte.getSeed(actionArray);
        return recoveredSeed;
    }

    public string getSeed()
    {
        int[] encodedActions = EncodeActionLog();
        string seed = RecoverSeed(encodedActions);
        return seed;
    }


}
