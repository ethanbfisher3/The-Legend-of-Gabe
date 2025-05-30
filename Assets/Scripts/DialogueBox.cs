using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    public Transform topLocation, bottomLocation;
    public Transform scrollSpritesParent;
    public Transform textSpritesParent;
    public Image letterPrefab;
    public Transform letterStartPosition;
    public Image escapeButtonImage;
    public float regularLetterSize = 40f;
    public float distanceX = 12f;
    public float distanceY = 70f;

    public bool Done { get; private set; }
    public bool IsOpen => scrollSpritesParent.gameObject.activeSelf;

    List<Image> textSpriteRenderers;
    Vector3 letterPosition;
    bool skipNextLine;
    bool started;

    void Start()
    {
        textSpriteRenderers = new List<Image>(textSpritesParent.GetComponentsInChildren<Image>());

        letterPosition = letterStartPosition.position;
        skipNextLine = false;
        Done = false;
        started = false;

        escapeButtonImage.gameObject.SetActive(false); // Hide the escape button initially
    }

    void Update()
    {
        if (started && Input.GetKeyDown(KeyCode.E))
        {
            skipNextLine = true;
        }

        if (Done && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)))
        {
            scrollSpritesParent.gameObject.SetActive(false); // Hide the text box when done
            Done = false; // Reset done state
            skipNextLine = false; // Reset skip state
            letterPosition = letterStartPosition.position; // Reset letter position for next use
            foreach (var spriteRenderer in textSpriteRenderers)
            {
                spriteRenderer.enabled = false; // Hide all sprites after finishing
            }
        }

        escapeButtonImage.gameObject.SetActive(Done); 
    }

    public void BeginDialogue(DialogueConfig config)
    {
        if (Done || !started)
            StartCoroutine(BeginDialogueEnumerator(config));
    }

    IEnumerator BeginDialogueEnumerator(DialogueConfig config)
    {
        started = true;
        scrollSpritesParent.gameObject.SetActive(true);
        SetLocation(config.location);
        int currentSprite = 0;

        foreach (var spriteRenderer in textSpriteRenderers)
        {
            spriteRenderer.enabled = false; // Hide all sprites initially
        }

        string text = config.text.ToUpper(); // Convert text to uppercase for consistency
        for (int i = 0; i < text.Length; i++)
        {
            char letter = text[i];
            if (letter == '`')
            {
                skipNextLine = false; // Reset skip state for new line
                letterPosition = new Vector3(letterStartPosition.position.x, letterPosition.y - distanceY, letterPosition.z); // Move to the next line
                continue; // Skip to the next iteration for the next character
            }

            if (i >= textSpriteRenderers.Count)
            {
                var newSprite = Instantiate(letterPrefab.gameObject, textSpritesParent).GetComponent<Image>();
                var rectTransform = newSprite.GetComponent<RectTransform>();
                var letterSize = regularLetterSize * (LetterSize(text[i]) + 2f) / 6f; // Calculate the size based on the letter
                rectTransform.sizeDelta = new Vector2(letterSize, regularLetterSize); // Set the size of the letter
                newSprite.gameObject.SetActive(true); // Ensure the new sprite is active
                textSpriteRenderers.Add(newSprite);
            }
            var sprite = GetSpriteForLetter(letter);
            var currentSpriteRenderer = textSpriteRenderers[currentSprite];
            currentSpriteRenderer.enabled = sprite != null; // Enable the sprite for the letter
            currentSpriteRenderer.sprite = sprite; // Set the sprite for the letter
            currentSpriteRenderer.transform.position = letterPosition;
            UpdateLetterPosition(letter);
            
            
            if (letter == '.' || letter == '!' || letter == '?')
            {
                yield return new WaitForSeconds(config.timePerNewLine); // Wait for the specified time before showing the next line
            }
            else if (!skipNextLine)
            {
                yield return new WaitForSeconds(config.timePerLetter); // Wait for the specified time before showing the next letter
            }

            currentSprite++;
        }

        Done = true;
        started = false; // Reset started state after finishing
    }

    Sprite GetSpriteForLetter(char letter)
    {
        int spriteIndex = letter - 'A'; // Assuming letters are A-Z
        if (spriteIndex >= 0 && spriteIndex < Resources.Instance.letterSprites.Length)
        {
            return Resources.Instance.letterSprites[spriteIndex].sprite;
        }

        // Punctuation handling
        if (letter == ',')
            return Resources.Instance.punctuationSprites[0].sprite; // Comma
        else if (letter == '.')
            return Resources.Instance.punctuationSprites[1].sprite; // Period
        else if (letter == '!')
            return Resources.Instance.punctuationSprites[2].sprite; // Exclamation mark
        else if (letter == '?')
            return Resources.Instance.punctuationSprites[3].sprite; // Question mark

        if (char.IsDigit(letter))
        {
            int numberIndex = letter - '0'; // Assuming numbers are 0-9
            if (numberIndex >= 0 && numberIndex < Resources.Instance.numberSprites.Length)
            {
                return Resources.Instance.numberSprites[numberIndex].sprite;
            }
        }

        return null; // Return null if the letter is not valid
    }

    void UpdateLetterPosition(char lastLetter)
    {
        letterPosition = new Vector3(letterPosition.x + LetterSize(lastLetter) * distanceX, letterPosition.y, letterPosition.z);
    }

    int LetterSize(char letter)
    {
        int letterPixelsWide = 4;
        if (letter == 'I' || letter == '?')
            letterPixelsWide = 3;

        else if (letter == 'M' || letter == 'n' || letter == 'T' || letter == 'V' || letter == 'W' || letter == 'X' || letter == 'Y' || letter == 'I')
            letterPixelsWide = 5;

        else if (letter == ',')
            letterPixelsWide = 2; // Comma is narrower
        else if (letter == '.' || letter == '!')
            letterPixelsWide = 1; // Period, exclamation mark, and question mark are slightly wider

        return letterPixelsWide;
    }

    void SetLocation(int location)
    {
        transform.SetParent(location == DialogueConfig.TOP ? topLocation : bottomLocation, false);
        // TODO Based on player location
    }
}
