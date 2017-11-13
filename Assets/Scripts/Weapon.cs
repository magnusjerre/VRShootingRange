using UnityEngine;
using UnityEngine.UI;

namespace Jerre
{
    [RequireComponent(typeof(AudioSource))]
    public class Weapon : MonoBehaviour, IWeapon
    {
        public Transform muzzle;
        private ParticleSystem fireParticles;

        public float FireInterval = 1f;
        public float MaxShotLength = 20f;
        private float elapsedTime;

        private IHitlistener hitListener;
        private Pool shotRendererPool;
        private AudioSource audioShot;

        public bool CanFire()
        {
            return elapsedTime >= FireInterval;
        }

        void Start()
        {
            audioShot = GetComponent<AudioSource>();
            audioShot.playOnAwake = false;
            shotRendererPool = GameObject.FindGameObjectWithTag(Tags.SHOT_POOL).GetComponent<Pool>();
            fireParticles = GetComponentInChildren<ParticleSystem>();
            elapsedTime = FireInterval;
            GameObject.FindGameObjectWithTag(Tags.GAME_CONTROLLER).GetComponent<GameController>().AddWeapon(this);
        }

        void Update()
        {
            elapsedTime += Time.deltaTime;
        }


        public bool Fire()
        {
            if (!CanFire())
            {
                return false;
            }

            audioShot.Play();
            fireParticles.Play();
            Ray ray = new Ray(muzzle.position, muzzle.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, MaxShotLength))
            {
                var targetCollider = hit.collider.GetComponent<TargetCollider>();
                if (targetCollider != null)
                {
                    Hit tHit = targetCollider.GetOwner<IHittable>().RegisterHit(hit);
                    if (hitListener != null)
                    {
                        hitListener.NotifyHit(tHit, this);
                    }
                    shotRendererPool.Get<ShotRenderer>().ShowShot(muzzle.position, hit.point);
                    return true;
                }
                if (hitListener != null)
                {
                    hitListener.NotifyHit(Hit.Miss(), this);
                    shotRendererPool.Get<ShotRenderer>().ShowShot(muzzle.position, hit.point);
                }
            }
            else
            {
                if (hitListener != null)
                {
                    hitListener.NotifyHit(Hit.Miss(), this);
                }
                shotRendererPool.Get<ShotRenderer>().ShowShot(muzzle.position, muzzle.position + muzzle.forward * MaxShotLength);
            }
            elapsedTime = 0f;
            return true;
        }

        public void AddListener(IHitlistener listener)
        {
            if (this.hitListener == null)
            {
                this.hitListener = listener;
            }
        }
    }
}