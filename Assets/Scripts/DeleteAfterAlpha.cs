using UnityEngine;
using TMPro;

public class DeleteAfterAlpha : MonoBehaviour
{
    public TextMeshProUGUI text;
    private void Update()
    {
        text.text = "Score: " + GameManager.instance.score.ToString();
    }
}
