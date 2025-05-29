using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    public Transform topLocation, bottomLocation;
    public Transform scrollSpritesParent;
    public Transform textSpritesParent;
    public Image letterPrefab;
    public Transform letterStartPosition, letterStopPosition;
    public float regularLetterSize = 40f;
    public float distanceX = 12f;
    public float distanceY = 70f;

    public const int TOP = 0;
    public const int BOTTOM = 1;
    public const int BASED_ON_PLAYER = 2;

    List<Image> textSpriteRenderers;
    Vector3 letterPosition;

    void Start()
    {
        textSpriteRenderers = new List<Image>(textSpritesParent.GetComponentsInChildren<Image>());

        letterPosition = letterStartPosition.position;
    }

    public void ShowText(string text, int location = TOP)
    {
        StartCoroutine(ShowTextOverTimeEnumerator(text, location, 0.05f));
    }

    IEnumerator ShowTextOverTimeEnumerator(string text, int location = TOP, float time = 0.05f)
    {
        scrollSpritesParent.gameObject.SetActive(true);
        SetLocation(location);

        foreach (var spriteRenderer in textSpriteRenderers)
        {
            spriteRenderer.enabled = false; // Hide all sprites initially
        }

        text = text.ToUpper(); // Convert text to uppercase for consistency
        for (int i = 0; i < text.Length; i++)
        {
            if (i >= textSpriteRenderers.Count)
            {
                var newSprite = Instantiate(letterPrefab.gameObject, textSpritesParent).GetComponent<Image>();
                var rectTransform = newSprite.GetComponent<RectTransform>();
                var letterSize = regularLetterSize * (LetterSize(text[i]) + 2f) / 6f; // Calculate the size based on the letter
                rectTransform.sizeDelta = new Vector2(letterSize, regularLetterSize); // Set the size of the letter
                newSprite.gameObject.SetActive(true); // Ensure the new sprite is active
                textSpriteRenderers.Add(newSprite);
            }
            char letter = text[i];
            var sprite = GetSpriteForLetter(letter);
            textSpriteRenderers[i].enabled = sprite != null; // Enable the sprite for the letter
            textSpriteRenderers[i].sprite = sprite; // Set the sprite for the letter
            textSpriteRenderers[i].transform.position = letterPosition;
            UpdateLetterPosition(letter);

            yield return new WaitForSeconds(time); // Wait for the specified time before showing the next letter
        }
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

        // TODO Numbers handling
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
        int letterPixelsWide = LetterSize(lastLetter);
        letterPosition = new Vector3(letterPosition.x + letterPixelsWide * distanceX, letterPosition.y, letterPosition.z);
        if (letterPosition.x > letterStopPosition.position.x)
        {
            var startPosition = letterStartPosition.position;
            letterPosition = new Vector3(startPosition.x, startPosition.y - distanceY, startPosition.z); // Reset position if it exceeds the stop position
        }
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
        transform.SetParent(location == TOP ? topLocation : bottomLocation, false);
        // TODO Based on player location
    }
}
