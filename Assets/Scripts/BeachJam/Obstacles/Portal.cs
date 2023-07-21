using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject portalA;
    public GameObject portalB;
    public float teleportDelay = 1;
    public bool useOffset = false;

    //Tracks number of enters and exits to ensure the player can telelport again and isn't stuck for eternity
    float teleportTimer = 0;

    private void OnTriggerEnter2D(Collider2D other) {
        //If we haven't already teleported
        if(teleportTimer < 0){
            //Check which portal is closest (the one colided with)
            float aDistance = Vector3.Distance(other.gameObject.transform.position, portalA.transform.position);
            float bDistance = Vector3.Distance(other.gameObject.transform.position, portalB.transform.position);
            
            //And teleport to the other one    
            if(aDistance < bDistance){
                Vector3 offset = other.transform.position - portalA.transform.position;
                other.gameObject.transform.position = portalB.transform.position + (useOffset?offset:new Vector3(0,0,0));
            }else{
                Vector3 offset = other.transform.position - portalB.transform.position;
                other.gameObject.transform.position = portalA.transform.position + (useOffset?offset:new Vector3(0,0,0));
            }
            teleportTimer = teleportDelay;
        }
    }
    private void FixedUpdate() {
        teleportTimer -= Time.deltaTime;
    }

    //Resets the ability to teleport when you exit the portal
    private void OnTriggerStay2D(Collider2D other) {
        teleportTimer = teleportDelay;
    }
}
