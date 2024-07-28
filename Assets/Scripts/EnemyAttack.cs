using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float timer = 5f;
    private float bulletTime;
    public Transform spawnPoint;
    public float power;
    public GameObject enemyBullet;
    private FieldofView _fieldofView;
    private DragAndShoot _dragAndShoot;
    private GameObject _gameObject;
    
    void Start()
    {
        bulletTime = timer;
        _fieldofView = GetComponent<FieldofView>();
        _gameObject = GameObject.Find("Character");
        _dragAndShoot = _gameObject.GetComponent<DragAndShoot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_fieldofView.canSeePlayer && _dragAndShoot)
        {
            
            ShootAtPlayer();
        }
        if (_fieldofView.canSeePlayer && !_dragAndShoot.isGrounded)
        {
            Debug.Log("Play Alert Animation Here!");
            Debug.Log("Hello");
        }
    }

    void ShootAtPlayer()
    {
        bulletTime -= Time.deltaTime;
        if (bulletTime > 0) return;

        bulletTime = timer;
        GameObject bulletInstantiate = Instantiate(enemyBullet, spawnPoint.position, spawnPoint.rotation);
        Rigidbody bulletRig = bulletInstantiate.GetComponent<Rigidbody>();
        bulletRig.AddForce((((_fieldofView.targetCenter - _fieldofView.objectCenter).normalized))*power, ForceMode.Impulse);
        // bulletRig.AddForce(new Vector3(-0.5f, 0.4f, 0) * power, ForceMode.Impulse);
        Destroy(bulletInstantiate, 1.4f); 
    }
}