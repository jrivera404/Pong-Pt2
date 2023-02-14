using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 5f;
    private int leftScore;
    private int rightScore;
    private Transform lastPaddleTouched;
    public AudioClip bounce;
    private AudioSource audioSource;
    public TextMeshProUGUI rightScoreText;
    public TextMeshProUGUI leftScoreText;
    public Transform camera;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = 0.4f;
        
        float sx = Random.Range(0, 2) == 0 ? -1 : 1;
        float sz = Random.Range(0, 2) == 0 ? Random.Range(-1.0f,-0.5f) : Random.Range(0.5f,1.0f);

        rb.velocity = new Vector3(speed * sx, 0f, speed * sz);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void LeftScores()
    {
        leftScore++;
        leftScoreText.text = $"{leftScore}";
        switch (leftScore)
        {
            case > 5 and < 10:
                leftScoreText.color = Color.blue;
                break;
            case 10:
                leftScoreText.color = Color.red;
                leftScoreText.fontSize += 10;
                break;
        }
        Debug.Log("Left Scored! Game Score: " + leftScore + " - " + rightScore);
        if (leftScore > 10)
        {
            GameOver();
            return;
        }
        audioSource.pitch = 0.4f;
        rb.position = new Vector3(7, 0, 0);
        float z = Random.Range(0, 2) == 0 ? Random.Range(-1.0f,-0.5f) : Random.Range(0.5f,1.0f);
        rb.velocity = new Vector3(-1 * speed, 0f,speed * z);
    }
    
    private void RightScores()
    {
        rightScore++;
        rightScoreText.text = $"{rightScore}";
        switch (rightScore)
        {
            case > 5 and < 10:
                rightScoreText.color = Color.blue;
                break;
            case 10:
                rightScoreText.color = Color.red;
                rightScoreText.fontSize += 10;
                break;
        }
        Debug.Log("Right Scored! Game Score: " + leftScore + " - " + rightScore);
        if (rightScore > 10)
        {
            GameOver();
            return;
        }
        audioSource.pitch = 0.4f;
        rb.position = new Vector3(-7, 0, 0);
        float z = Random.Range(0, 2) == 0 ? Random.Range(-1.0f,-0.5f) : Random.Range(0.5f,1.0f);;
        rb.velocity = new Vector3(speed, 0f,speed * z);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Left Goal"))
        {
            StartCoroutine(Shake(.2f, new Vector3(.5f, .5f, .5f), .8f));
            RightScores();
        }
        else if (other.name.Equals("Right Goal"))
        {
            StartCoroutine(Shake(.2f, new Vector3(.5f, .5f, .5f), .8f));
            LeftScores();
        }
        else if (other.name.Equals("Ball Speed Power up"))
        {
            rb.AddForce(rb.velocity * 150);
            Debug.Log($"Hit a powerup {other.name}");
        }
        else if (other.name.Equals("Paddle Power up"))
        {
            StartCoroutine(ChangePaddleSize());
            Debug.Log($"Hit a powerup {other.name}");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"collided with {collision.gameObject.name}");
        if (collision.gameObject.name.Equals("Left Paddle"))
        {
            // Debug.Log($"Hit Left paddle");
            lastPaddleTouched = collision.transform;
        }
        else if (collision.gameObject.name.Equals("Right Paddle"))
        {
            // Debug.Log("hit right paddle");
            lastPaddleTouched = collision.transform;
        }
    }

    private void GameOver()
    {
        if (leftScore > rightScore)
        {
            Debug.Log("Game Over! Left Wins! 0 - 0");
        }
        else
        {
            Debug.Log("Game Over! Right Wins! 0 - 0");
        }
        
        rb.position = Vector3.zero;
        rb.velocity = Vector3.zero;
        leftScore = 0;
        rightScore = 0;
    }

    public void SpeedUp()
    {
        speed += 25;
    }

    public void AddForce(Vector3 force)
    {
        rb.AddForce(force);
        audioSource.clip = bounce;
        audioSource.pitch += 0.2f;
        audioSource.Play();
    }
    
    IEnumerator ChangePaddleSize()
    {
        lastPaddleTouched.localScale = new Vector3(0.5f, 1f, 5);
        yield return new WaitForSeconds(10);
        lastPaddleTouched.localScale = new Vector3(0.5f, 1f, 3);
    }

    IEnumerator Shake(float duration, Vector3 magnitude, float wavelength)
    {
        Vector3 originalPos = camera.localPosition;
        float currentX = 0.0f;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            Vector3 shakeAmount = new Vector3(
                (Mathf.PerlinNoise(currentX,2.2f) * 2) - 1f,
                (Mathf.PerlinNoise(currentX,5.5f) * 2) - 1f,
                (Mathf.PerlinNoise(currentX,8.8f) * 2) - 1f
                );

            camera.localPosition = Vector3.Scale(magnitude, shakeAmount) + originalPos;
            currentX += wavelength;
            elapsed += Time.deltaTime;
            
            yield return null;
        }

        camera.localPosition = originalPos;
    }
}
