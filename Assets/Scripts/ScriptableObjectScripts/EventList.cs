using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Event List", menuName ="Single Reference/Event List")]
public class EventList : ScriptableObject
{
    public EventStart[] List;

    public bool AllComplete() {
        foreach (EventStart e in List)
            if (!e.done)
                return false;
        return true;
    }
}
