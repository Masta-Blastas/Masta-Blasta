using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private Player player;
    public float HP = 30;
    public float pointValue = 15;

    public ScoreSystem scoreSystem;

    public UnityEvent death;

    private void Start()
    {
        player = GameObject.Find("XR Rig").GetComponent<Player>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        DamageDealer damage = other.GetComponent<DamageDealer>();

        if (damage != null)
        {
            HP -= damage.damageAmount.Value;
        }

        if (HP <= 0)
        {
            scoreSystem.score.ApplyChange(pointValue);
            death.Invoke();
            Destroy(this.gameObject);
        }
    }
}
