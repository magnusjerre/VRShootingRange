using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre
{
    public class Target : MonoBehaviour, IDestroyedListener, IListener, IHittable
    {
        public Texture2D ScoreTexture;
        [SerializeField] private Transform targetTransform;
        private Texture2D actualTargetTexture;
        public int MaxScore = 100;
        public bool AllowNegativeScore;
        public bool IsPlainScoreTarget;
        public bool FaceForward = true;

        public bool HasSubTargets, CanOutliveSubTargets;
        public int BonusScore, BonusTargetsDestroyed;
        public Target[] subTargets;
        public bool ReceivedBonus { get { return subTargets.Length != 0 && BonusTargetsDestroyed == subTargets.Length; } }

        private List<IDestroyedListener> destroyedLiisteners;

        private int totalDestructionScore;
        public int TotalDestructionScore { get { return totalDestructionScore; } }

        private bool isDestroyed;

        private IDestructor destructor;
        [SerializeField] private ParticleSystem hitParticles;

        public string startTriggerName, hitTriggerName;
        private ITriggerable startTrigger = new EmptyTrigger(), hitTrigger = new EmptyTrigger();
        public float lifetime = 5f;

        [SerializeField] private AudioClip bullseyeHitSound;
        [SerializeField] private AudioClip missSound;
        private Pool audioSourcePool;

        void Awake()
        {
            destroyedLiisteners = new List<IDestroyedListener>();
            destructor = GetComponent<IDestructor>();
            if (destructor == null)
            {
                throw new NullReferenceException("Target requires an IDestructor");
            }
            if (hitParticles == null)
                hitParticles = GetComponent<ParticleSystem>();

            if (targetTransform != null)
            {
                var material = targetTransform.GetComponent<Renderer>().material;
                if (material != null)
                {
                    actualTargetTexture = (Texture2D)material.mainTexture;
                }
            }

            var triggers = GetComponents<ITriggerable>();
            for (var i = 0; i < triggers.Length; i++)
            {
                var trigger = triggers[i];
                if (trigger.Name().Equals("StartTrigger"))
                {
                    startTrigger = trigger;
                }
                else if (trigger.Name().Equals("HitTrigger"))
                {
                    hitTrigger = trigger;
                }
            }

            var targetCollider = GetTargetCollider();
            targetCollider.SetOwner(gameObject);
            SetTargetTransform(targetCollider);

            if (hitParticles == null)
            {
                targetTransform.GetComponent<ParticleSystem>();
            }

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
            if (subTargets.Length > 0)
            {
                foreach (var bonusTarget in subTargets)
                {
                    bonusTarget.RegisterDestroyedListener(this);
                }
            }

            startTrigger.AddListener(this);
            startTrigger.Trigger();
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
            float uvX = hit.textureCoord.x;
            float uvY = hit.textureCoord.y;
            var score = GetScore(uvX, uvY);
            totalDestructionScore = score + (ReceivedBonus ? BonusScore : 0);
            ShowHitParticles(hit);
            hitTrigger.Trigger();
            if (bullseyeHitSound != null && score == MaxScore)
            {
                PlaySound(bullseyeHitSound);
            }
            if (missSound != null && score == 0) {
                PlaySound(missSound);
            }
            for (var i = 0; i < subTargets.Length; i++)
            {
                subTargets[i].NotifyParentTargetDestroyed(this);
            }

            for (var i = 0; i < destroyedLiisteners.Count; i++)
            {
                destroyedLiisteners[i].NotifyDestroyed(this);
            }
            DestroyTarget();

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

        public void RegisterDestroyedListener(Target listener)
        {
            destroyedLiisteners.Add(listener);
        }

        public void NotifyDestroyed(Target target)
        {
            BonusTargetsDestroyed++;
            if (ReceivedBonus && !CanOutliveSubTargets)
            {
                DestroyTarget();
            }
        }

        public void NotifyParentTargetDestroyed(Target parent)
        {
            destructor.DestroyAsSubTarget();
        }

        private void DestroyTarget()
        {
            if (isDestroyed)
            {
                return;
            }
            isDestroyed = true;
            destructor.DestroyTarget();
        }

        private void ShowHitParticles(RaycastHit hit)
        {
            if (hitParticles == null)
            {
                return;
            }

            int x = (int)(hit.textureCoord.x * actualTargetTexture.width);
            int y = (int)(hit.textureCoord.y * actualTargetTexture.height);
            Color particleHitColor = actualTargetTexture.GetPixel(x, y);
            var colorOverLifetime = hitParticles.colorOverLifetime;
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
            colorOverLifetime.color = instanceGradient;
            hitParticles.transform.position = hit.point;
            hitParticles.transform.LookAt(hit.point + hit.normal * 10);
            hitParticles.Play();
        }

        public void Notify(object notifier)
        {
            if (notifier == startTrigger)
            {
                Invoke("HideTarget", lifetime);
            }
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