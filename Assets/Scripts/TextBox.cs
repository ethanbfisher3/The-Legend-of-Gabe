using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class TextBox : MonoBehaviour
{
    public Transform topLocation, bottomLocation;

    public const int TOP = 0;
    public const int BOTTOM = 1;
    public const int BASED_ON_PLAYER = 2;

    public void ShowText(string text, int location = TOP)
    {
        gameObject.SetActive(true);
        SetLocation(location);

        //TODO
    }

    void SetLocation(int location)
    {
        transform.SetParent(location == TOP ? topLocation : bottomLocation, false);
    }
}
