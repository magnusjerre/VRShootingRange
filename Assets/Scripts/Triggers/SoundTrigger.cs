using UnityEngine;

namespace Jerre
{
    public class SoundTrigger : BaseTriggerable
    {
        [SerializeField] private AudioClip clip;
        private AudioSource audioSource;
        public SoundTrigger()
        {
        }

        void Awake() 
        {
            audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
            audioSource.clip = clip;
            audioSource.playOnAwake = false;
        }

        public override void Trigger()
        {
            audioSource.Play();
        }
    }
}