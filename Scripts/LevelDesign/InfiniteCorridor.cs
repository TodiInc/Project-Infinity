using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InfiniteCorridor : NetworkBehaviour
{
    private float length, startpos;
    public GameObject player;

    void Update()
    {
        if(player == null){
            player = GameManager.Instance.getPlayers().Find(playerObject => playerObject.GetComponent<NetworkIdentity>().isLocalPlayer);
        }
        else{
            float temp = player.transform.position.x;

            transform.position = new Vector3(startpos, transform.position.y, transform.position.z);

            if (temp > startpos + length){
                startpos += length;
            }

            else if (temp < startpos - length){
                startpos -= length;
            }
        }
        
    }
    public void Start(){

        startpos = transform.position.x;
        length = GetComponent<TilemapRenderer>().bounds.size.x;
    }
}
