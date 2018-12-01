using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBucket : MonoBehaviour {

    public static DataBucket instance = null;

    public bool chthuluEnding = false;
    public bool gummybearEnding = false;
    public bool baldrEnding = false;

	void Awake ()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
            Destroy(gameObject);
	}
}