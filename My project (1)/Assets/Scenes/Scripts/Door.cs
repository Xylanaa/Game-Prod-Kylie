using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // These are the game objects
    public GameObject Door_closed, Door_opened, intText;
    // Audio (still have to add it)
    public AudioSource open, close;
    // Check if the door is opened or not 
    public bool opened;

    // If the player camera collides with the door, the interaction icon will be turned on. If the player presses E as they look at the door, it will open
    void OnTriggerStay (Collider other){
        if (other.CompareTag("MainCamera")){
            if (opened == false){
                intText.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E)){
                    Door_closed.SetActive (false);
                    Door_opened.SetActive(true);
                    intText.SetActive (false);
                    //open.Play();
                    StartCoroutine(repeat());
                    opened = true;
                }
            }
        }
    }
    // When the player is no longer looking at the door, the interaction icon will be switched off
    void OnTriggerExit (Collider other){
        if(other.CompareTag("MainCamera")){
            intText.SetActive (false) ;
        }
    }

    // When the door is opened, it will shut after 4 seconds
    IEnumerator repeat (){
        yield return new WaitForSeconds(4.0f);
        opened = false ;
        Door_closed.SetActive(true);
        Door_opened.SetActive(false);
        //close.play();
    }
}