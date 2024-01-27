using UnityEngine;

namespace Events
{
    public class PlayerEnterNpc
    {
        public GameObject NpcGameObject;

        public PlayerEnterNpc(GameObject npcGameObject)
        {
            NpcGameObject = npcGameObject;
        }
    }
}