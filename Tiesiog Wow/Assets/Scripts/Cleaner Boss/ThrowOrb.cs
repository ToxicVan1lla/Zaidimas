using UnityEngine;

public class ThrowOrb : MonoBehaviour
{
    public string objectName;
    public Vector2 landPosition;
    private bool fall = true;
    [SerializeField] GameObject Tornado;
    [SerializeField] GameObject Rat;
    private CleanerControl cleaner;
    private GameObject GO;

    private void Start()
    {
        cleaner = GameObject.Find("The Cleaner").GetComponent<CleanerControl>();
    }
    private void Update()
    {
        if (transform.position.x == landPosition.x && transform.position.y == landPosition.y)
            fall = false;
        if (fall)
        {
            transform.position = Vector2.MoveTowards(transform.position, landPosition, 7 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            fall = false;
            if(objectName == "Tornado")
                GO = Instantiate(Tornado, transform.position, transform.rotation);
            if (objectName == "Rat")
                GO = Instantiate(Rat, transform.position, transform.rotation);
            cleaner.GoHolder.Add(GO);

            Destroy(gameObject);
        }
    }

}
