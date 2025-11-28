using UnityEngine;
using UnityEngine.UI;

public class LeverSwitchUI : MonoBehaviour
{
    public Sprite blueLever;
    public Sprite redLever;

    private Image img;
    private bool isRed = false;

    void Start()
    {
        img = GetComponent<Image>();
        img.sprite = blueLever;
    }

    public void OnClickLever()
    {
        isRed = !isRed;
        img.sprite = isRed ? redLever : blueLever;
    }
}
