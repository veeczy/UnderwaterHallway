using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyScript : MonoBehaviour
{
    //Variables
    public GameObject player;
    public GameObject key;
    public GameObject chest;
    public KeyCode pickupKey = KeyCode.E;
    public bool hasKey = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(pickupKey)) //if pickup is pressed
        {
            //check if hovering over item you can pickup
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if(Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.gameObject.tag == "Key") //pick up key
                {
                    hasKey = true;
                    Debug.Log("Picked up Key");
                    Destroy(key); //destroy it from scene so there arent duplicates
                }
                if (hitInfo.collider.gameObject.tag == "Chest" && (!hasKey))
                {
                    // remind player they need a key
                }
                if (hitInfo.collider.gameObject.tag == "Chest" && hasKey)
                {
                    //chest opening stuff
                }
            }
        }
    }
}
