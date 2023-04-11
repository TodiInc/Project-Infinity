using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float attackCooldown = 0f;
    [SerializeField] public float damage = 1f;
    [SerializeField] public float moveSpeed;
    
    [SerializeField, HideInInspector] private Enemy otherEnemy;
    [SerializeField, HideInInspector] private GameObject player;
    [SerializeField, HideInInspector] private Rigidbody2D rb;
    [SerializeField, HideInInspector] private SpriteRenderer sprite;
    [SerializeField, HideInInspector] private Animator animator;
    [SerializeField, HideInInspector] private Vector2 noMovement = new Vector2(0,0);
    [SerializeField, HideInInspector] private float time = 0f;
    [SerializeField, HideInInspector] private float actualAttackCooldown = 0f;

    private void Start(){
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        GameManager.Instance.addToEnemiesList(gameObject);
        int playerIndex = Random.Range(0,GameManager.Instance.getPlayers().Count);
        player = GameManager.Instance.getPlayers()[playerIndex];
    }

    private bool IsMoving{
        set{
            isMoving = value;
            animator.SetBool("isMoving", isMoving);
        }
    }
    private bool isMoving = false;

    private void FixedUpdate(){

        if(player==null){
            int playerIndex = Random.Range(0,GameManager.Instance.getPlayers().Count);
            player = GameManager.Instance.getPlayers()[playerIndex];
        }
        Vector2 direction = (((Vector2)player.transform.position + player.GetComponent<BoxCollider2D>().offset) - (Vector2)transform.position).normalized;

        rb.AddForce(direction * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
        if(rb.velocity == noMovement){
            IsMoving = false;
            return;
        }
        else
        {
            if(rb.velocity.x > 0)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }
            IsMoving = true;
        }
        
    }

    void OnCollisionStay2D(Collision2D other){
        if(other.gameObject.TryGetComponent<Enemy>(out otherEnemy))
            return;
            
        time += Time.deltaTime;
        IDamageable damageable = other.collider.GetComponent<IDamageable>();

        if(damageable != null){
            if( time >= actualAttackCooldown)
            {
                animator.SetTrigger("swordAttack");
                damageable.OnHit(damage);
                actualAttackCooldown = attackCooldown;
                time = 0f;
            }
        }
    }
}
