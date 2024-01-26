namespace Events
{
    public class PlayerChangedDirectionEvent
    {
        public float Direction;

        public PlayerChangedDirectionEvent(float direction)
        {
            Direction = direction;
        }
    }
}