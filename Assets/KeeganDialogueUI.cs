using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

public class KeeganDialogueUI : DialogueUIBehaviour
{
    public GameObject textPanel;
    public GameObject gameControlsContainer;
    public float typeSpeed;
    public List<Button> optionButtons;
    public Text boxContent;
    private GameObject choicePanel;
    private bool isTyping;
    private bool cancelTyping;
    private Yarn.OptionChooser optionChoiceDelegate;


    private void Awake()
    {
        if (textPanel)
        {
            //boxContent = textPanel.GetComponentInChildren<Text>();
            textPanel.SetActive(false);
            choicePanel = textPanel.transform.Find("ChoicePanel").gameObject;
            if (choicePanel)
                choicePanel.SetActive(false);
        }
        else
        {
            throw new System.Exception("There must be a text panel on which to display the text!");
        }
       
    }

    private void Update()
    {
        if (boxContent.gameObject.activeSelf)
        {
            if (Input.GetButton("Jump"))
            {
                cancelTyping = true;
            }
        }
    }


    public override IEnumerator RunLine(Line line)
    {
        boxContent.gameObject.SetActive(true);
        string lineOfText = line.text; //was argument in TextBoxManager

        //Below taken from TextScroll() in "TextBoxManager", which I made for The Arena, which seems roughly equivalent in functionality to this script
        #region from Text Box Manager
        int letter = 0;
        boxContent.text = "";
        isTyping = true;
        cancelTyping = false;

        while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
        {
            boxContent.text += lineOfText[letter];
            letter += 1;
            yield return new WaitForSeconds(typeSpeed);
        }
        boxContent.text = lineOfText; //when the update loop breaks the while loop by making cancelTyping false, we want to make sure that the whole text line is displayed
        isTyping = false;
        cancelTyping = false;
        #endregion

        while (Input.anyKeyDown == false)
        {
            yield return null;
        }

        //boxContent.gameObject.SetActive(false);
        Debug.Log("Reached end of RunLine coroutine");
    }

    /* Stolen outright from the example
     * */
    public override IEnumerator RunCommand(Command command)
    {
        // "Perform" the command
        Debug.Log("Command: " + command.text);

        yield break;
    }


    //Mostly stolen outright from the example
    public override IEnumerator RunOptions(Options optionsCollection, OptionChooser optionChooser)
    {
        if (optionsCollection.options.Count > optionButtons.Count)
        {
            Debug.LogWarning("There are more options to present than there are" +
                             "buttons to present them in. This will cause problems.");
        }

        if (choicePanel)
            choicePanel.SetActive(true);

        int i = 0;
        foreach (var optionString in optionsCollection.options)
        {
            optionButtons[i].gameObject.SetActive(true);
            optionButtons[i].GetComponentInChildren<Text>().text = optionString;
            i++;
        }
        
        // Set the delegate that this script uses to the one passed as an argument from Yarn
        optionChoiceDelegate = optionChooser;

        // Wait until the chooser has been used and then removed (see SetOption below)
        while (optionChoiceDelegate != null)
        {
            yield return null;
        }

        // Hide all the buttons
        if (choicePanel)
            choicePanel.SetActive(false);
        foreach (var button in optionButtons)
        {
            button.gameObject.SetActive(false);
        }

    }

    /* This is called by buttons, menu choices, etc.
     * */
    public void SetOption(int selectedOption)
    {

        /* Does this need some sort of guarantee that the delegate isn't null? 
         * This _should_ never happen, but as a "something absolutely wild has happened" guarantee?
         * */

        // Call the delegate to tell the dialogue system that we've
        // selected an option.
        try
        {
            optionChoiceDelegate(selectedOption);
        }
        catch (System.Exception)
        {

            throw;
        }
        finally
        {
            // Now remove the delegate so that the loop in RunOptions will exit
            optionChoiceDelegate = null;
        }
    }

    /// Called when the dialogue system has started running.
    public override IEnumerator DialogueStarted()
    {
        Debug.Log("Dialogue starting!");

        // Enable the dialogue controls.
        if (textPanel != null)
            textPanel.SetActive(true);

        // Hide the game controls.
        if (gameControlsContainer != null)
        {
            gameControlsContainer.gameObject.SetActive(false);
        }

        yield break;
    }

    /// Called when the dialogue system has finished running.
    public override IEnumerator DialogueComplete()
    {
        Debug.Log("Complete!");

        // Hide the dialogue interface.
        if (textPanel != null)
            textPanel.SetActive(false);

        // Show the game controls.
        if (gameControlsContainer != null)
        {
            gameControlsContainer.gameObject.SetActive(true);
        }

        yield break;
    }



}
