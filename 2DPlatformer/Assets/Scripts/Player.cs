using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpStrength = 15f;
    [SerializeField] float climbSpeed = 3f;

    // State
    bool isAlive = true;

    // Cached component references
    Rigidbody2D playerRigidBody;
    Animator playerAnimator;
    CapsuleCollider2D playerBodyCollider2D;
    BoxCollider2D playerFeetCollider2D;
    float gravityScaleAtStart;

    // Message then methods
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBodyCollider2D = GetComponent<CapsuleCollider2D>();
        playerFeetCollider2D = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = playerRigidBody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive)
        {
            return;
        }
        Run();
        ClimbLadder();
        Jump();
        FlipSprite();
        Death();
    }
    
    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // Value is between -1 to +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, playerRigidBody.velocity.y);
        playerRigidBody.velocity = playerVelocity;
        
        // Animation State (Transition to running animation if player is moving)
        bool isPlayerMovingHorizontally = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("isRunning", isPlayerMovingHorizontally);
    }
    private void Jump()
    {
        //If the player is NOT touching the "ground" do not allow them to jump
        if(!playerFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))){ return; }

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpStrength);
            playerRigidBody.velocity += jumpVelocityToAdd;
        }
    }    
    private void ClimbLadder()
    {
        //Similar to Jump() but for climbing
        if (!playerFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladders"))) 
        {
            playerAnimator.SetBool("isClimbing", false);
            playerRigidBody.gravityScale = gravityScaleAtStart;
            return;
        }
        
        //Similar to Run() but vertical instead of horizontal
        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(playerRigidBody.velocity.x / 3, controlThrow * climbSpeed);
        playerRigidBody.velocity = climbVelocity;
        playerRigidBody.gravityScale = 0; // Player does not drop due to gravity while on ladder
        // Transition animation state to climbing
        bool isPlayerMovingVertically = Mathf.Abs(playerRigidBody.velocity.y) > Mathf.Epsilon;
        playerAnimator.SetBool("isClimbing", isPlayerMovingVertically);
    }
    private void FlipSprite()
    {
        bool isPlayerMovingHorizontally = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;
        
        if(isPlayerMovingHorizontally)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidBody.velocity.x), 1f);
        }
    }
    private void Death()
    {
        if(playerBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
            {
                isAlive = false;
                playerAnimator.SetTrigger("isDying");
            }
    }
}
