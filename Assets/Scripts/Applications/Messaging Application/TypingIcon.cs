using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class TypingIcon : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float delayBetweenNewDots;


    private int index;
    private bool waiting;

    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (!waiting)
        {
            StartCoroutine(IncrementIndex());
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(index >= i);
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    private IEnumerator IncrementIndex()
    {
        waiting = true;
        yield return new WaitForSeconds(delayBetweenNewDots);
        waiting = false;
        index++;
        if (index == transform.childCount)
        {
            index = 0;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
