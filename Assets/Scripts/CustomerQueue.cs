using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerQueue
{
    public Queue<Customer> queue;
    public List<GameObject> queuePositions;
    public GameObject queueEnd;

    public float overflowDistance = 1.5f;

    public CustomerQueue()
    {
        queue = new Queue<Customer>();
    }

    public void Clear()
    {
        queue.Clear();
        
    }

    public int Count()
    {
        return queue.Count;
    }

    public Customer front()
    {
        Customer customer = null;
        queue.TryPeek(out customer);

        return customer;
    }

    public void Enqueue(Customer customer)
    {
        queue.Enqueue(customer);
        customer.queuePosition = queue.Count - 1;

        if (queue.Count <= queuePositions.Count)
        {

            // Move customer to the end of the queue
            List<Vector3> positions = new List<Vector3>();
            positions.Add(queueEnd.transform.position);

            // Move the customer to the next position of the queue
            Vector3 queuePosition = queuePositions[customer.queuePosition].transform.position;
            positions.Add(queuePosition);

            customer.StartCoroutine(customer.MoveToQueue(positions, GameManager.Instance.speed));
        } else
        {
            // Move customer to the end of the queue
            List<Vector3> positions = new List<Vector3>();
            Vector3 end = queueEnd.transform.position - new Vector3(0, 0, (customer.queuePosition - queue.Count) * overflowDistance);
            positions.Add(end);

            customer.StartCoroutine(customer.MoveToQueue(positions, GameManager.Instance.speed));
        }

        
    }

    public Customer Dequeue()
    {
        Customer popped = queue.Dequeue();

        foreach (Customer c in queue)
        {
            c.queuePosition--;
        }

        return popped;
    }

    public bool Contains(Customer customer)
    {
        return queue.Contains(customer);
    }

    public void UpdatePositions()
    {
        int count = 0;
        foreach (Customer c in queue)
        {
            List<Vector3> positions = new List<Vector3>();
            if (c.moving)
                continue;
            if (c.queuePosition < queuePositions.Count)
            {
                // Move the customer to the next position of the queue
                Vector3 queuePosition = queuePositions[c.queuePosition].transform.position;
                positions.Add(queuePosition);
            }
            else
            {
                // Move them a little closer ig
                Vector3 end = Vector3.MoveTowards(c.go.transform.position, queueEnd.transform.position, 4);
                positions.Add(end);
            }

            c.StartCoroutine(c.MoveToQueue(positions, GameManager.Instance.speed));

        }
    }

}
