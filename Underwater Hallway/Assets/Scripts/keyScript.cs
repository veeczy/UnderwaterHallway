using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyScript : MonoBehaviour
{
    // Variables
    public GameObject key; // so you can destroy and teleport it
    //public GameObject chest; // so you can animate it

    public KeyCode pickupKey = KeyCode.E;

    public bool hasKey = false;
    public bool keyReach = false;
    public bool chestReach = false;

    // Variables for VFX
    //public AudioSource keySource;
    //public GameObject keyText;
    //public GameObject chestkeyText;
    //public GameObject chestnokeyText;

    // Variables for win/lose condition
    public bool winCondition = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //
        if(Input.GetKeyDown(pickupKey)) // if player tries to pickup
        {
            if(keyReach && !hasKey) // if key is in reach and you don't have it picked up
            {
                Debug.Log("Key is picked up.");
                Destroy(key);
                //keySource.Play(); // audio for picking up key
                hasKey = true;
            }
            if(chestReach && !hasKey) // if chest is in reach and you don't have the key
            {
                Debug.Log("You don't have the key.");
            }
            if(chestReach && hasKey)
            {
                Debug.Log("Player opened chest.");
                // play animation
                winCondition = true;
            }
        }
    }

    public void OnTriggerStay(Collider other) // what is in range of sight
    {
        if (other.gameObject.CompareTag("Key")) // key in sight
        {
            keyReach = true;
            Debug.Log("Key in sight.");
            //keyText.SetActive(true); // vfx to indicate it can be picked up
        }

        if (other.gameObject.CompareTag("Chest")) // if chest in sight
        {
            chestReach = true;
            Debug.Log("Chest in sight.");
            //if(!hasKey) { chestnokeyText.SetActive(true); } // vfx to indicate it needs a key
            //if(hasKey) {chestkeyText.SetActive(true); } // vfx to indicate it can be opened up
        }
    }

    public void OnTriggerExit(Collider other) // what is out of range of sight
    {
        if (other.gameObject.CompareTag("Key")) // if key is out of sight
        {
            keyReach = false;
            Debug.Log("Key out of sight.");
            //keyText.SetActive(false); // remove vfx now that key is out of range
        }

        if (other.gameObject.CompareTag("Chest")) // if chest is out of sight
        {
            chestReach = false;
            Debug.Log("Chest in sight.");
            //if(!hasKey) { chestnokeyText.SetActive(true); } // remove vfx to indicate it needs a key
            //if(hasKey) {chestkeyText.SetActive(true); } // remove vfx to indicate it can be opened up
        }
    }
}
