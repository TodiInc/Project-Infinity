using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Collider2D swordCollider;
    private float _damage = 10;
    public float Damage {
        get{
            return _damage;
        } 
        set{
            _damage = value;
        }
    }
    Vector2 rightAttackOffset;
    public string targetTag;

    private void Start() {
        rightAttackOffset = transform.localPosition;
    }

    public void AttackRight() {
        
        swordCollider.enabled = true;
        transform.localPosition = rightAttackOffset;
        
    }

    public void AttackLeft() {
        
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
        
    }

    public void StopAttack() {
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter2D (Collider2D other) {
        IDamageable damageableObject = other.GetComponent<IDamageable>();

        if(other.CompareTag(targetTag)){
            damageableObject.OnHit(Damage);
        }
    }
}
