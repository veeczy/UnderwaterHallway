using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight : MonoBehaviour
{
    // Variables
    [SerializeField] GameObject flashlightLight;
    public KeyCode flashlightKey = KeyCode.Mouse0;
    private bool flashlightActive = false;

    // Start is called before the first frame update
    void Start()
    {
        flashlightLight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        flashlightLight.SetActive(flashlightActive);

        if (Input.GetKeyDown(flashlightKey)) // flip switch for it on/off
        {
            flashlightActive = !flashlightActive;
        }

        if (flashlightActive) //if on
        {
            flashlightLight.SetActive(true);
            //if statement for stun mechanic
        }
        if (!flashlightActive) //if off
        {
            flashlightLight.SetActive(false);
        }
    }
}
