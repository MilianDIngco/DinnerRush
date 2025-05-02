using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public int tableID;
    public GameObject GameManagerObject;
    GameManager manager;
    public Material cleanMaterial;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManagerObject == null)
        {
            GameManagerObject = GameObject.Find("GameManager"); // name must match
        }

        manager = GameManagerObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Bus()
    {
        manager.BusTable(tableID);
        GetComponent<Renderer>().material = cleanMaterial;
    }
}
