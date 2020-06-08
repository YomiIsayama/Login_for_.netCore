using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertBox : Image
{
    public Text aText;
    public Button aBtn;

    protected override void Start()
    {
        base.Start();

        foreach(Transform t in this.transform.GetComponentInChildren<Transform>())
        {
            if (t.name.CompareTo("aText") == 0)
            {
                aText = t.GetComponent<Text>();
            }
            else if (t.name.CompareTo("aBtn") == 0)
            {
                aBtn = t.GetComponent<Button>();
            }
        }
    }
}
