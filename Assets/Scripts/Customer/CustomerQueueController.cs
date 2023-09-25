using System.Collections;
using System.Collections.Generic;
using Recipes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CustomerQueueController : MonoBehaviour
{
    public Queue<GameObject> queue;
    public int customerCount;
    
    // Start is called before the first frame update
    void Start()
    {
        CreateNewPanelForCustomer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddCustomer(GameObject customerPanel)
    {
        queue.Enqueue(customerPanel);
    }

    public void RemoveCustomer()
    {
        GameObject customerPanel = queue.Dequeue();
        Destroy(customerPanel);
    }

    public void CreateNewPanelForCustomer()
    {
        GameObject customerPanel = new GameObject("CustomerPanel" + ++customerCount);
        customerPanel.AddComponent<CanvasRenderer>();
        customerPanel.AddComponent<CustomerController>();
        customerPanel.AddComponent<RectTransform>();
        RectTransform rectTransform = customerPanel.GetComponent<RectTransform>();
        rectTransform.SetParent(this.gameObject.GetComponent<RectTransform>().transform);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 80, 256);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 20, 128);
        AddCustomer(customerPanel);
    }
}
