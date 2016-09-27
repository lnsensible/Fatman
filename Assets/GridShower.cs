using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GridShower : MonoBehaviour {

    public struct Grid
    {
        public bool isTraversable;
        public Vector3 position;

        public Grid(Vector3 pos)
        {
            isTraversable = true;
            position = pos;
        }
    };

    public float nodeSize;

    public int width;
    public int depth;

    public Grid[,] gridNodes;

    public bool Restart;
    public bool ShowGrid;

    void Initialise()
    {
        gridNodes = new Grid[width, depth];
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < depth; ++j)
            {
                gridNodes[i, j].position = new Vector3(i, 0, j);
            }
        }
    }
    
	// Use this for initialization
	void Start () {
        Initialise();
	}

    void Update()
    {
        if (Restart)
        {
            Restart = false;
            Initialise();
        }
    }

    void OnDrawGizmos()
    {
        if (ShowGrid && gridNodes != null)
        {
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < depth; ++j)
                {
                    //if (gridNodes[i, j].isTraversable)
                    //{
                        Vector3 pos = gridNodes[i, j].position;
                        Vector3[] verts = new Vector3[4];
                        var _nodeSize = nodeSize * 0.5f;

                        verts[0] = new Vector3(pos.x - _nodeSize, pos.y + 0.01f, pos.z - _nodeSize);
                        verts[1] = new Vector3(pos.x - _nodeSize, pos.y + +0.01f, pos.z + _nodeSize);
                        verts[2] = new Vector3(pos.x + _nodeSize, pos.y + 0.01f, pos.z + _nodeSize);
                        verts[3] = new Vector3(pos.x + _nodeSize, pos.y + 0.01f, pos.z - _nodeSize);

                        Gizmos.color = Color.blue;
                        Gizmos.DrawLine(verts[0], verts[1]);
                        Gizmos.DrawLine(verts[1], verts[2]);
                        Gizmos.DrawLine(verts[2], verts[3]);
                        Gizmos.DrawLine(verts[3], verts[0]);

                        //Handles.DrawPolyLine(verts);
                    //}
                }
            }
        }
    }
}
