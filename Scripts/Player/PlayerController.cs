using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    public float moveSpeed = 1f;
    public float maxSpeed = 2f;
    //public float collisionOffset = 0.05f;
    //public ContactFilter2D movementFilter;
    private PlayerInput playerInput;
    public SwordAttack swordAttack;
    public Camera playerCamera;
    private Vector2 movementInput;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    Animator animator;
    //private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    private bool canMove = true;
    private bool IsMoving{
        set{
            isMoving = value;
            animator.SetBool("isMoving", isMoving);
        }
    }
    private bool isMoving = false;

    void Start()
    {
        if(!isLocalPlayer){
            playerCamera.enabled = false;
        }
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        GameManager.Instance.addToPlayersList(gameObject);
    }

    private void FixedUpdate() {

        if(isLocalPlayer){
            if(canMove && movementInput != Vector2.zero) {
                rb.velocity = Vector2.ClampMagnitude(rb.velocity + (movementInput * moveSpeed * Time.deltaTime), maxSpeed);

                if(movementInput.x > 0){
                    flipOnServer(false);
                }else if(movementInput.x < 0){
                    flipOnServer(true);
                }
                IsMoving = true;
                playerInput.neverAutoSwitchControlSchemes = true;
            } else{
                IsMoving = false;
                playerInput.neverAutoSwitchControlSchemes = false;
            }
        }
    }

    // private bool TryMove(Vector2 direction) { // this doesn't do anything
    //     Debug.Log("TryMove");
    //         if(direction != Vector2.zero) {
    //             int count = rb.Cast(
    //                 direction, 
    //                 movementFilter, 
    //                 castCollisions, 
    //                 moveSpeed * Time.fixedDeltaTime + collisionOffset); 
    //             if(isLocalPlayer){
    //                 if(count == 0){
    //                     rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    //                     return true;
    //                 } else {
    //                     return false;
    //                 }
    //             }else{
    //                 return false;
    //             }
    //         } else {
    //             return false;
    //         }
    // }

    void OnMove(InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire() {
        if(isLocalPlayer){
            int random = Random.Range(0,2);
            animator.SetTrigger("swordAttack");
            animator.SetInteger("attackIndex", random);
        }
    }

    public void SwordAttack() {
        LockMovement();

        if(sprite.flipX == true){
            swordAttack.AttackLeft();
        } else {
            swordAttack.AttackRight();
        }
    }

    public void EndSwordAttack() {
        UnlockMovement();
        swordAttack.StopAttack();
    }

    public void LockMovement() {
        canMove = false;
    }

    public void UnlockMovement() {
        canMove = true;
    }

    [Command]
    public void flipOnServer(bool direction){
        sprite.flipX = direction;
        flipOnClients(direction);
    }
    [ClientRpc]
    public void flipOnClients(bool direction){
        sprite.flipX = direction;
    }
}
