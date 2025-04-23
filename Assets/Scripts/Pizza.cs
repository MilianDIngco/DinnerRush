using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pizza;

public class Pizza : MonoBehaviour
{
    public Customer.Recipe recipe;

    // Start is called before the first frame update
    void Start()
    {
        recipe = new Customer.Recipe();
        recipe.toppings = new List<Ingredient.IngredientType>();
        recipe.toppings.Add((Ingredient.IngredientType)0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ingredient")
        {
            GameObject ingredient = col.gameObject;

            Vector3 originalWorldScale = ingredient.transform.lossyScale;

            ingredient.transform.SetParent(gameObject.transform);

            ingredient.transform.localPosition = Vector3.zero;
            ingredient.transform.localRotation = Quaternion.identity;

            ingredient.transform.localScale = new Vector3(
                originalWorldScale.x / gameObject.transform.lossyScale.x,
                originalWorldScale.y / gameObject.transform.lossyScale.y,
                originalWorldScale.z / gameObject.transform.lossyScale.z
            );

            ingredient.GetComponent<Collider>().enabled = false;
            ingredient.GetComponent<Rigidbody>().isKinematic = true;
            ingredient.GetComponent<Rigidbody>().detectCollisions = false;

            Ingredient.IngredientType type = ingredient.GetComponent<Ingredient>().ingredientType;

            recipe.toppings.Add(type);

            if(type == Ingredient.IngredientType.TomatoSauce)
            {
                ingredient.transform.localScale = new Vector3(ingredient.transform.localScale.x * 2.8f, ingredient.transform.localScale.y * 0.18f, ingredient.transform.localScale.z * 2.8f);
            }
            else if(type != Ingredient.IngredientType.PizzaCrust)
            {
                if (type == Ingredient.IngredientType.Pepperoni)
                    ingredient.transform.localScale = new Vector3(ingredient.transform.localScale.x * 0.9f, ingredient.transform.localScale.y * 6, ingredient.transform.localScale.z * 0.9f);
                else
                    ingredient.transform.localScale = new Vector3(ingredient.transform.localScale.x * 1.1f, ingredient.transform.localScale.y * 0.5f, ingredient.transform.localScale.z * 1.1f);
                float radius = 0.25f;
                float randomAngle = Random.Range(0f, 2f * Mathf.PI);
                float randomRadius = radius * Mathf.Sqrt(Random.Range(0f, 1f));

                float xPos = randomRadius * Mathf.Cos(randomAngle);
                float zPos = randomRadius * Mathf.Sin(randomAngle);

                ingredient.transform.localPosition = new Vector3(xPos, 0, zPos);
            }
        }
        else if(col.gameObject.tag == "PizzaCutter")
        {
            recipe.numSlices++;
            UnityEngine.Debug.Log("NumSlices: " +  recipe.numSlices);
        }
    }
}
