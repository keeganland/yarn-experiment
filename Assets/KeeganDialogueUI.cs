using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

public class KeeganDialogueUI : DialogueUIBehaviour
{
    public GameObject textPanel;
    public float typeSpeed = 0.01f;
    public List<Button> optionButtons;
    private Text boxContent;
    private GameObject choicePanel;
    private bool isTyping;
    private bool cancelTyping;
    private Yarn.OptionChooser SetSelectedOption;


    private void Awake()
    {
        if (textPanel)
        {
            boxContent = textPanel.GetComponentInChildren<Text>();
            textPanel.SetActive(false);
            choicePanel = textPanel.transform.Find("ChoicePanel").gameObject;
        }
        else
        {
            throw new System.Exception("There must be a text panel on which to display the text!");
        }
       
    }

    public override IEnumerator RunLine(Line line)
    {

        string lineOfText = line.text; //was argument in TextBoxManager

        //Below taken from TextScroll() in "TextBoxManager", which I made for The Arena, which seems roughly equivalent in functionality to this script

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
    }

    public override IEnumerator RunCommand(Command command)
    {
        throw new System.NotImplementedException();
    }


    //Stolen outright from the example
    public override IEnumerator RunOptions(Options optionsCollection, OptionChooser optionChooser)
    {
        if (optionsCollection.options.Count > optionButtons.Count)
        {
            Debug.LogWarning("There are more options to present than there are" +
                             "buttons to present them in. This will cause problems.");
        }

        int i = 0;
        foreach (var optionString in optionsCollection.options)
        {
            optionButtons[i].gameObject.SetActive(true);
            optionButtons[i].GetComponentInChildren<Text>().text = optionString;
            i++;
        }


        // Record that we're using it
        SetSelectedOption = optionChooser;

        // Wait until the chooser has been used and then removed (see SetOption below)
        while (SetSelectedOption != null)
        {
            yield return null;
        }

        // Hide all the buttons
        foreach (var button in optionButtons)
        {
            button.gameObject.SetActive(false);
        }

    }

    public void SetOption(int selectedOption)
    {

        // Call the delegate to tell the dialogue system that we've
        // selected an option.
        SetSelectedOption(selectedOption);

        // Now remove the delegate so that the loop in RunOptions will exit
        SetSelectedOption = null;
    }



}
