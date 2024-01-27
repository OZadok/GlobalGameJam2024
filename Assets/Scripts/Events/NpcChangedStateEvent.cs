using UnityEngine;

namespace Events
{
    public class NpcChangedStateEvent
    {
        public GameObject NpcGameObject;
        public NpcChangedStateEvent(GameObject gameObject)
        {
            NpcGameObject = gameObject;
        }
    }
}