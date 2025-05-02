using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DevUI : EditorWindow
{
    [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/DevUI")]
    public static void ShowExample()
    {
        var wnd = GetWindow<DevUI>();
        wnd.titleContent = new GUIContent("DevUI");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        var root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
    }
}