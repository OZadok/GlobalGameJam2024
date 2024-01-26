using UnityEngine;

namespace Events
{
    public class TickEvent
    {
        public NpcData NpcData;

        public TickEvent(NpcData npcData)
        {
            NpcData = npcData;
        }
    }
}