using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject customerPrefab;
    public GameObject customerStartPosition;
    public Queue<int> customerQueue;
    public Queue<int> customerWait;

    public List<GameObject> customers;
    public int score = 0;

    public float waveStartTime;
    public float waveDuration;
    public List<float> entranceTimes;
    public int currentCustomer = 0;


    // Start is called before the first frame update
    void Start()
    {
        customers = new List<GameObject>();

        GenerateWave(5);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > entranceTimes[currentCustomer])
        {
            customers[currentCustomer].transform.position = customerStartPosition.transform.position;
            customerQueue.Enqueue(currentCustomer);
            Customer customer = customers[currentCustomer].GetComponent<Customer>();
            
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
        waveDuration = 3000;

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
