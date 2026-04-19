using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class WhatToCheckInfo : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<GameObject> pages;
    [SerializeField] private GameObject decrementPageButton;
    [SerializeField] private GameObject incrementPageButton;

    private int currentPageIndex;

    //////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }
        pages[0].SetActive(true);
        
    }

    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        CheckWhichButtonsToDisplay();
    }

    //////////////////////////////////////////////////////////////////////////////
    public void IncrementPage()
    {
        SwitchPage(1);
    }

    //////////////////////////////////////////////////////////////////////////////
    public void DecrementPage()
    {
        SwitchPage(-1);
    }
    //////////////////////////////////////////////////////////////////////////////
    private void CheckWhichButtonsToDisplay()
    {
        incrementPageButton.SetActive(currentPageIndex + 1 < pages.Count);
        decrementPageButton.SetActive(currentPageIndex != 0);
    }

    //////////////////////////////////////////////////////////////////////////////
    private void SwitchPage(int direction)
    {
        pages[currentPageIndex].SetActive(false);
        currentPageIndex += direction;
        currentPageIndex = Mathf.Clamp(currentPageIndex, 0, pages.Count -1);
        pages[currentPageIndex].SetActive(true);
    }

    //////////////////////////////////////////////////////////////////////////////
}
