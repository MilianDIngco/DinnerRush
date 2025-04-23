using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public struct Recipe
    {
        public List<Ingredient.IngredientType> toppings;
        public int cookedness;
        public int numSlices;
    }

    Recipe desired;
    public int queuePosition;
    public int waitPosition;

    public void Awake()
    {
        // When a character is made, a random recipe is generated
        desired = GenerateRecipe();
    }

    public void MoveToQueue()
    {

    }

    public void MoveToWait()
    {

    }

    public void Leave()
    {

    }

    public void PrintRecipe()
    {
        string print = "";
        print += ("I want ");
        for (int i = 0; i < desired.toppings.Count; i++)
        {
            print += (desired.toppings[i] + ", ");
        }
        print += ("cooked at " + desired.cookedness + " cookedness, ");
        print += ("and " + desired.numSlices + " slices please!");

        Debug.Log(print);
    }

    // Generates a recipe randomly
    private Recipe GenerateRecipe()
    {
        Recipe recipe = new Recipe();
        recipe.toppings = new List<Ingredient.IngredientType>();

        recipe.cookedness = Mathf.RoundToInt(Random.Range(0, 10));
        recipe.numSlices = Mathf.RoundToInt(Random.Range(0, 4));
        
        // Set plate
        recipe.toppings.Add((Ingredient.IngredientType)0);

        // Set sauce
        int minSauce = 2;
        int maxSauce = 2;
        recipe.toppings.Add(
            (Ingredient.IngredientType)Mathf.RoundToInt(Random.Range(minSauce, maxSauce))
            );

        // Set toppings
        int nToppings = Mathf.RoundToInt(Random.Range(1, 4));

        int minTopping = 3;
        int maxTopping = 6;
        for (int i = 0; i < nToppings; i++)
        {
            recipe.toppings.Add(
            (Ingredient.IngredientType)Mathf.RoundToInt(Random.Range(minTopping, maxTopping))
            );
        }

        return recipe;
    }

    // I'm aware this scoring is far too lenient, but how its calculated can be changed later
    public int scoreResult(Recipe created)
    {
        // Calculate closeness to cookedness
        int cookednessAccuracy = normalAccuracy(created.cookedness, desired.cookedness);

        // CANNOT BE BOTHERED TO FIND HOW CLOSE A CUT IS TO A LINE
        int cutAccuracy = normalAccuracy(created.numSlices, desired.numSlices);

        // Calculate number of correct toppings
        int total = desired.toppings.Count;
        int correct = 0;
        for (int i = 0; i < created.toppings.Count; i++)
        {
            if (desired.toppings.Contains(created.toppings[i]))
            {
                correct++;
            }
        }

        int toppingAccuracy = normalAccuracy(correct, total);

        return (cookednessAccuracy + cutAccuracy + toppingAccuracy) / 3;
    }

    public int normalAccuracy(float x, float mu)
    {
        float sigma = 0.35f;
        float normDistA = (float)(1 / (sigma * Mathf.Sqrt(2 * Mathf.PI)));
        float normDistEXP = (float)Mathf.Exp(-0.5f * Mathf.Pow((x - mu) / sigma, 2));
        return Mathf.RoundToInt(Mathf.Max(normDistA * normDistEXP * 100, 100));
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Pizza")
        {
            UnityEngine.Debug.Log(scoreResult(col.gameObject.GetComponent<Pizza>().recipe));
        }
    }
}
