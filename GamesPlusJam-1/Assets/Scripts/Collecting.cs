using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collecting : MonoBehaviour
{
    public static Collecting instance;

    private BoxCollider2D collider;

    public bool isSubtracting;

    private void Awake()
    {
        instance = this;   
    }

    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("ObjectPickUp"))
        {
            return;
        }

        if (!isSubtracting)
        {
            if (collision.gameObject.name.Contains("Rogue Pick Up"))
            {
                AudioManager.instance.Play("Rogue Object");
                ScoreManager.instance.RoguePointRemove();

                Destroy(collision.gameObject);
                return;
            }

            ScoreManager.instance.AddPoint();
            AudioManager.instance.Play("Object Collected");
        }
        else
        {
            ScoreManager.instance.RemovePoint();
            AudioManager.instance.Play("Minus Point");
        }

        Destroy(collision.gameObject);
    }

    public bool TogglePoints()
    {
        return isSubtracting = !isSubtracting;
    }
}
