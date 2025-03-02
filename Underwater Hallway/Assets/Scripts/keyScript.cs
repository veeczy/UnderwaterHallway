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

    public GameObject pickupText;
    public GameObject chestOpenText;
    public GameObject winChestText;
    // Variables for VFX
    public AudioSource keySource;
    public AudioSource winSource;
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
                keySource.Play(); // audio for picking up key
                hasKey = true;
            }
            if(chestReach && !hasKey) // if chest is in reach and you don't have the key
            {
                Debug.Log("You don't have the key.");
                chestOpenText.SetActive(true);
            }
            if(chestReach && hasKey)
            {
                Debug.Log("Player opened chest.");
                // play animation
                winCondition = true;
                winSource.Play();
            }
            
            if (hasKey == true)
            {
                pickupText.SetActive(false);
            }

            if (hasKey && chestReach)
            {
                winChestText.SetActive(true);
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

        if (keyReach == true)
        {
            pickupText.SetActive(true);
        }

        if (chestReach == true)
        {
            chestOpenText.SetActive(true);
        }

        if (chestReach && hasKey)
        {
            chestOpenText.SetActive(false);
            winChestText.SetActive(true);
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
            Debug.Log("Chest out of sight.");
            //if(!hasKey) { chestnokeyText.SetActive(true); } // remove vfx to indicate it needs a key
            //if(hasKey) {chestkeyText.SetActive(true); } // remove vfx to indicate it can be opened up
        }

        if (keyReach == false) 
        {
            pickupText.SetActive(false);
        }

        if (chestReach == false)
        {
            chestOpenText.SetActive(false);
        }
    }
}
