using System.Collections.Generic;
using UnityEngine;

namespace Assets.Application.Structure
{
    public class TrussManager : MonoBehaviour
    {
        [SerializeField] private List<TrussStructure> Structures;
        [SerializeField] public TrussStructure ActiveStructure;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        private TrussFactory _trussFactory;

        private void Start()
        {
            _trussFactory = new TrussFactory();
            Structures ??= new List<TrussStructure>();
            Initalize();
        }

        public void Initalize()
        {
            //TODO: In the future I should initialize entire members from a saved file
            CreateNewTrussStructure();
        }

        public void CreateNewTrussStructure()
        {
            ActiveStructure = _trussFactory.CreateStructure(this);
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.A)) ActiveStructure.CreateMember(new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        }

        public void OnNodeClicked(TrussNode trussNode)
        {
            //if multiselect, add to selectedlists
        }

        public void OnNodeDragged(TrussNode draggedNode)
        {
        }
    }
}