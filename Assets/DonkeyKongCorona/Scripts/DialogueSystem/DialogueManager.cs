using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI nameText;
    [SerializeField]
    TextMeshProUGUI bossDialogueText;
    [SerializeField]
    TextMeshProUGUI playerDialogueText;

    [SerializeField]
    GameObject bossDialogue;
    [SerializeField]
    GameObject playerDialogue;

    [SerializeField]
    GameObject bossDialogueBGImage;
    [SerializeField]
    GameObject playerDialogueBGImage;

    [SerializeField]
    GameObject continueButton;

    [SerializeField]
    Sprite dialogueBG;
    [SerializeField]
    Sprite dialogueCompleteBG;

    Queue<string> sentences;

    int count = 0;

    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        print("Starting Dialogue with " + dialogue.name);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.bossSentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (count%2 == 0)
        {
            bossDialogue.SetActive(false);
            playerDialogue.SetActive(true);
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            string sentence = sentences.Dequeue();

            playerDialogueText.text = sentence;
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }
        else
        {

            bossDialogue.SetActive(true);
            playerDialogue.SetActive(false);
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            string sentence = sentences.Dequeue();

            bossDialogueText.text = sentence;
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }
        if (count == 3 || count == 5)
        {
            count++;
        }
        count++;
    }

    IEnumerator TypeSentence(string sentence)
    {
        if (count%2==0)
        {
            continueButton.GetComponent<Button>().interactable = false;
            playerDialogueBGImage.GetComponent<Image>().sprite = dialogueBG;
            playerDialogueText.text = "";
            char[] letters = sentence.ToCharArray();
            int length = 0;
            foreach (char letter in letters)
            {
                yield return new WaitForSecondsRealtime(.05f);
                playerDialogueText.text += letter;
                length++;
                if (letters.Length == length)
                {
                    playerDialogueBGImage.GetComponent<Image>().sprite = dialogueCompleteBG;
                    continueButton.GetComponent<Button>().interactable = true;
                }
                //yield return null;
            }
        }
        else
        {
            continueButton.GetComponent<Button>().interactable = false;
            bossDialogueBGImage.GetComponent<Image>().sprite = dialogueBG;
            bossDialogueText.text = "";
            char[] letters = sentence.ToCharArray();
            int length = 0;
            foreach (char letter in letters)
            {
                yield return new WaitForSecondsRealtime(.05f);
                bossDialogueText.text += letter;
                length++;
                if (letters.Length == length)
                {
                    bossDialogueBGImage.GetComponent<Image>().sprite = dialogueCompleteBG;
                    continueButton.GetComponent<Button>().interactable = true;
                }
                //yield return null;
            }
        }
    }

    void EndDialogue()
    {
        print("End od conversation.");
        FindObjectOfType<DialogueTrigger>().EndConversation();
    }
}
