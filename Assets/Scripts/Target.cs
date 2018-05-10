using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre
{
    public class Target : MonoBehaviour, IHittable
    {
        [SerializeField] private Texture2D ScoreTexture;
		[SerializeField] private Transform triggerObjectPrefab;
		[SerializeField] private Transform offsetObjectPrefab;
		[SerializeField] private Transform targetModelPrefab;
		[SerializeField] private ParticleSystem hitParticlesPrefab;
        
		private Transform offsetInstance;
		private Transform totalOffset;
		private Transform targetModelInstance;
		private Transform triggerObjectInstance;
		private Texture2D targetColorTexture;
        private Color targetColor;
        
		public int MaxScore = 100;
        public bool AllowNegativeScore;
        public bool IsPlainScoreTarget;
        public bool FaceForward = true;

        private int totalDestructionScore;
        public int TotalDestructionScore { get { return totalDestructionScore; } }
        private bool isDestroyed;
		public bool IsDestroyed { get { return isDestroyed; } }
		private bool isHiding;

		private ITriggerable initTrigger, hideTrigger;
		public ITriggerable HideTrigger { get { return hideTrigger; } }
        public float lifetime = 5f;

        [SerializeField] private AudioClip bullseyeHitSound;
        [SerializeField] private AudioClip missSound;
        private Pool audioSourcePool;

        void Awake()
        {
			triggerObjectInstance = Instantiate (triggerObjectPrefab, transform);
			offsetInstance = Instantiate (offsetObjectPrefab, triggerObjectInstance).transform;
			offsetInstance.parent = triggerObjectInstance;
			totalOffset = offsetInstance;
			if (offsetInstance.childCount > 0) {
				totalOffset = offsetInstance.GetChild (0);
			}
			targetModelInstance = Instantiate (targetModelPrefab, triggerObjectInstance);
			targetModelInstance.position = totalOffset.position;
			targetModelInstance.rotation = totalOffset.rotation;
			SetColliderOwner (targetModelInstance);
			targetColorTexture = (Texture2D) targetModelInstance.GetChild(0).GetComponent<Renderer>().sharedMaterial.mainTexture;
            targetColor = targetModelInstance.GetChild(0).GetComponent<Renderer>().sharedMaterial.color;
			SetInitAndHitTriggers ();

            audioSourcePool = GameObject.FindGameObjectWithTag(Tags.AUDIO_SOURCE_POOL).GetComponent<Pool>();
        }

		private void SetColliderOwner(Transform targetModel)
        {
            var targetColliders = targetModel.GetComponentsInChildren<TargetCollider>();
            if (targetColliders.Length == 0) {
                throw new NullReferenceException("Must have a targetCollider");
            }
            for (var i = 0; i < targetColliders.Length; i++) {
                targetColliders[i].SetOwner(gameObject);
            }

            // var targetCollider = targetModel.GetComponent<TargetCollider>();
            // if (targetCollider == null)
            // {
            //     targetCollider = targetModel.GetComponentInChildren<TargetCollider>();
            // }
            // if (targetCollider == null)
            // {
                
            // }
			// targetCollider.SetOwner (gameObject);
        }

		private void SetInitAndHitTriggers() {
			var triggers = triggerObjectInstance.GetComponents<ITriggerable>();
			var count = 0;
			foreach (var trigger in triggers) {
				if (trigger.Name ().Equals ("init")) {
					initTrigger = trigger;
					count++;
				} else if (trigger.Name ().Equals ("hide")) {
					hideTrigger = trigger;
					count++;
				}
				if (count == 2) {
					break;
				}
			}
		}

        // Use this for initialization
        void Start()
        {
            if (!FaceForward)
            {
				targetModelInstance.Rotate(Vector3.up * 180f);
            }

            initTrigger.Trigger();
			if (lifetime != -1) {
				Invoke("HideTarget", lifetime);
			}
        }

        public int GetScore(float posX, float posY)
        {
            if (IsPlainScoreTarget)
            {
                return MaxScore;
            }

            int x = (int)(posX * ScoreTexture.width);
            int y = (int)(posY * ScoreTexture.height);
            if (AllowNegativeScore)
            {
                return Mathf.RoundToInt((ScoreTexture.GetPixel(x, y).r - 0.5f) * 2 * MaxScore);
            }
            return Mathf.RoundToInt(Mathf.Max((ScoreTexture.GetPixel(x, y).r - 0.5f) * 2 * MaxScore, 0));
        }

        public Hit RegisterHit(RaycastHit hit)
        {
			if (isDestroyed || isHiding)
            {
                return Hit.Miss();
            }
			isDestroyed = true;
            float uvX = hit.textureCoord.x;
            float uvY = hit.textureCoord.y;
            var score = GetScore(uvX, uvY);
			totalDestructionScore = score;
            ShowHitParticles(hit);
            hideTrigger.Trigger();
            if (bullseyeHitSound != null && score == MaxScore)
            {
                PlaySound(bullseyeHitSound);
            }
            if (missSound != null && score == 0) {
                PlaySound(missSound);
            }

			return new Hit(totalDestructionScore > 0 ? HitEnum.HIT : HitEnum.MISS, totalDestructionScore, hit.point);
        }

        private void PlaySound(AudioClip soundClip)
        {
            var audioSource = audioSourcePool.Get<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.clip = soundClip;
            audioSource.Play();
            var waitDisabler = audioSource.GetComponent<WaitDisabler>();
            if (waitDisabler != null)
            {
                waitDisabler.waitTime = audioSource.clip.length;
                waitDisabler.Trigger();
            }
        }

        private void ShowHitParticles(RaycastHit hit)
        {
			if (hitParticlesPrefab == null || targetModelInstance == null)
            {
                return;
            }
            Color particleHitColor = targetColor;
            if (targetColorTexture != null) {
                int x = (int)(hit.textureCoord.x * targetColorTexture.width);
                int y = (int)(hit.textureCoord.y * targetColorTexture.height);
                particleHitColor = targetColorTexture.GetPixel(x, y);
            }
			var particles = Instantiate (hitParticlesPrefab, triggerObjectInstance.transform.position, triggerObjectInstance.transform.rotation);
			var particleColors = particles.colorOverLifetime;
            var instanceGradient = new Gradient();
            instanceGradient.SetKeys(
                new GradientColorKey[]{
                new GradientColorKey(particleHitColor, 0f),
                new GradientColorKey(particleHitColor, 1f)
            }, new GradientAlphaKey[]{
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(0.3f, 0.6f),
                new GradientAlphaKey(0f, 0f)
            });
            particleColors.color = instanceGradient;
            particles.transform.position = hit.point;
            particles.transform.LookAt(hit.point + hit.normal * 10);
            float distanceForScaleEqualToOneSqr = 330;
            float minScale = 0.4f;
            float scale = Mathf.Max(minScale, transform.position.sqrMagnitude / distanceForScaleEqualToOneSqr);
            particles.transform.localScale = Vector3.one * scale;
            particles.Play();
        }

        public void HideTarget()
        {
			if (isDestroyed || isHiding)
            {
                return;
            }
			isHiding = true;
            hideTrigger.Trigger();
        }

		public void ApplyCustomisation(float[] deltaAngles, float[] animationTimes, bool hasRotationInfo, float rotationsPerSecond) {
			var waver = triggerObjectInstance.GetComponent<Waver> ();
			if (waver != null) {
				waver.ApplyCustomisation (deltaAngles, animationTimes);
			}

			var rotator = triggerObjectInstance.GetComponent<Rotator> ();
			if (rotator != null && hasRotationInfo) {
				rotator.ApplyCustomisation (rotationsPerSecond);
			}
		}

    }
}