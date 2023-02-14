using UnityEngine;

public class PaddlePowerUp : MonoBehaviour
{
    public float speed = 5f;

    public float height = 0.5f;

    private Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        pos = new Vector3(0f, 0.21f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        float newZ = Mathf.Cos(Time.time * speed) * height + pos.z;
        transform.position = new Vector3(0f,transform.position.y, newZ);
    }
}
