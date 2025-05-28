using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public UnityEvent onGameStart;
    public UnityEvent onGameOver;

    public TextBox textBox;

    private void Start()
    {

    }
}