using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    [SerializeField] private Transform pickUpLocation;
    [SerializeField] private GameObject holdingObject;
    [SerializeField] private GameObject objectPickedUp;

    private BoxCollider2D pickUpArea;
    private bool isObjectInRange = false;
    private Rigidbody2D objectsRB;
    public float throwForce;

    [SerializeField] public bool hasPickUp = false;
    [SerializeField] private bool canThrow = false;

    private PlayerController playerController;

    private void Start()
    {
        pickUpArea = GetComponentInChildren<BoxCollider2D>();
    }

    private bool CanPickUp()
    {
        if(Input.GetKeyDown(KeyCode.E) && isObjectInRange && !hasPickUp)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Throw()
    {
        hasPickUp = false;
        canThrow = false;

        playerController = holdingObject.transform.parent.GetComponent<PlayerController>();

        objectsRB.bodyType = RigidbodyType2D.Dynamic;

        objectsRB.constraints = RigidbodyConstraints2D.None;

        if (!playerController.facingRight)
        {
            objectsRB.AddForce(-transform.right * throwForce);
        } 
        else if(playerController.facingRight)
        {
            objectsRB.AddForce(transform.right * throwForce);
        }
        
        objectPickedUp.transform.parent = null;
    }

    private void Update()
    {
        if (CanPickUp())
        { 
            hasPickUp = true;
            canThrow = true;

            objectsRB = objectPickedUp.GetComponent<Rigidbody2D>();
            objectsRB.bodyType = RigidbodyType2D.Kinematic;

            objectPickedUp.transform.parent = holdingObject.transform;
            objectPickedUp.transform.position = holdingObject.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.Q) && canThrow)
        {
            Throw();
        } 

        if(!hasPickUp && !canThrow && holdingObject.transform.childCount > 0)
        {
            Destroy(holdingObject.gameObject.transform.GetChild(0));
            Debug.Log("Has children");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("ObjectPickUp"))
            return;

        isObjectInRange = true;
        objectPickedUp = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("ObjectPickUp"))
            return;

        isObjectInRange = false;
    }
}
