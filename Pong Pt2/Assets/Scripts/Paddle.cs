using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public bool leftPaddle;
    public Ball ball;
    public float speed = 10f;

    public float speedIncrement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (leftPaddle)
        {
            transform.Translate(0f, 0f, Input.GetAxis("Vertical") * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(0f, 0f, Input.GetAxis("Vertical2") * speed * Time.deltaTime);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        
        ball = collision.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            Vector3 normal = collision.GetContact(0).normal;
            ball.AddForce(-normal * speedIncrement);
            ball.gameObject.GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            
        }

        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

}
