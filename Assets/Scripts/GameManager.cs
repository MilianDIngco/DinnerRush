using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject customerPrefab;
    public GameObject customerStartPosition;

    public List<GameObject> orderingPositions = new List<GameObject>();
    public List<GameObject> waitingPositions = new List<GameObject>();
    public List<GameObject> seatPositions = new List<GameObject>();

    public List<GameObject> tables = new List<GameObject>();
    public Material messyMaterial;

    public float waveDurationIncrement = 5;

    public List<GameObject> customers;
    public int score = 0;

    public float waveStartTime;
    public float waveDuration;
    public List<float> entranceTimes;
    public int currentCustomer = 0;

    bool waveEnd = false;

    public CustomerQueue orderingQueue;
    public CustomerQueue waitingQueue;
    public CustomerQueue seatQueue;

    public SpawnObject plateSpawner;
    public SpawnObject crustSpawner;
    public SpawnObject sauceSpawner;
    public SpawnObject tomatoSpawner;
    public SpawnObject pepperoniSpawner;
    public SpawnObject cheeseSpawner;

    public StressLevel stressMeter;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        customers = new List<GameObject>();

        orderingQueue = new CustomerQueue();
        waitingQueue = new CustomerQueue();
        seatQueue = new CustomerQueue();

        orderingQueue.queuePositions = orderingPositions;
        orderingQueue.queueEnd = orderingPositions[orderingPositions.Count - 1];
        waitingQueue.queuePositions = waitingPositions;
        waitingQueue.queueEnd = waitingPositions[waitingPositions.Count - 1];
        seatQueue.queuePositions = seatPositions;
        seatQueue.queueEnd = seatPositions[seatPositions.Count - 1];

        foreach (GameObject go in orderingPositions)
        {
            go.SetActive(false);
        }

        foreach (GameObject go in waitingPositions)
        {
            go.SetActive(false);
        }

        foreach (GameObject go in seatPositions)
        {
            go.SetActive(false);
        }

        GenerateWave(5);
    }

    // Update is called once per frame
    void Update()
    {
        //if (currentCustomer == customers.Count)
        //{
        //Debug.Log("Game Over");
        //}

        if (!waveEnd && currentCustomer < customers.Count && Time.time > entranceTimes[currentCustomer])
        {
            customers[currentCustomer].transform.position = customerStartPosition.transform.position;
            orderingQueue.Enqueue(customers[currentCustomer].GetComponent<Customer>());
            Customer customer = customers[currentCustomer].GetComponent<Customer>();
            customer.ordering = true;
            currentCustomer++;
        }

        orderingQueue.UpdatePositions();
        waitingQueue.UpdatePositions();
        seatQueue.UpdateTablePositions();
    }

    public bool PopCustomer(bool ordering)
    {
        Customer customer;

        if (ordering)
        {
            customer = orderingQueue.Dequeue();
            waitingQueue.Enqueue(customer);
            return true;
        }
        else
        {
            bool seatAvailable = false;
            if(seatQueue.tableSpotsOccupied == null)
                seatQueue.tableSpotsOccupied = new bool[seatQueue.queuePositions.Count];
            for (int i = 0; i < seatQueue.queuePositions.Count; i++)
            {
                if (!seatQueue.tableSpotsOccupied[i])
                {
                    seatAvailable = true;
                }
            }
            if (seatAvailable)
            {
                customer = waitingQueue.Dequeue();
                seatQueue.EnqueueTable(customer);
                Debug.Log("thanks....");
                customer.cui.ShowCanvas(false);
                StartCoroutine(Leave());
                return true;
            }
            Debug.Log("nowhere to sit....");
            return false;
        }
    }

    IEnumerator Leave()
    {
        yield return new WaitForSeconds(60);
        Customer customer;
        customer = seatQueue.DequeueTable();
        customer.Leave();
    }

    public void BusTable(int tableID)
    {
        seatQueue.FreeTablePosition(tableID);
    }

    void GenerateWave(int waveNumber)
    {
        foreach (GameObject go in customers)
        {
            Destroy(go);
        }

        stressMeter.SetSlider(10);

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

    public void SubmitPizza(GameObject pizza)
    {
        Customer customer = waitingQueue.front();
        if (customer != null)
        {
            Customer.Recipe submitted = pizza.GetComponent<Pizza>().recipe;
            int customerScore = customer.scoreResult(submitted);
            score += customerScore;

            customer.GiveOrder();

            stressMeter.SetSlider(-customerScore / 10);
        }

    }

    public void spawnObject(Ingredient.IngredientType ingredient)
    {
        switch (ingredient)
        {
            case Ingredient.IngredientType.Plate:
                plateSpawner.Spawn();
                break;
            case Ingredient.IngredientType.PizzaCrust:
                crustSpawner.Spawn();
                break;
            case Ingredient.IngredientType.TomatoSauce:
                sauceSpawner.Spawn();
                break;
            case Ingredient.IngredientType.Tomato:
                tomatoSpawner.Spawn();
                break;
            case Ingredient.IngredientType.Pepperoni:
                pepperoniSpawner.Spawn();
                break;
            case Ingredient.IngredientType.MozzerellaCheese:
                cheeseSpawner.Spawn();
                break;
        }
    }

    public void MessyTable(int tableID)
    {
        tables[tableID].GetComponent<Renderer>().material = messyMaterial;
    }
}
