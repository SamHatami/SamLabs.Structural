
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

        private SelectionInteraction _selectionHandler;
        private TrussNode _node { get; set; } // I need to re-bind each time this is changed?

        [CreateProperty]
        public float PositionX
        {
            get => _node!= null ? _node.transform.position.x : 0f;
            set
            {
                if (Mathf.Abs(_node.transform.position.x - value) > 0.0001f)
                {
                    var p = _node.transform.position;
                    _node.transform.position = new Vector3(value, p.y, p.z);
                    Notify(nameof(PositionX));
                }
            }
        }

        [CreateProperty]
        public float PositionY
        {
            get => _node != null ? _node.transform.position.y : 0f;
            set
            {
                if (Mathf.Abs(_node.transform.position.y - value) > 0.0001f)
                {
                    var p = _node.transform.position;
                    _node.transform.position = new Vector3(p.x, value, p.z);
                    Notify(nameof(PositionY));
                }
            }
        }

        [CreateProperty]
        public float PositionZ
        {
            get => _node != null ? _node.transform.position.z: 0f;
            set
            {
                if (Mathf.Abs(_node.transform.position.z - value) > 0.0001f)
                {
                    var p = _node.transform.position;
                    _node.transform.position = new Vector3(p.x, p.y, value);
                    Notify(nameof(PositionZ));
                }
            }
        }
        private void Notify([CallerMemberName] string property = "")
        {
            propertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(property));
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
            
            Vector3 currentPos = _node.transform.position;
            
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
            if(_node != null)
                _node.OnPositionChanged -= UpdateNodePosition;
            
            _node = newNode;
            if(_node != null)
                _node.OnPositionChanged += UpdateNodePosition;

        }
        public void UpdateNodePosition(TrussNode node)
        {
            SetNode(node);
            
            if (_node != null)
            {
                Vector3 currentPos = _node.transform.position;

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