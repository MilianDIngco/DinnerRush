using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CustomerUI : MonoBehaviour
{
    public GameObject canvas;
    public Image plateImg;
    public Image doughImg;
    public Image sauceImg;
    public Image topping1Img;
    public Image topping2Img;
    public TextMeshProUGUI cookednessTxt;
    public TextMeshProUGUI nSlicesTxt;

    public List<Sprite> plates;
    public List<Sprite> doughs;
    public List<Sprite> sauces;
    public List<Sprite> toppings;

    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(false);
    }

    public void ShowCanvas(bool value)
    {
        canvas.SetActive(value);
    }

    public void SetRecipe(Customer.Recipe recipe) 
    {
        int plate = 0, dough = 0, sauce = 0;
        List<int> toppingValues = new List<int>();
        foreach (Ingredient.IngredientType i in recipe.toppings)
        {
            int value = (int)i;
            if (value < Ingredient.firstCrust)
            {
                plate = value - Ingredient.firstPlate; // Should be 1 - 1 = 0 to index into plate array
            } else if (value < Ingredient.firstSauce)
            {
                dough = value - Ingredient.firstCrust; 
            } else if (value < Ingredient.firstTopping)
            {
                sauce = value - Ingredient.firstSauce;
            } else
            {
                toppingValues.Add(value - Ingredient.firstTopping);
            }
        }

        // Set plate img
        plateImg.sprite = plates[plate];

        // Set dough img
        doughImg.sprite = doughs[dough];

        // Set sauce img
        sauceImg.sprite = sauces[sauce];

        // Set toppings
        topping1Img.sprite = toppings[toppingValues[0]];
        topping2Img.sprite = toppings[toppingValues[1]];

        // Set cookedness
        cookednessTxt.text = recipe.cookedness.ToString();

        // Set # slices
        nSlicesTxt.text = recipe.numSlices.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
