using UnityEngine;

public class Destroy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name + " was destroyed");
        Destroy(collision.gameObject);
    }
}
