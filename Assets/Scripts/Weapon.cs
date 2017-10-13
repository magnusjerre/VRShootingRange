using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    public Transform muzzle;

    private int _weaponScore;

    public float FireInterval = 1f;
    private float elapsedTime;

    public bool CanFire()
    {
        return elapsedTime >= FireInterval;
    }

    public int WeaponScore
    {
        get { return _weaponScore; }
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
                return targetCollider.GetTarget().RegisterHit(hit);
            }
        }
        elapsedTime = 0f;
        return Hit.Miss();
    }
}