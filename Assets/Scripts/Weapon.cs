using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour, IWeapon
{
    public Transform muzzle;

    public float FireInterval = 1f;
    public float MaxShotLength = 20f;
    private float elapsedTime;

    private IHitlistener hitListener;
    private Pool shotRendererPool;

    public bool CanFire()
    {
        return elapsedTime >= FireInterval;
    }

    void Start()
    {
        shotRendererPool = GameObject.FindGameObjectWithTag(Tags.SHOT_POOL).GetComponent<Pool>();
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
        if (Physics.Raycast(ray, out hit, MaxShotLength))
        {
            var targetCollider = hit.collider.GetComponent<TargetCollider>();
            if (targetCollider != null)
            {
                Hit tHit = targetCollider.GetTarget().RegisterHit(hit);
                if (hitListener != null) {
                    hitListener.NotifyHit(tHit, this);
                }
                shotRendererPool.Get<ShotRenderer>().ShowShot(muzzle.position, hit.point);
                return tHit;;
            }
            var startButton = hit.collider.GetComponent<StartButton>();
            if (startButton != null) {
                startButton.Click();
                return Hit.Miss();
            }
            if (hitListener != null) {
                hitListener.NotifyHit(Hit.Miss(), this);
                shotRendererPool.Get<ShotRenderer>().ShowShot(muzzle.position, hit.point);
            }
        } else {
            if (hitListener != null) {
                hitListener.NotifyHit(Hit.Miss(), this);
            }
            shotRendererPool.Get<ShotRenderer>().ShowShot(muzzle.position, muzzle.position + muzzle.forward * MaxShotLength);
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