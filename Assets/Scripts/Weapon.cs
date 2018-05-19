using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof(AudioSource))]
	public class Weapon : MonoBehaviour, IWeapon
    {
		public WeaponEnum position;
        
		[SerializeField] Transform muzzleCalculation, muzzleVisual;
		[SerializeField] float FireInterval = 1f;
		[SerializeField] float MaxShotLength = 40f;

		float timeSinceLastShot;
        IHitlistener hitListener;
		IFireListener fireListener;
		ParticleSystem muzzleSmokeParticles;
		Pool magazine;
        AudioSource audioShot;
        

        void Start()
        {
			if (muzzleVisual == null) {
				muzzleVisual = muzzleCalculation;
			}
            
			audioShot = GetComponent<AudioSource>();
            audioShot.playOnAwake = false;
			magazine = GetComponentInChildren<Pool> ();
            muzzleSmokeParticles = GetComponentInChildren<ParticleSystem>();
			muzzleSmokeParticles.transform.parent = muzzleVisual;
			muzzleSmokeParticles.transform.localPosition = Vector3.zero;
			muzzleSmokeParticles.transform.localScale = Vector3.one;
			muzzleSmokeParticles.transform.localRotation = Quaternion.identity;
            timeSinceLastShot = FireInterval;
            GameObject.FindGameObjectWithTag(Tags.GAME_CONTROLLER).GetComponent<GameController>().AddWeapon(this);
        }

        void Update()
        {
            timeSinceLastShot += Time.deltaTime;
        }


        public bool Fire()
        {
			if (timeSinceLastShot < FireInterval)
            {
                return false;
            }

			timeSinceLastShot = 0f;
			fireListener.NotifyFire (this);
			audioShot.Play();
			muzzleSmokeParticles.Play();
			magazine.Get<BulletRay> ().Fire (muzzleVisual, this, muzzleCalculation, hitListener);
			return true;
        }

		public void ResetCooldown() 
		{
			timeSinceLastShot = FireInterval;
		}

        public void AddHitListener(IHitlistener listener)
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