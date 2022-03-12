using UnityEngine;

public class PlatformCheck : MonoBehaviour
{
    private Renderer rend;

    private void Start()
    {
        rend = GetComponentInParent<Renderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if(rend.sharedMaterial.name == "M_deathPlatform")
        Destroy(collision.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (rend.sharedMaterial.name == "M_deathPlatform")
            Destroy(collision.gameObject);
    }
}
