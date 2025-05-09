using System;
using System.Collections.Generic;
using Structure.Base;
using Structure.Factories;
using UnityEngine;

namespace Structure.Managers
{
    public class TrussManager : MonoBehaviour
    {
        [SerializeField] private List<TrussStructure> Structures;
        [SerializeField] public TrussStructure ActiveStructure;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        [SerializeField] private TrussFactory _trussFactory;
     
        public Action<List<TrussStructure>> OnStructureCollectionChanged;
        public Action<TrussStructure> OnActiveStructureChanged;
        
        public TrussFactory TrussFactory => _trussFactory;

        public List<TrussStructure> TrussStructures => Structures;
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
            OnActiveStructureChanged?.Invoke(ActiveStructure);
            AddStructure(ActiveStructure);
        }

        private void AddStructure(TrussStructure structure)
        {
            if (Structures.Contains(structure))
                return;
            Structures.Add(structure);
            structure.MemberCollectionChanged += MemberCollectionChanged;
        }

        private void MemberCollectionChanged(TrussMember obj)
        {
            OnStructureCollectionChanged?.Invoke(Structures);
        }

        // Update is called once per frame
        private void Update()
        {
            //move this to commandmanager
            if (Input.GetKeyUp(KeyCode.A)) ActiveStructure.CreateMember(new Vector3(0, 0, 0), new Vector3(1, 0, 0));
        }

        public void RemoveStructure(TrussStructure structure)
        {
            if(!Structures.Contains(structure))
                return;
            structure.MemberCollectionChanged -= MemberCollectionChanged;
            Structures.Remove(structure);
            OnStructureCollectionChanged?.Invoke(Structures);
        }

        public void SetActiveStructure(TrussStructure structure)
        {
            ActiveStructure = structure;
            OnActiveStructureChanged?.Invoke(ActiveStructure);
        }

        public void AddLoad()
        {
        }
    }
}