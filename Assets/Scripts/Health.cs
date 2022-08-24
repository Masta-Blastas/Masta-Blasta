using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public bool resetHP;

    public FloatVariable HP;
    public FloatVariable startingHP;

    public UnityEvent playerDeath;

    void Start()
    {
        if (resetHP)
        {
            HP.SetValue(startingHP);
        }
    }

    void Update()
    {
        if (HP.Value <= 0)
        {
            playerDeath.Invoke();
        } 
    }
}
