using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    public ItemDisplay boots;
    public ItemDisplay chestplate;
    public ItemDisplay helmet;
    public ItemDisplay sword;

    [SerializeField] private static Inventory _Instance;

    public static Inventory Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = new GameObject().AddComponent<Inventory>();
                _Instance.name = _Instance.GetType().ToString();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }

    public void Start(){
        boots = GameObject.Find("Boots").GetComponent<ItemDisplay>();
        chestplate = GameObject.Find("Chestplate").GetComponent<ItemDisplay>();
        helmet = GameObject.Find("Helmet").GetComponent<ItemDisplay>();
        sword = GameObject.Find("Sword").GetComponent<ItemDisplay>();
    }

    public void Add(Item item){
        StartCoroutine("Add"+item.getItemType(),item);
        items.Add(item);
    }

    public void Remove(Item item){
        items.Remove(item);
    }
    public List<Item> getItems(){
        return items;
    }
    private IEnumerator AddBoots(Item item){
        if(boots.Item == null){
            boots.Item = item;
        }
        else{
            boots.Item = item;
        }
        yield return null;
    }
    private IEnumerator AddChestplate(Item item){
        if(chestplate.Item == null){
            chestplate.Item = item;
        }
        else{
            chestplate.Item = item;
        }
        yield return null;
    }
    private IEnumerator AddSword(Item item){
        if(sword.Item == null){
            sword.Item = item;
        }
        else{
            sword.Item = item;
        }
        yield return null;
    }
    private IEnumerator AddHelmet(Item item){
        if(helmet.Item == null){
            helmet.Item = item;
        }
        else{
            helmet.Item = item;
        }
        yield return null;
    }
}
