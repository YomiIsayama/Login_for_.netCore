using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ResurnCode
{
    public int code;
    public string message;
}
public enum ActionCode
{
    Login,
    Register
}
public class ActionAct
{
    public ActionCode code;
    public string message;
}

