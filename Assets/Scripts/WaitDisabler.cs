using UnityEngine;

namespace Jerre {
    public class WaitDisabler : MonoBehaviour {
        public float waitTime;

        public void Trigger() {
            Invoke("Disable", waitTime);
        }

        private void Disable() {
            gameObject.SetActive(false);
        }

    }
}