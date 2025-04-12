using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Unity.PlasticSCM.Editor.WebApi.CredentialsResponse;

public class SubsystemSelectionMenu : MonoBehaviour
{
    public SubsystemSlot selectedSlot;
    
    public TextAsset jsonFile;

    public GameObject subsystemPrefab;

    public Transform subsystemContainer;

    public SubsystemsData subsystemsData = new SubsystemsData();

    void Start()
    {
        //LoadReactorSubsystems();
        //DisplayLoadedReactors();
    }

    void LoadReactorSubsystems()
    {
        if (jsonFile == null)
        {
            Debug.LogError("JSON file invalid");
            return;
        }

        try
        {
            // Fix the JSON format to work with JsonUtility
            string jsonText = "{\"Reactors\":" + ExtractArrayFromJson(jsonFile.text, "Reactors") + "}";

            // Parse the JSON using Unity's JsonUtility
            SubsystemsData parsedJson = JsonUtility.FromJson<SubsystemsData>(jsonText);

            if (parsedJson != null/* && parsedJson.reactors != null*/)
            {
                List<ReactorSubsystem> reactors = new List<ReactorSubsystem>();

                foreach (ReactorSubsystem reactorJson in parsedJson.reactors)
                {
                    ReactorSubsystem reactor = new ReactorSubsystem();
                    reactor.displayName = reactorJson.displayName;
                    reactor.description = reactorJson.description;
                    reactor.power = reactorJson.power;

                    // Parse type which could be int or string in JSON
                    reactor.type = reactorJson.type;

                    reactor.mass = reactorJson.mass;

                    reactor.icon = reactorJson.icon;

                    //reactor.rank = reactorJson.rank;
                    reactor.powerOutput = reactorJson.powerOutput;

                    // Parse powerType which could be int or string in JSON
                    reactor.powerType = reactorJson.powerType;

                    reactor.efficiency = reactorJson.efficiency;

                    reactors.Add(reactor);
                }

                subsystemsData.reactors = reactors.ToArray();
                Debug.Log($"Successfully loaded {subsystemsData.reactors.Length} reactors");
            }

            // Initialize empty arrays for other subsystem types for now
            subsystemsData.shieldGenerators = new ShieldGeneratorSubsystem[0];
            subsystemsData.lifeSupports = new LifeSupportSubsystem[0];
        }
        catch (Exception e)
        {
            Debug.LogError($"Error parsing JSON: {e.Message}");
        }
    }

    // Helper method to extract a specific array from the JSON text
    private string ExtractArrayFromJson(string jsonText, string arrayName)
    {
        int startIndex = jsonText.IndexOf("\"" + arrayName + "\"");
        if (startIndex == -1) return "[]";

        startIndex = jsonText.IndexOf("[", startIndex);
        if (startIndex == -1) return "[]";

        int endIndex = startIndex;
        int bracketCount = 1;

        for (int i = startIndex + 1; i < jsonText.Length; i++)
        {
            if (jsonText[i] == '[') bracketCount++;
            else if (jsonText[i] == ']') bracketCount--;

            if (bracketCount == 0)
            {
                endIndex = i;
                break;
            }
        }

        if (endIndex > startIndex)
        {
            return jsonText.Substring(startIndex, endIndex - startIndex + 1);
        }

        return "[]";
    }

    // Helper method to load sprite from path
    private Sprite LoadSpriteFromPath(string path)
    {
        // Option 1: If your sprites are in Resources folder
        if (!string.IsNullOrEmpty(path))
        {
            string resourcePath = path.Replace(".jpg", "").Replace(".png", "");
            return Resources.Load<Sprite>(resourcePath);
        }

        return null;
    }

    // Example method to display loaded reactors
    private void DisplayLoadedReactors()
    {
        if (subsystemsData == null || subsystemsData.reactors == null)
            return;

        foreach (var reactor in subsystemsData.reactors)
        {
            Debug.Log($"Reactor: {reactor.displayName}");
            Debug.Log($"  Description: {reactor.description}");
            Debug.Log($"  Power Output: {reactor.powerOutput}");
            Debug.Log($"  Type: {reactor.type}");
            Debug.Log($"  Power Type: {reactor.powerType}");
            Debug.Log($"  Mass: {reactor.mass}");
            //Debug.Log($"  Rank: {reactor.rank}");
            Debug.Log("---------------------");
        }
    }

    // Method to get all reactors
    public ReactorSubsystem[] GetAllReactors()
    {
        return subsystemsData?.reactors;
    }

    // Method to get reactor by name
    public ReactorSubsystem GetReactorByName(string name)
    {
        if (subsystemsData?.reactors == null)
            return null;

        foreach (var reactor in subsystemsData.reactors)
        {
            if (reactor.displayName == name)
                return reactor;
        }

        return null;
    }

    void CreateReactorSubsystemUI(ReactorSubsystem reactor)
    {
        // Instantiate the prefab
        GameObject subsystemEntry = Instantiate(subsystemPrefab, subsystemContainer);

        // Find and set the text components
        Text nameText = subsystemEntry.transform.Find("NameText")?.GetComponent<Text>();
        Text descriptionText = subsystemEntry.transform.Find("DescriptionText")?.GetComponent<Text>();
        Text detailsText = subsystemEntry.transform.Find("DetailsText")?.GetComponent<Text>();
        Image iconImage = subsystemEntry.transform.Find("IconImage")?.GetComponent<Image>();

        // Set the text
        if (nameText) nameText.text = reactor.displayName;
        if (descriptionText) descriptionText.text = reactor.description;

        // Add additional details specific to ReactorSubsystem
        if (detailsText)
        {
            detailsText.text = $"Power Type: {reactor.powerType}\n" +
                                $"Power Output: {reactor.powerOutput} MW\n" +
                                $"Efficiency: {reactor.efficiency * 100:F2}%\n" +
                                $"Mass: {reactor.mass} kg";
        }

        // Set the icon
        //if (iconImage) iconImage.sprite = reactor.icon;
    }

    public void SetSelectedSlot(SubsystemSlot slot)
    {
        selectedSlot = slot;
    }

    public void AddSubsystemToSlot(Subsystem subsystem)
    {
        //selectedSlot.SetSubsystem(subsystem);

        //ShipManager.Instance.currentShip.GetComponent<ShipBase>().subsystems.Add(subsystem);

        //print(subsystem.displayName);
    }
}
