using UnityEngine;

public class MouseFire : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 20f))
            {
                var target = hit.collider.GetComponent<Target>();
                if (target != null)
                {
                    target.RegisterHit(hit.textureCoord.x, hit.textureCoord.y);
                }
            }
        }
    }
}