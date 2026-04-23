using TMPro;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////
public class Message : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI text;


    private GameObject growthPivot;
    private float currentScale;
    private bool growing;

    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        growthPivot = transform.GetChild(0).gameObject;
        currentScale = 0.01f;
        growthPivot.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
        growing = true;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void FixedUpdate()
    {
        if (growing)
        {
            Grow();
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void SetText(string textToSet)
    {
        text.text = textToSet;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void Grow()
    {
        currentScale += GameManager.instance.messageGrowthSpeed;

        if (currentScale >= 1)
        {
            growthPivot.transform.localScale = new Vector3(1,1,1);
            growing = false;
        }
        else
        {
            growthPivot.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
