using UnityEngine;

public class RequestHappinessChangeEvent
{
    public readonly GameObject GameObject;
    public readonly float Amount;

    public RequestHappinessChangeEvent(GameObject gameObject, float amount)
    {
        GameObject = gameObject;
        Amount = amount;
    }
}
