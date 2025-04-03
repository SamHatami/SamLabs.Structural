using System.Collections.Generic;
using Assets.Scripts.Structure.Factories;
using UnityEngine;

namespace Assets.Scripts.Structure.Managers
{
    public class TrussManager : MonoBehaviour
    {
        [SerializeField] private List<TrussStructure> Structures;
        [SerializeField] public TrussStructure ActiveStructure;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        [SerializeField] private TrussFactory _trussFactory;
        public TrussFactory TrussFactory => _trussFactory;

        private void Start()
        {
            Structures ??= new List<TrussStructure>();
            Initialize();
        }

        public void Initialize()
        {
            //TODO: In the future I should initialize entire members from a saved file
            CreateNewTrussStructure();
        }

        public void CreateNewTrussStructure()
        {
            ActiveStructure = TrussFactory.CreateStructure(this);
            AddStructure(ActiveStructure);
        }

        private void AddStructure(TrussStructure structure)
        {
            if(Structures.Contains(structure))
                return;
            Structures.Add(structure);
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.A)) ActiveStructure.CreateMember(new Vector3(0, 0, 0), new Vector3(1, 0, 0));

        }

        public void SetActiveStructure(TrussStructure structure)
        {
            ActiveStructure = structure;
        }

        public void AddLoad()
        {

        }
    }
}