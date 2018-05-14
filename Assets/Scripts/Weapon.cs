using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof(AudioSource))]
	public class Weapon : MonoBehaviour, IWeapon
    {
        public Transform muzzleCalculation, muzzleVisual;
		private ParticleSystem muzzleSmokeParticles;

        public float FireInterval = 1f;
        public float MaxShotLength = 20f;
        private float elapsedTime;

        private IHitlistener hitListener;
		private IFireListener fireListener;
        private Pool shotRendererPool;
        private AudioSource audioShot;
        
        private Animator animator;

		public WeaponEnum position;

        public bool CanFire()
        {
            return elapsedTime >= FireInterval;
        }

        void Start()
        {
			if (muzzleVisual == null) {
				muzzleVisual = muzzleCalculation;
			}
            
			audioShot = GetComponent<AudioSource>();
            audioShot.playOnAwake = false;
            shotRendererPool = GameObject.FindGameObjectWithTag(Tags.SHOT_POOL).GetComponent<Pool>();
            muzzleSmokeParticles = GetComponentInChildren<ParticleSystem>();
			muzzleSmokeParticles.transform.parent = muzzleVisual;
			muzzleSmokeParticles.transform.localPosition = Vector3.zero;
			muzzleSmokeParticles.transform.localScale = Vector3.one;
			muzzleSmokeParticles.transform.localRotation = Quaternion.identity;
            elapsedTime = FireInterval;
            GameObject.FindGameObjectWithTag(Tags.GAME_CONTROLLER).GetComponent<GameController>().AddWeapon(this);
            animator = GetComponentInChildren<Animator>();

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
			fireListener.NotifyFire (this);

			bool result = false;

            audioShot.Play();
			muzzleSmokeParticles.Play();
            if (animator != null) {
                animator.SetTrigger("Fire");
            }
            Ray ray = new Ray(muzzleCalculation.position, muzzleCalculation.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, MaxShotLength))
            {
				shotRendererPool.Get<ShotRenderer>().ShowShot(muzzleVisual.position, hit.point);
                var targetCollider = hit.collider.GetComponent<TargetCollider>();
                if (targetCollider != null)
                {
                    Hit tHit = targetCollider.GetOwner<IHittable>().RegisterHit(hit);
                    if (hitListener != null)
                    {
                        hitListener.NotifyHit(tHit, this);
                    }
				} 
				else if (hitListener != null)
                {
                    hitListener.NotifyHit(Hit.Miss(), this);
                }
            }
            else
            {
                if (hitListener != null)
                {
                    hitListener.NotifyHit(Hit.Miss(), this);
                }
                shotRendererPool.Get<ShotRenderer>().ShowShot(muzzleVisual.position, muzzleVisual.position + muzzleVisual.forward * MaxShotLength);
            }
            elapsedTime = 0f;
            return true;
        }

		public void ResetCooldown() 
		{
			elapsedTime = FireInterval + 1f;
		}

        public void AddListener(IHitlistener listener)
        {
            if (this.hitListener == null)
            {
                this.hitListener = listener;
            }
        }

		public void AddFireListener (IFireListener listener) {
			if (this.fireListener == null) {
				this.fireListener = listener;
			}
		}
    }
}