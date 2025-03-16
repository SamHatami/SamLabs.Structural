using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamLab.Structural.Core.Elements;
using UnityEngine;

namespace Assets.Application.Structure
{
    public class TrussNode : MonoBehaviour
    {
        public NodeData NodeData { get; set; }
        private TrussManager _manager;
        public void Initialize(TrussManager manager)
        {
            _manager = manager;
        }


        void OnMouseDown()
        {
            //_manager.OnNodeClicked(this);s
        }

        void OnMouseDrag()
        {
            //TODO : Do I really need to create a new Vector3 every frame?,
            //TODO:  I will need some transforms gizmos here. 
            //TODO: spacemovement should be dependant of the direction of the drag.
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = objPosition;
            
            //_manager.OnNodeDragged(this);
        }


    }
}
