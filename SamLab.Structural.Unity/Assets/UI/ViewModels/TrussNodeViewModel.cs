
using System;
using System.Runtime.CompilerServices;
using Structure.Base;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using Workspace.Interaction;

namespace UI.ViewModels
{
    public class TrussNodeViewModel: MonoBehaviour, INotifyBindablePropertyChanged
    {
        public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;
        private void Notify([CallerMemberName] string property = "")
        {
            propertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(property));
        }

        private SelectionInteraction _selectionHandler;
        private TrussNode Node { get; set; }

        [CreateProperty]
        public float PositionX
        {
            get => Node!= null ? Node.transform.position.x : 0f;
            set
            {
                if (Mathf.Abs(Node.transform.position.x - value) > 0.0001f)
                {
                    var p = Node.transform.position;
                    Node.transform.position = new Vector3(value, p.y, p.z);
                    Notify(nameof(PositionX));
                }
            }
        }

        [CreateProperty]
        public float PositionY
        {
            get => Node != null ? Node.transform.position.y : 0f;
            set
            {
                if (Mathf.Abs(Node.transform.position.y - value) > 0.0001f)
                {
                    var p = Node.transform.position;
                    Node.transform.position = new Vector3(p.x, value, p.z);
                    Notify(nameof(PositionY));
                }
            }
        }

        [CreateProperty]
        public float PositionZ
        {
            get => Node != null ? Node.transform.position.z: 0f;
            set
            {
                if (Mathf.Abs(Node.transform.position.z - value) > 0.0001f)
                {
                    var p = Node.transform.position;
                    Node.transform.position = new Vector3(p.x, p.y, value);
                    Notify(nameof(PositionZ));
                }
            }
        }


        private void Awake()
        {
            _selectionHandler = FindFirstObjectByType<SelectionInteraction>();
            if (_selectionHandler != null)
            {
                _selectionHandler.NodeSelectionEvent += HandleNodeSelected;
            }
        }

        private void HandleNodeSelected(TrussNode newNode)
        {
    
            SetNode(newNode);
            
            Vector3 currentPos = Node.transform.position;
            
            PositionX = currentPos.x;
            PositionY = currentPos.y;
            PositionZ = currentPos.z;
            
            Notify(nameof(PositionX));
            Notify(nameof(PositionY));
            Notify(nameof(PositionZ));
            
            // Notify(nameof(IsShared));
            // Notify(nameof(IsMovable));
        }

        private void SetNode(TrussNode newNode)
        {
            if(Node != null)
                Node.OnPositionChanged -= UpdateNodePosition;
            
            Node = newNode;
            if(Node != null)
                Node.OnPositionChanged += UpdateNodePosition;

        }
        public void UpdateNodePosition(TrussNode node)
        {
            SetNode(node);
            
            if (Node != null)
            {
                Vector3 currentPos = Node.transform.position;

                PositionX = currentPos.x;
                PositionY = currentPos.y;
                PositionZ = currentPos.z;
                
                Notify(nameof(PositionX));
                Notify(nameof(PositionY));
                Notify(nameof(PositionZ));
            }
            else
            {
                Notify(nameof(PositionX));
                Notify(nameof(PositionY));
                Notify(nameof(PositionZ));

            }
        }
        
    }
}