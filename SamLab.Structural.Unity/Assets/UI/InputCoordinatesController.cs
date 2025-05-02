using System;
using Structure.Managers;
using UnityEngine;
using UnityEngine.UIElements;

public class InputCoordinatesController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TrussManager trussManager;

    private void OnEnable()
    {
        trussManager = FindFirstObjectByType<TrussManager>();
        
        VisualElement root = FindFirstObjectByType<UIDocument>().rootVisualElement;
        
        
        
    }
}
