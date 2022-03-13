using UnityEngine;

public class Destroy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.instance.Play("Player Dies");
        }

        Debug.Log(collision.name + " was destroyed");
        Destroy(collision.gameObject);
    }
}
