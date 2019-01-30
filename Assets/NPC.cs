using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    private CharacterMover npcMover;
    private TextAsset dialogue;
    public string characterName;
    public string talkToNode;

    private void Awake()
    {
        if (!dialogue)
            dialogue = Resources.Load<TextAsset>("NPC/helloworld");
        if (characterName == "" || characterName == null)
            characterName = gameObject.name;
    }


    // Start is called before the first frame update
    void Start()
    {
        if (!npcMover)
            npcMover = gameObject.GetComponent<CharacterMover>();

        if (dialogue != null)
        {
            FindObjectOfType<Yarn.Unity.DialogueRunner>().AddScript(dialogue);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
