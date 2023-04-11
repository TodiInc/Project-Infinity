using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable{
    public float CurrentHealth { set; get; }
    public float BaseHealth {set ; get;}
    public bool Targetable { set; get; }
    public void OnHit(float damage);
    public void RemoveSelf();
}
