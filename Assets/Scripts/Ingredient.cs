using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public enum IngredientType
    {
        // Plate : 0
        Plate,

        // Crust : 1
        PizzaCrust,

        // Sauce : 2
        TomatoSauce,

        // Toppings : 3 - 5
        Tomato,
        Pepperoni,
        MozzerellaCheese
    }

    public static int firstPlate = 0;
    public static int firstCrust = 1;
    public static int firstSauce = 2;
    public static int firstTopping = 3;
    public static int last = 5;

    public IngredientType ingredientType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
