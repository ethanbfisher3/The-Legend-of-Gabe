using UnityEngine;

[System.Serializable]
public class DialogueConfig
{
    public const int TOP = 0;
    public const int BOTTOM = 1;
    public const int BASED_ON_PLAYER = 2;

    public int location = TOP;
    public string text;
    public float timePerLetter = 0.1f;
    public float timePerNewLine = 1f;
}