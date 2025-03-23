using UnityEngine;

namespace Assets.Scripts.Workspace.UI
{
    //Source https://discussions.unity.com/t/in-game-infinite-grids/604479/5
    public class DynamicGrid : MonoBehaviour
    {
        public Material GLMat;
        public int gridCount = 5; //number of grids to draw on each side of the look position (half size)
        public float gridSize = 1.0f; //spacing between gridlines

        Ray ray;
        float rayDist;
        Vector3 lookPosition;
        Plane world = new Plane(Vector3.up, Vector3.zero); //world plane to draw the grid on
        UnityEngine.Camera cam;



        void Start()
        {
            cam = GetComponent<UnityEngine.Camera>();
            GLMat.renderQueue = 1;

        }

        void Update()
        {
            //ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            //world.Raycast(ray, out rayDist);
            //lookPosition = ray.GetPoint(rayDist);
        }

        void OnPostRender()
        {
            GL.PushMatrix();
            GLMat.SetPass(0);
            GL.Begin(GL.LINES);

            Vector3 rounedPos = new Vector3(Round(lookPosition.x), 0, Round(lookPosition.z));

            //Actual look position
            //GL.Color(Color.black);
            //GL.Vertex(lookPosition);
            //GL.Vertex(lookPosition + Vector3.up);

            GL.Color(Color.white);

            //Major x line
            GL.Vertex( new Vector3(gridCount * gridSize, 0, 0));
            GL.Vertex( new Vector3(-gridCount * gridSize, 0, 0));
            //Major z line
            GL.Vertex( new Vector3(0, 0, gridCount * gridSize));
            GL.Vertex( new Vector3(0, 0, -gridCount * gridSize));

            GL.Color(Color.red);

            for (int i = 1; i < gridCount + 1; i++)
            {
                //positive x lines
                GL.Vertex(new Vector3(i * gridSize, 0, gridCount * gridSize));
                GL.Vertex(new Vector3(i * gridSize, 0, -gridCount * gridSize));
                //negative x lines
                GL.Vertex(new Vector3(-i * gridSize, 0, gridCount * gridSize));
                GL.Vertex(new Vector3(-i * gridSize, 0, -gridCount * gridSize));
                //positive z lines
                GL.Vertex(new Vector3(gridCount * gridSize, 0, i * gridSize));
                GL.Vertex(new Vector3(-gridCount * gridSize, 0, i * gridSize));
                //negative z lines
                GL.Vertex(new Vector3(gridCount * gridSize, 0, -i * gridSize));
                GL.Vertex(new Vector3(-gridCount * gridSize, 0, -i * gridSize));


            }

            GL.End();
            GL.PopMatrix();
        }

        float Round(float x)
        {
            return Mathf.Round(x / gridSize) * gridSize;
        }
    }
}
