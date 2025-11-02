using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public SubsystemListPanel subsystemContainer;
    public ShipDetailsPanel shipDetails;
    public GameObject shipGallery;

    public TMP_Text fundsText;

    public AudioClip buttonPressAudio;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayButtonPressSound()
    {
        audioSource.clip = buttonPressAudio;
        audioSource.Play();
    }

    public void ShowShipGallery()
    {
        shipGallery.SetActive(shipGallery.activeSelf);
    }
}
