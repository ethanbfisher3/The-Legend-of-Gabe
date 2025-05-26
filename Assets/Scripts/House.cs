using Unity.VisualScripting;
using UnityEngine;

public class House : MonoBehaviour
{
    public TriggerEvent doorTrigger;
    public SpriteRenderer enterSprite;

    bool canEnter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doorTrigger.onTriggerEnter.AddListener(() => canEnter = true);
        doorTrigger.onTriggerExit.AddListener(() => canEnter = false);
    }

    // Update is called once per frame
    void Update()
    {
        enterSprite.enabled = canEnter;

        if (canEnter && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Entering house");
        }
    }
}
