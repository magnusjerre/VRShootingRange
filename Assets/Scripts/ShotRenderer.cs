using UnityEngine;

namespace Jerre
{
    public class ShotRenderer : MonoBehaviour
    {
        public ParticleSystem particleSystem;

        void Awake()
        {
            particleSystem = GetComponentInChildren<ParticleSystem>();
            var shape = particleSystem.shape;
        }
        
        public void ShowShot(Vector3 start, Vector3 end)
        {
            var shape = particleSystem.shape;
            var length = (end - start).magnitude;
            var localPos = Vector3.forward * length / 2f;
            transform.position = start;
            transform.LookAt (end);   
            shape.radius = length / 2f;         
            shape.position = localPos;
            shape.rotation = Vector3.up * 90f;
            particleSystem.Play();
            gameObject.SetActive(true);

            Invoke("Hide", particleSystem.main.duration * 2f);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}