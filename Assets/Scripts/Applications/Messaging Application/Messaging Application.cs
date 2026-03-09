using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

//////////////////////////////////////////////////////////////////////////////////
public class MessagingApplication : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject messagingAppUI;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject conversationUI;
    [SerializeField] private GameObject availableConversationsParent;

    [Header("Prefabs")]
    [SerializeField] private GameObject availableConversationPrefab;

    public List<MessagingRecipientSO> availableMessagingRecipients;
    public List<MessagingDialogueSO> availableConversations;

    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        menuUI.SetActive(true);
        conversationUI.SetActive(false);
        messagingAppUI.SetActive(false);

        UpdateMenuDisplay();
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void SelectRecipient() //MessagingRecipientSO recipient
    {
        menuUI.SetActive(false);
        conversationUI.SetActive(true);
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void ReturnToMenu()
    {
        menuUI.SetActive(true);
        conversationUI.SetActive(false);
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void ToggleVisibility()
    {
        messagingAppUI.SetActive(!messagingAppUI.activeSelf);
        if (!messagingAppUI.activeSelf) 
        {
            ReturnToMenu();
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void UpdateMenuDisplay()
    {
        //First removes previous available conversations
        foreach (Transform child in  availableConversationsParent.transform)
        {
            Destroy(child);
        }
        foreach (MessagingRecipientSO availRecipient in availableMessagingRecipients) 
        {
            GameObject availConvo = Instantiate(availableConversationPrefab, availableConversationsParent.transform);
            availConvo.GetComponent<AvailableConversation>().AssignValues(availRecipient.name, "");
            availConvo.GetComponent<Button>().onClick.AddListener(SelectRecipient);
        }

    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////


