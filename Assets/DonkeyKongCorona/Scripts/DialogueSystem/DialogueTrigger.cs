using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    GameObject hud;

    GameObject dialogueParent;

    bool playIdle = false;

    bool hadDialogue = false;

    private void Start()
    {
        hud = GameObject.Find("HUD");
        dialogueParent = GameObject.Find("DialogueParent");
        dialogueParent.SetActive(false);
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hadDialogue)
        {
            GameManager.Instance.StopBGMusic();
            Time.timeScale = 0;
            //hud.SetActive(false);
            //GameObject.Find("HUD").SetActive(false);
            //GameObject.Find("DialogueParent").transform.GetChild(0).gameObject.SetActive(true);
            dialogueParent.SetActive(true);
            playIdle = true;
            StartCoroutine(PlayerIdleAniamtion());
        
            TriggerDialogue();
            hadDialogue = true;

            //EventTrigger trigger = GameObject.Find("LeftButton").GetComponent<EventTrigger>().OnPointerDown.trigger
        }
    }

    IEnumerator PlayerIdleAniamtion()
    {
        while (playIdle)
        {
            FindObjectOfType<PlayerController>().GetComponent<Animator>().Play("Idle");
            yield return null;
        }
    }

    public void EndConversation()
    {
        Time.timeScale = 1;
        //hud.SetActive(true);
        //GameObject.Find("DialogueParent").transform.GetChild(0).gameObject.SetActive(false);
        dialogueParent.SetActive(false);
        playIdle = false;
        StopCoroutine(PlayerIdleAniamtion());
        //FindObjectOfType<PlayerController>().enabled = true;
        GameManager.Instance.ResumeBGMusic();
    }
}
