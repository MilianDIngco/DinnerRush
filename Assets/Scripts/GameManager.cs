using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject customerPrefab;
    public GameObject customerStartPosition;
    public Queue<int> customerQueue;
    public Queue<int> customerWait;

    public float waveDurationIncrement = 5;

    public List<GameObject> customers;
    public int score = 0;

    public float waveStartTime;
    public float waveDuration;
    public List<float> entranceTimes;
    public int currentCustomer = 0;

    bool waveEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        customers = new List<GameObject>();
        customerQueue = new Queue<int>();
        customerWait = new Queue<int>();

        GenerateWave(5);
    }

    // Update is called once per frame
    void Update()
    {
        //if (currentCustomer == customers.Count)
        //{
            //Debug.Log("Game Over");
        //}

        if (!waveEnd && Time.time > entranceTimes[currentCustomer])
        {
            customers[currentCustomer].transform.position = customerStartPosition.transform.position;
            customerQueue.Enqueue(currentCustomer);
            Customer customer = customers[currentCustomer].GetComponent<Customer>();

            customer.PrintRecipe();
            currentCustomer++;
        }
    }

    void GenerateWave(int waveNumber)
    {
        customers.Clear();
        for (int i = 0; i < waveNumber; i++)
        {
            GameObject customerGO = Instantiate(customerPrefab);
            customerGO.transform.position = new Vector3(0, -100, 0);
            customerGO.transform.parent = transform;
            customers.Add(customerGO);
        }

        entranceTimes = new List<float>(customers.Count);
        waveStartTime = Time.time;
        waveDuration = waveNumber * waveDurationIncrement;

        for (int i = 0; i < customers.Count; i++)
        {
            entranceTimes.Add((waveStartTime + (waveDuration * (float)i / customers.Count)));
        }

    }

    bool IsWaveOver()
    {
        return customers.Count == 0;
    }

    void SubmitPizza(int customerIndex, Customer.Recipe recipe)
    {
        Customer customer = customers[customerIndex].GetComponent<Customer>();
        int customerScore = customer.scoreResult(recipe);

        score += customerScore;
    }
}
