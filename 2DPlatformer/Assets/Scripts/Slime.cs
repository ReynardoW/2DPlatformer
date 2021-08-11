using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    Rigidbody2D slimeRigidBody;
    CapsuleCollider2D slimeCapsuleCollider;
    [SerializeField] float moveSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        slimeRigidBody = GetComponent<Rigidbody2D>();
        slimeCapsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsFacingRight())
        {
            slimeRigidBody.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            slimeRigidBody.velocity = new Vector2(-moveSpeed, 0f);
        }
    }

    bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(slimeRigidBody.velocity.x)), 1f);
    }
}
