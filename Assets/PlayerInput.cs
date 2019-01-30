using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public CharacterMover player;

    [SerializeField]
    private float xMovementf = 0.0f, zMovementf = 0.0f;

    private void Awake()
    {
        

    }

    // Use this for initialization
    private void Start()
    {
        if (!player)
        {
            Debug.Log("Warning! No player character was set through the inspector."
                       + "The game will attempt to automatically find the player character");
            GameObject possiblePlayer = GameObject.Find("Hero");
            player = possiblePlayer.GetComponent<CharacterMover>();
        }
        
    }



    // Update is for scanning for inputs
    void Update()
    {

        xMovementf = Input.GetAxisRaw("Horizontal");
        zMovementf = Input.GetAxisRaw("Vertical");

    }

    // FixedUpdate is for passing inputs along to the controller script. 
    private void FixedUpdate()
    {

        player.MoveCharacter(xMovementf, zMovementf);
    }
}
