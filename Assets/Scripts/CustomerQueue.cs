using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerQueue
{
    public Queue<Customer> queue;
    public List<GameObject> queuePositions;
    public GameObject queueEnd;

    public int speed = 2;
    public float overflowDistance = 1.5f;

    public CustomerQueue()
    {
        queue = new Queue<Customer>();
    }

    public void Enqueue(Customer customer)
    {
        queue.Enqueue(customer);

        if (queue.Count <= queuePositions.Count)
        {
            customer.queuePosition = queue.Count - 1;

            // Move customer to the end of the queue
            List<Vector3> positions = new List<Vector3>();
            positions.Add(queueEnd.transform.position);

            // Move the customer to the next position of the queue
            Vector3 queuePosition = queuePositions[customer.queuePosition].transform.position;
            positions.Add(queuePosition);

            customer.StartCoroutine(customer.MoveToQueue(positions, speed));
        } else
        {
            customer.queuePosition = queue.Count - 1;

            // Move customer to the end of the queue
            List<Vector3> positions = new List<Vector3>();
            Vector3 end = queueEnd.transform.position - new Vector3(0, 0, (customer.queuePosition - queue.Count) * overflowDistance);
            positions.Add(end);

            customer.StartCoroutine(customer.MoveToQueue(positions, speed));
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

    public void UpdatePositions()
    {
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

            c.StartCoroutine(c.MoveToQueue(positions, speed));

        }
    }

}
