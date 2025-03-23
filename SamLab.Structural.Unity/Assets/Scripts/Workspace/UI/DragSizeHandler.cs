using UnityEngine;

namespace Assets.Scripts.Workspace.UI
{
    public class DragSizeHandler : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnMouseEnter()
        {
            Debug.Log("Mouse is over GameObject.");
            //Switch material to nr 2
            GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Red");
        }
        private void OnMouseDown()
        {
            Debug.Log("Mouse is down.");
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Mouse is over GameObject.");
            }
        }

        private void OnMouseExit()
        {
            Debug.Log("Mouse is no longer over GameObject.");
        }

        private void OnMouseDrag()
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 objPosition = UnityEngine.Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = objPosition;
        }


    }
}
