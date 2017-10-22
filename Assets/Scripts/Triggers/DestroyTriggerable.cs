public class DestroyTriggerable : BaseTriggerable
{
    void Start()
    {
        
    }
    
    public override void Trigger()
    {
        NotifyListeners();
        Destroy(gameObject);
    }

}