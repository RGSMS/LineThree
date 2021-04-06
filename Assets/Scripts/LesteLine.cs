using UnityEngine;
using RGSMS;

public class LesteLine : MonoBehaviour
{
    public Line line;

    public int curves;

    public Point start;
    public Point final;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log(5.0f - (-2.0f));
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            line.StartDrawLine(start, final, 1.0f, curves);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            line.ClearDraw();
        }
    }
}
