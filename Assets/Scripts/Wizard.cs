using UnityEngine;

public class Wizard : MonoBehaviour
{
    public TriggerEvent triggerEvent;
    public SpriteRenderer talkToSprite;
    public DialogueConfig dialogue;

    bool canTalkTo = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        triggerEvent.onTriggerEnter.AddListener(() => canTalkTo = true);
        triggerEvent.onTriggerExit.AddListener(() => canTalkTo = false);
    }

    // Update is called once per frame
    void Update()
    {
        talkToSprite.enabled = canTalkTo;

        if (canTalkTo && Input.GetKeyDown(KeyCode.E))
        {
            GameManager.Instance.dialogueBox.BeginDialogue(dialogue);
            canTalkTo = false;
        }
    }

    void Talk()
    {
    }
}
