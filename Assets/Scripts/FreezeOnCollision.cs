using UnityEngine;

public class FreezeObject : MonoBehaviour
{
    public GameObject objectToFreeze;
    private bool isFrozen = false;
    private readonly float freezeDuration = 3f;
    private float freezeTimer = 0f;

    private void Update()
    {
        if (isFrozen)
        {
            freezeTimer += Time.deltaTime;
            if (freezeTimer >= freezeDuration)
            {
                Unfreeze();
            }
        }
    }

    public void Freeze()
    {
        isFrozen = true;

        freezeTimer = 0f;

        Rigidbody2D rbToFreeze = objectToFreeze.GetComponent<Rigidbody2D>();
        if (rbToFreeze != null)
        {
            rbToFreeze.simulated = false;
        }

        objectToFreeze.SetActive(false);
    }

    public void Unfreeze()
    {
        isFrozen = false;
        Rigidbody2D rbToFreeze = objectToFreeze.GetComponent<Rigidbody2D>();
        if (rbToFreeze != null)
        {
            rbToFreeze.simulated = true;
        }
        objectToFreeze.SetActive(true);
    }
}