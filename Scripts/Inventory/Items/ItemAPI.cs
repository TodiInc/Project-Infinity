using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class ItemAPI : MonoBehaviour
{
    [SerializeField] private DataSnapshot snapshot = null;
    
    [SerializeField] private static ItemAPI _Instance;

    public DatabaseReference dbReference;

    public static ItemAPI Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = new GameObject().AddComponent<ItemAPI>();
                _Instance.name = _Instance.GetType().ToString();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }


    public void Awake(){
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    
    public async Task<Item> ConstructItem(){

        ItemAPIParameters iAP = new ItemAPIParameters();

        await FirebaseDatabase.DefaultInstance
            .GetReference("items")
            .GetValueAsync().ContinueWithOnMainThread(task => {

            if (task.IsFaulted) {
                Debug.Log("Could not get object");
            }
            else if (task.IsCompleted) {

                snapshot = task.Result;
                int randomItemIndex = (int)Random.Range(1,snapshot.ChildrenCount);
                DataSnapshot itemSnapshot = snapshot.Child("item"+randomItemIndex);
                iAP.ItemType = itemSnapshot.Child("type").Value.ToString();
                iAP.ItemAttribute = itemSnapshot.Child("attribute").Value.ToString();
            }
        });
        await FirebaseDatabase.DefaultInstance
            .GetReference("descriptions")
            .GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted) {
                Debug.Log("Could not get object");
            }
            else if (task.IsCompleted) {
                snapshot = task.Result;

                int randomItemIndex = Random.Range(1,3);
                
                foreach(DataSnapshot description in snapshot.Children){
                    if( description.Key.ToString().Contains(iAP.ItemType) && 
                        description.Key.ToString().Contains(iAP.ItemAttribute.ToLower()) && 
                        description.Key.ToString().Contains(randomItemIndex.ToString()
                        ))
                        {
                        iAP.ItemDescription = description.Value.ToString();
                        if(iAP.ItemAttribute == "Fire"){
                            iAP.HigherItem = true;
                        }
                        break;
                    }
                }
            }
        });
        await FirebaseDatabase.DefaultInstance
            .GetReference("stats")
            .GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted) {
                Debug.Log("Could not get object");
            }
            else if (task.IsCompleted) {
                snapshot = task.Result;

                if(iAP.HigherItem){
                    iAP.From = int.Parse(snapshot.Child("higherItems").Child("from").Value.ToString());
                    iAP.To = int.Parse(snapshot.Child("higherItems").Child("to").Value.ToString());
                    iAP.Damage = Random.Range(iAP.From,iAP.To+1);
                    iAP.Health = Random.Range(iAP.From,iAP.To+1);
                }
                else{
                    iAP.From = int.Parse(snapshot.Child("lowerItems").Child("from").Value.ToString());
                    iAP.To = int.Parse(snapshot.Child("lowerItems").Child("to").Value.ToString());
                    iAP.Damage = Random.Range(iAP.From,iAP.To+1);
                    iAP.Health = Random.Range(iAP.From,iAP.To+1);
                }
                Sprite[] sprites = Resources.LoadAll<Sprite>("Items"+"/"+iAP.ItemType+"/"+iAP.ItemAttribute);
                iAP.ItemIcon = sprites[Random.Range(0,sprites.Length)];
            }
        });
        Item item = ScriptableObject.CreateInstance<Item>();
        item.setItemAttribute(iAP.ItemAttribute);
        item.setItemStats(iAP.Damage, iAP.Health);
        item.setItemDescription(iAP.ItemDescription);
        item.setItemIcon(iAP.ItemIcon);
        item.setItemType(iAP.ItemType);

        await FirebaseDatabase.DefaultInstance
            .GetReference("UserInventories")
            .GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted) {
                Debug.Log("Could not get object");
            }
            else if (task.IsCompleted) {
                snapshot = task.Result;

                FirebaseUser user = FirebaseAuthManager.user;
                string userJson = JsonUtility.ToJson(user.UserId);
                string itemJson = JsonUtility.ToJson(item);

                string itemKey = dbReference.Child("UserInventories").Child(user.UserId).Child("items").Child("item").Push().Key;
                dbReference.Child("UserInventories").Child(user.UserId).Child("items").Child(itemKey).SetRawJsonValueAsync(itemJson);
                
            }
        });
        return  item;
    }

    public class ItemAPIParameters

    {
    public string ItemType {get;set;}
    public string ItemAttribute {get;set;}
    public string ItemDescription {get;set;}

    public Sprite ItemIcon;

    public int Health {get;set;}
    public int Damage {get;set;}
    public int From {get;set;}
    public int To {get;set;}

    public bool HigherItem {get;set;}
    }
}
