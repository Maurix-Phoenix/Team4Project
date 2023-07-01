using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class LevelEditorGrid : MonoBehaviour
{
    public Vector2 Position;
    public Vector2 PositionOffset;
    public Vector2Int Size;
    public int CellSize;

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        transform.position = Position + PositionOffset;
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        int numVerticesX = Size.x + 1; // +1 : for closing the top part of the grid
        int numVerticesY = Size.y + 1; // +1 : for closding the right part of the grid

        int numHorizontalLines = numVerticesX * Size.y;
        int numVerticalLines = numVerticesY * Size.x;
        int numLines = numHorizontalLines + numVerticalLines + 2; // +2: are the closing lines of the grid

        Vector3[] vertices = new Vector3[numLines * 2];
        int[] indices = new int[numLines * 2];

        int currentIndex = 0;

        // Horizontal lines
        for (int y = 0; y < Size.y; y++)
        {
            for (int x = 0; x < numVerticesX - 1; x++)
            {
                vertices[currentIndex] = new Vector3(x * CellSize, y * CellSize, 0);
                vertices[currentIndex + 1] = new Vector3((x + 1) * CellSize, y * CellSize, 0);

                indices[currentIndex] = currentIndex;
                indices[currentIndex + 1] = currentIndex + 1;

                currentIndex += 2;
            }
        }

        // Vertical lines
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < numVerticesY - 1; y++)
            {
                vertices[currentIndex] = new Vector3(x * CellSize, y * CellSize, 0);
                vertices[currentIndex + 1] = new Vector3(x * CellSize, (y + 1) * CellSize, 0);

                indices[currentIndex] = currentIndex;
                indices[currentIndex + 1] = currentIndex + 1;

                currentIndex += 2;
            }
        }

        // add the top grid line
        vertices[currentIndex] = new Vector3(0, Size.y * CellSize, 0);
        vertices[currentIndex + 1] = new Vector3(Size.x * CellSize, Size.y * CellSize, 0);

        indices[currentIndex] = currentIndex;
        indices[currentIndex + 1] = currentIndex + 1;

        // add the right grid line
        vertices[currentIndex + 2] = new Vector3(Size.x * CellSize, 0, 0);
        vertices[currentIndex + 3] = new Vector3(Size.x * CellSize, Size.y * CellSize, 0);

        indices[currentIndex + 2] = currentIndex + 2;
        indices[currentIndex + 3] = currentIndex + 3;

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.Lines, 0);

        meshFilter.mesh = mesh;
    }
}
