using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GridManager : MonoBehaviour
{
    private Camera mainCamera;

    public GameObject gridCellPrefab;

    public GameObject modulePrefab;
    
    private GameObject previewObject;

    public ModuleBase previewModule;

    public Color previewColor;
    public Sprite previewSprite;

    public bool isPlacementMode = false;

    private void GenerateGridRepresentation()
    {
        for (int i = -24; i < 24; i++)
        {
            for (int j = -10; j < 10; j++)
            {
                Instantiate(gridCellPrefab, new Vector3(i, j), gridCellPrefab.transform.rotation);
            }
        }
    }
    
    private void Start()
    {
        mainCamera = Camera.main;
        
        //GenerateGridRepresentation();
    }

    private void Update()
    {
        if (!isPlacementMode) return;
        
        Vector2 worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
        //int x = Mathf.CeilToInt(worldPosition.x);
        //int y = Mathf.CeilToInt(worldPosition.y);
        int x = Mathf.RoundToInt(worldPosition.x);
        int y = Mathf.RoundToInt(worldPosition.y);
        
        Vector2Int gridPosition = new Vector2Int(x, y);

        //print($"Grid position: [{gridPosition.x}, {gridPosition.y}]");
        
        previewObject.transform.position = new Vector3(gridPosition.x, gridPosition.y, 0);

        if (Input.GetMouseButtonDown(0))
        {
            PlaceObject(gridPosition);
        }
    }

    public void CreatePreviewObject(ModuleBase module)
    {
        previewObject = new GameObject("PreviewObject");
        SpriteRenderer renderer = previewObject.AddComponent<SpriteRenderer>();
        renderer.color = previewColor;
        renderer.sprite = module.sprite;
        previewObject.transform.localScale = new Vector3(module.size.x, module.size.y, 1);
    }

    private void PlaceObject(Vector2Int gridLocation)
    {
        GameObject newObject = Instantiate(modulePrefab, (Vector2)gridLocation, modulePrefab.transform.rotation);

        newObject.GetComponent<SpriteRenderer>().sprite = previewModule.sprite;
        newObject.transform.localScale = new Vector3(previewModule.size.x, previewModule.size.y, 1);
    }
}