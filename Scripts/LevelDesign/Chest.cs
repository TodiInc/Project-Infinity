using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;

public class Chest : NetworkBehaviour
{
    public async void DropItem(){
        Inventory.Instance.Add(await ItemAPI.Instance.ConstructItem());
    }
}
