using UnityEngine;

public class HappinessChangedEvent
{
    public GameObject GameObject;
    public float Level;

    public HappinessChangedEvent(GameObject gameObject, float level)
    {
        GameObject = gameObject;
        Level = level;
    }
}
