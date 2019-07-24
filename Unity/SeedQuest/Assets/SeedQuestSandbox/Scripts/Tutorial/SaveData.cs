using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
	public bool skip ;

    public SaveData(bool data)
	{
		skip = data;
	}
}
