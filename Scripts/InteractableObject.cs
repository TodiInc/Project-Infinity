using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] public Button button;
    [SerializeField, HideInInspector] private Chest chest;
    [SerializeField, HideInInspector] private Rigidbody2D rb;

    public void Start(){
        rb = GetComponent<Rigidbody2D>();
        button.onClick.AddListener(Interact);
    }

    public void Interact()
    {
        if(TryGetComponent<Chest>(out chest)){
            chest.DropItem();
        }
        RemoveSelf();
    }

    public void RemoveSelf()
    {
        return;
        //Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.CompareTag("Player")){
            button.interactable = true;
        }
    }
    public void OnTriggerExit2D(Collider2D col){
        if(col.gameObject.CompareTag("Player")){
            button.interactable = false;
        }
    }
}
