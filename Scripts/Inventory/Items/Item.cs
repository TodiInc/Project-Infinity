using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    [SerializeField] private string itemType;
    [SerializeField] private string itemAttribute;
    [SerializeField] private string itemDescription;
    [SerializeField] private int damage,health;
    [SerializeField] Sprite icon = null;

    // public Item (string itemType, string itemAttribute, string itemDescription, int damage, int health, Sprite icon)
    // {
    //     this.itemType = itemType;
    //     this.itemAttribute = itemAttribute;
    //     this.itemDescription = itemDescription;
    //     this.damage = damage;
    //     this.health = health;
    //     this.icon = icon;
    // }

    public string getItemAttribute(){
        return itemAttribute;
    }
    public string getItemDescription(){
        return itemDescription;
    }
    public string getItemType(){
        return itemType;
    }
    public int getItemDamage(){
        return damage;
    }
    public int getItemHealth(){
        return health;
    }
    public Sprite getItemIcon(){
        return icon;
    }
    public void setItemIcon(Sprite icon){
        this.icon = icon;
    }
    public void setItemAttribute(string itemAttribute){
        this.itemAttribute = itemAttribute;
    }
    public void setItemDescription(string itemDescription){
        this.itemDescription = itemDescription;
    }
    public void setItemType(string itemType){
        this.itemType = itemType;
    }
    public void setItemStats(int damage, int health){
        this.damage = damage;
        this.health = health;
    }
    public void setItemDamage(int damage){
        this.damage = damage;
    }
    public void setItemHealth(int health){
        this.health = health;
    }
}
