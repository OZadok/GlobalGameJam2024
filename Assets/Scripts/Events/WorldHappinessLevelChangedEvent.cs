namespace Events
{
    public class WorldHappinessLevelChangedEvent
    {
        public float Level;

        public WorldHappinessLevelChangedEvent(float level)
        {
            Level = level;
        }
    }
}