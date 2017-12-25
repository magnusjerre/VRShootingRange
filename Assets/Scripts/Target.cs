using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre
{
    public class Target : MonoBehaviour, IListener, IHittable
    {
        public Texture2D ScoreTexture;
        [SerializeField] private Transform targetTransform;
        public int MaxScore = 100;
        public bool AllowNegativeScore;
        public bool IsPlainScoreTarget;
        public bool FaceForward = true;

        private int totalDestructionScore;
        public int TotalDestructionScore { get { return totalDestructionScore; } }

        private bool isDestroyed;

		[SerializeField] private ParticleSystem hitParticlesPrefab;

		[SerializeField] private BaseTriggerable initTrigger, hitTrigger;
        public float lifetime = 5f;

        [SerializeField] private AudioClip bullseyeHitSound;
        [SerializeField] private AudioClip missSound;
        private Pool audioSourcePool;

        void Awake()
        {
            var targetCollider = GetTargetCollider();
            targetCollider.SetOwner(gameObject);
            SetTargetTransform(targetCollider);

            audioSourcePool = GameObject.FindGameObjectWithTag(Tags.AUDIO_SOURCE_POOL).GetComponent<Pool>();
        }

        private TargetCollider GetTargetCollider()
        {
            var targetCollider = GetComponent<TargetCollider>();
            if (targetCollider == null)
            {
                targetCollider = GetComponentInChildren<TargetCollider>();
            }
            if (targetCollider == null)
            {
                throw new NullReferenceException("Must have a targetCollider");
            }
            return targetCollider;
        }

        private void SetTargetTransform(TargetCollider collider)
        {
            if (targetTransform == null)
            {
                targetTransform = collider.transform;
            }
        }

        // Use this for initialization
        void Start()
        {
            if (!FaceForward)
            {
                targetTransform.Rotate(Vector3.up * 180f);
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
            if (isDestroyed)
            {
                return Hit.Miss();
            }
			isDestroyed = true;
            float uvX = hit.textureCoord.x;
            float uvY = hit.textureCoord.y;
            var score = GetScore(uvX, uvY);
			totalDestructionScore = score;
            ShowHitParticles(hit);
            hitTrigger.Trigger();
            if (bullseyeHitSound != null && score == MaxScore)
            {
                PlaySound(bullseyeHitSound);
            }
            if (missSound != null && score == 0) {
                PlaySound(missSound);
            }

            return new Hit(totalDestructionScore > 0, totalDestructionScore, hit.point);
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
			if (hitParticlesPrefab == null || targetTransform == null)
            {
                return;
            }
			var targetColorTexture = (Texture2D) targetTransform.GetComponent<Renderer>().sharedMaterial.mainTexture;
			int x = (int)(hit.textureCoord.x * targetColorTexture.width);
			int y = (int)(hit.textureCoord.y * targetColorTexture.height);
			Color particleHitColor = targetColorTexture.GetPixel(x, y);
			var particles = Instantiate (hitParticlesPrefab, targetTransform);
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
			particles.transform.parent = targetTransform;
            particles.transform.position = hit.point;
            particles.transform.LookAt(hit.point + hit.normal * 10);
            particles.Play();
        }

        public void Notify(object notifier)
        {
        }

        public void HideTarget()
        {
            if (isDestroyed)
            {
                return;
            }
            isDestroyed = true;
            hitTrigger.Trigger();
        }

    }
}