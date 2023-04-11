using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DamageableCharacter : NetworkBehaviour, IDamageable
{
    [SerializeField] public bool targetable = true;
    [SerializeField] public float _baseHealth = 1;

    [SerializeField, HideInInspector] Animator animator;
    [SerializeField, HideInInspector] private Enemy enemy;
    [SerializeField, HideInInspector] private PlayerController player;
    [SerializeField, HideInInspector, SyncVar] private bool isAlive = true;
    [SerializeField, HideInInspector] private Rigidbody2D rb;
    [SerializeField, HideInInspector] private Collider2D physicsCollider;
    [SerializeField, HideInInspector] private float _currentHealth = 1;

    public float BaseHealth{
        set{
            _baseHealth = value;
        }
        get{
            return _baseHealth;
        }
    }
    public float CurrentHealth {
        set {

            if(value <= _currentHealth){
                animator.SetTrigger("hit");
            }
            _currentHealth = value;

            if(_currentHealth <= 0) {
                Defeated();
                Targetable = false;
            }
        }
        get {
            return _currentHealth;
        }
    }

    public bool Targetable { get { return targetable; }  
    set {
        targetable = value;
        
        if(TryGetComponent<PlayerController>(out player)){
            player.enabled = value;
        }
        rb.simulated = value;
        physicsCollider.enabled = value;
    } }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        physicsCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("isAlive", isAlive);
        _currentHealth = _baseHealth;
    }

    public void Defeated(){
        if(TryGetComponent<Enemy>(out enemy)){
            GameManager.Instance.getEnemies().Remove(gameObject);
            GameManager.Instance.enemiesKilled++;
        }
        isAlive = false;
        animator.SetBool("isAlive", isAlive);
    }

    public void RemoveSelf() {
        NetworkServer.Destroy(gameObject);
    }
    
    public void OnHit(float damage)
    {
        CurrentHealth -=damage;
    }
}
