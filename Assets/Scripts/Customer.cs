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
    public bool ordering = false;
    public bool waiting = false;
    public bool moving = false;
    public GameObject go;
    public StressLevel stressMeter;

    public CustomerUI cui;

    public int tableID = -1;

    public float scoringHarshness = 1.5f;
    public void Awake()
    {
        // When a character is made, a random recipe is generated
        desired = GenerateRecipe();
        cui = GetComponent<CustomerUI>();
        cui.SetRecipe(desired);

        go = this.gameObject;
    }

    public void Update()
    {
        
    }

    public void TakeOrder()
    {
        PrintRecipe();

        StartCoroutine(showRecipe());
        
    }

    public IEnumerator showRecipe()
    {
        cui.ShowCanvas(true);
        yield return new WaitForSeconds(6);

        if (queuePosition == 0 && ordering)
        {
            ordering = false;
            waiting = true;
            GameManager.Instance.PopCustomer(true);
        }
    }

    public void GiveOrder()
    {
        Debug.Log("Gave order");
        if (queuePosition == 0 && waiting)
        {
            if (GameManager.Instance.PopCustomer(false))
            {
                waiting = false;
                Debug.Log("Gave order");
            }
        }
    }

    public IEnumerator MoveToQueue(List<Vector3> destinations, float speed)
    {
        moving = true;
        foreach (Vector3 dest in destinations)
        {
            yield return StartCoroutine(MoveTo(dest, speed));
        }
        moving = false;
    }

    IEnumerator MoveTo(Vector3 destination, float speed)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        float distance = Vector3.Distance(transform.position, destination);
        float duration = distance / speed;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, destination, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = destination;
    }

    public void Leave()
    {
        Vector3 leave = transform.position - new Vector3(0, -10, 0);
        List<Vector3> positions = new List<Vector3>();
        positions.Add(leave);

        StartCoroutine(MoveToQueue(positions, 1));
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
        recipe.numSlices = Mathf.RoundToInt(Random.Range(1, 4));
        
        // Set plate
        recipe.toppings.Add((Ingredient.IngredientType)0);

        // Set sauce
        int minSauce = Ingredient.firstSauce;
        int maxSauce = Ingredient.firstTopping;
        recipe.toppings.Add(
            (Ingredient.IngredientType)Mathf.RoundToInt(Random.Range(minSauce, maxSauce))
            );

        // Set toppings
        int nToppings = 2; // Mathf.RoundToInt(Random.Range(1, 4));

        int minTopping = Ingredient.firstTopping;
        int maxTopping = Ingredient.last + 1;
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
        int cookednessAccuracy = normalAccuracy((float) created.cookedness / desired.cookedness);

        // CANNOT BE BOTHERED TO FIND HOW CLOSE A CUT IS TO A LINE
        int cutAccuracy = normalAccuracy((float) created.numSlices / desired.numSlices);

        // Calculate number of correct toppings
        int total = desired.toppings.Count;
        int correct = 0;
        for (int i = 0; i < desired.toppings.Count; i++)
        {
            if (created.toppings.Contains(desired.toppings[i]))
            {
                correct++;
            }
        }

        int toppingAccuracy = normalAccuracy((float) correct / desired.toppings.Count);

        return (cookednessAccuracy + cutAccuracy + toppingAccuracy) / 3;
    }

    public int normalAccuracy(float x)
    {
        float mu = 1f;
        float sigma = 0.35f;
        float normDistA = (float)(1 / (sigma * Mathf.Sqrt(2 * Mathf.PI)));
        float normDistEXP = (float)Mathf.Exp(-scoringHarshness * Mathf.Pow((x - mu) / sigma, 2));
        return Mathf.RoundToInt(Mathf.Min(normDistA * normDistEXP * 100, 100));
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Pizza")
        {
            GiveOrder();
            UnityEngine.Debug.Log(scoreResult(col.gameObject.GetComponent<Pizza>().recipe));
            Destroy(col.gameObject);
        }
    }
}
