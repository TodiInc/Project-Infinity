using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ItemDisplay : MonoBehaviour
{
    private Item _Item = null;

    public Image itemIcon;
    public Canvas canvas;

    [SerializeField, HideInInspector] private GameObject player;

    public Item Item{
        get {return _Item;}

        set{
            player = GameManager.Instance.getPlayers().Find(playerObject => playerObject.GetComponent<NetworkIdentity>().isLocalPlayer);
            if(_Item == value){
                return;
            }
            float baseHealth = player.GetComponent<DamageableCharacter>().BaseHealth;
            float currentHealth = player.GetComponent<DamageableCharacter>().CurrentHealth;
            float damage = player.GetComponentInChildren<SwordAttack>().Damage;
            
             if(_Item != null){
                baseHealth -= _Item.getItemHealth();
                currentHealth -=_Item.getItemHealth();
                damage -= _Item.getItemDamage();
             }
            _Item = value;
            baseHealth += value.getItemHealth();
            damage += value.getItemDamage();
            currentHealth += value.getItemHealth();
            player.GetComponent<DamageableCharacter>().BaseHealth = baseHealth;
            player.GetComponent<DamageableCharacter>().CurrentHealth = currentHealth;
            player.GetComponentInChildren<SwordAttack>().Damage = damage;
            itemIcon.sprite = Item.getItemIcon();
            itemIcon.color = new Color(itemIcon.color.r,itemIcon.color.g,itemIcon.color.b,1);
        }
    }
    public void Start(){
        canvas.GetComponent<Canvas>().enabled = true;
    }
}
