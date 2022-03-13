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
            if(collision.gameObject.name.Contains("Rogue Pick Up"))
            {
                ScoreManager.instance.RoguePointRemove();
            }
            ScoreManager.instance.AddPoint();
        }
        else
            ScoreManager.instance.RemovePoint();

        Destroy(collision.gameObject);
    }

    public bool TogglePoints()
    {
        return isSubtracting = !isSubtracting;
    }
}
