using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerInput : MonoBehaviour
{
    public CharacterMover player;
    public float interactionRadius = 2.0f;

    [SerializeField]
    private float xMovementf = 0.0f, zMovementf = 0.0f;
    private bool isTalkingToNPC = false;
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
        //Movement
        xMovementf = Input.GetAxisRaw("Horizontal");
        zMovementf = Input.GetAxisRaw("Vertical");

        //Talk
        if (Input.GetButton("Jump") && !isTalkingToNPC)
        {
            //StartCoroutine("CheckForNearbyNPC");
            CheckForNearbyNPC();
        }

    }

    // FixedUpdate is for passing inputs along to the controller script. 
    private void FixedUpdate()
    {
        player.MoveCharacter(xMovementf, zMovementf);
    }

    private void CheckForNearbyNPC()
    {
        Debug.Log("Checking for nearby NPCs");
        var allParticipants = new List<NPC>(FindObjectsOfType<NPC>());
        var target = allParticipants.Find(delegate (NPC npc) {
            return string.IsNullOrEmpty(npc.talkToNode) == false && // has a conversation node?
            (npc.transform.position - player.transform.position)// is in range?
            .magnitude <= interactionRadius;
        });
        if (target != null)
        {
            Debug.Log("NPC found");
            // Kick off the dialogue at this node.
            FindObjectOfType<DialogueRunner>().StartDialogue(target.talkToNode);
        }
    }
}
