namespace Jerre
{
    public class WaitTrigger : BaseTriggerable
    {
        public float waitTime;
        private bool animate;

        void Start()
        {

        }

        private void DoNotifyListeners()
        {
            NotifyListeners();
            animate = false;
        }

        public override void Trigger()
        {
            Invoke("DoNotifyListeners", waitTime);
            animate = true;
        }

    }
}