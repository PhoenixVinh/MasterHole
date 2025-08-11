namespace _Scripts.Event
{
    public class ResourceEvent
    {
        public delegate void UpdateResourceEvent();
        public static UpdateResourceEvent OnUpdateResource;
    }
}