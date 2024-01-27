using UnityEngine;

namespace Events
{
    public class PlayerExitNpc
    {
        public GameObject NpcGameObject;

        public PlayerExitNpc(GameObject npcGameObject)
        {
            NpcGameObject = npcGameObject;
        }
    }
}