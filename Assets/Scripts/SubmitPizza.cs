using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitPizza : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;

        if (go.tag == "Pizza")
        {
            Debug.Log("SUBMITTED PIZZA");
            GameManager.Instance.SubmitPizza(go);
            Destroy(go);
        }
    }
}
