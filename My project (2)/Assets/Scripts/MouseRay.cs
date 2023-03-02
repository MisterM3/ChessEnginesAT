using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetMousePosition();
    }

    public static Vector3 GetMousePosition()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out RaycastHit hit);


    
        return hit.point + new Vector3(0.5f, 0, 0.5f);

    }

    public static Vector2Int GetMouseGridPosition()
    {
        Vector3 positon = GetMousePosition();

        return new Vector2Int((int)positon.x, (int)positon.z);
    }
}
