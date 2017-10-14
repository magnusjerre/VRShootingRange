using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour, IWeapon
{
    public Transform muzzle;

    public float FireInterval = 1f;
    private float elapsedTime;

    private IHitlistener hitListener;

    public bool CanFire()
    {
        return elapsedTime >= FireInterval;
    }

    void Start()
    {
        elapsedTime = FireInterval;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
    }


    public Hit Fire()
    {
        if (!CanFire())
        {
            return Hit.Miss();
        }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20f))
        {
            var targetCollider = hit.collider.GetComponent<TargetCollider>();
            if (targetCollider != null)
            {
                Hit tHit = targetCollider.GetTarget().RegisterHit(hit);
                if (hitListener != null) {
                    hitListener.NotifyHit(tHit, this);
                }
                return tHit;;
            }
            var startButton = hit.collider.GetComponent<StartButton>();
            if (startButton != null) {
                startButton.Click();
                return Hit.Miss();
            }
        }
        elapsedTime = 0f;
        return Hit.Miss();
    }

    public void AddListener(IHitlistener listener)
    {
        if (this.hitListener == null) {
            this.hitListener = listener;
        }
    }
}