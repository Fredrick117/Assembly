using System;
using UnityEngine;
using UnityEngine.UI;

public enum CurrentView
{
    Store,
    Ship,
}

public class Sidebar : MonoBehaviour
{
    public CurrentView currentView;
    
    public Button storeButton;
    public Button shipButton;

    public GameObject shipPanel;
    public GameObject storePanel;
    
    private void OnEnable()
    {
        storeButton.onClick.AddListener(OpenStoreView);
        shipButton.onClick.AddListener(OpenShipView);
    }

    private void OnDisable()
    {
        storeButton.onClick.RemoveAllListeners();
        shipButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        shipButton.interactable = false;
    }

    private void OpenStoreView()
    {
        storeButton.interactable = false;
        shipButton.interactable = true;
        
        storePanel.SetActive(true);
        shipPanel.SetActive(false);
    }

    private void OpenShipView()
    {
        storeButton.interactable = true;
        shipButton.interactable = false;
        
        storePanel.SetActive(false);
        shipPanel.SetActive(true);
    }
}
