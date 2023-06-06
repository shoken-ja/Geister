using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class space : MonoBehaviour
{
    public int x;
    public int y;
    public int yourGhost = -1;

    public void Start()
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();

        EventTrigger trigger = GetComponent<EventTrigger>();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventDate) => { OnClick(); });

        trigger.triggers.Add(entry);
    }

    public void OnClick()
    {
        if (yourGhost != -1)
        {
            MyGhosts.instance.Attacked(yourGhost);
        }
        else
        {
            MyGhosts.instance.OnClickSpace(x, y);
        }
    }
}
