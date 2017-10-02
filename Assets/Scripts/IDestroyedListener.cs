public interface IDestroyedListener
{
     void NotifyDestroyed(Target target);
     void NotifyParentTargetDestroyed(Target parent);
}