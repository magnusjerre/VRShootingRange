using UnityEngine;

namespace Jerre
{
    public class LookAt : MonoBehaviour
    {
        public Vector3 target;
        public bool onlyXZDirection = true;
		public bool continuous = false;

        void Start()
        {
            if (onlyXZDirection)
            {
                transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
            }
            else
            {
                transform.LookAt(target);
            }
        }

		void Update() {
			if (continuous) {
				if (onlyXZDirection)
				{
					transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
				}
				else
				{
					transform.LookAt(target);
				}
			}
		}

    }
}