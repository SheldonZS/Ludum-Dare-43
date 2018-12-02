using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBucket : MonoBehaviour {

    public static DataBucket instance = null;

    public bool CthulhuEnding = false;
    public bool GummyBearEnding = false;
    public bool BaldrEnding = false;

    public bool CuchulainOffering = false;
    public bool AnansiOffering = false;
    public bool AmaterasuOffering = false;
    public bool NephthysOffering = false;
    public bool SekhmetOffering = false;
    public bool MercuryOffering = false;
    public bool MonkeyKingOffering = false;

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

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            Debug.Log(Input.mousePosition);
    }
}