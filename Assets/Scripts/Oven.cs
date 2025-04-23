using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
    public bool cooking = false;
    GameObject pizzaCooking;
    [SerializeField] GameObject ovenLight;
    [SerializeField] Material off, on, onTick;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        if (!cooking)
        {
            cooking = true;
            if (col.gameObject.tag == "Pizza")
            {
                ovenLight.GetComponent<MeshRenderer>().material = on;

                col.transform.SetParent(gameObject.transform);
                pizzaCooking = col.gameObject;
                pizzaCooking.GetComponent<Rigidbody>().isKinematic = true;
                pizzaCooking.GetComponent<Collider>().enabled = false;
                pizzaCooking.GetComponent<MeshRenderer>().enabled = false;
                pizzaCooking.SetActive(false);

                StartCoroutine(IncreaseCookedness());
            }
        }
    }

    public GameObject StopCooking()
    {
        if (cooking)
        {
            cooking = false;
            ovenLight.GetComponent<MeshRenderer>().material = off;

            pizzaCooking.SetActive(true);

            pizzaCooking.GetComponent<MeshRenderer>().enabled = true;
            pizzaCooking.GetComponent<Rigidbody>().isKinematic = false;
            StopCoroutine("IncreaseCookedness");

            return pizzaCooking;
        }
        return null;
    }

    IEnumerator IncreaseCookedness()
    {
        yield return new WaitForSeconds(2);
        if(cooking)
        {
            ovenLight.GetComponent<MeshRenderer>().material = onTick;

            var tempRecipe = pizzaCooking.GetComponent<Pizza>().recipe;
            tempRecipe.cookedness++;
            pizzaCooking.GetComponent<Pizza>().recipe = tempRecipe;
        }
        yield return new WaitForSeconds(0.5f);
        if (cooking)
        {
            ovenLight.GetComponent<MeshRenderer>().material = on;

            UnityEngine.Debug.Log("Cookedness: " + pizzaCooking.GetComponent<Pizza>().recipe.cookedness);
            StartCoroutine(IncreaseCookedness());
        }
    }
}