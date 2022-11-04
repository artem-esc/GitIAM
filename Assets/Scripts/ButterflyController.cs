using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class ButterflyController : MonoBehaviour
{

    public SpriteRenderer _renderer;

    public float turn_angle_y = 10.5f;
    public float turn_speed_y = 2.0f;
    private double prev_pos_y;

    private double prev_pos_x;

    public float speed = 10.0f;
    private Vector2 target;
    private Vector2 position;
    private Camera cam;
    private Vector3 w_pos;
    private bool flipped = false;
    private float speed_inc_value = 3.0f;


    void Start()
    {

 
        _renderer = GetComponent<SpriteRenderer>();
       
        if (_renderer == null)
        {
            Debug.LogError("Player Sprite is missing a renderer");
        }

        target = new Vector2(0.0f, 0.0f);
        position = gameObject.transform.position;

        cam = Camera.main;
    }

    void FixedUpdate()
    {
        float step = speed * Time.deltaTime;

        // move sprite towards the target location
        transform.position = Vector2.MoveTowards(transform.position, target, step);
        Vector3 mousePos = Input.mousePosition;
        Vector3 w_pos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0.0f));
        {
            if (mousePos.y > prev_pos_y && prev_pos_y > -80)
            {
                transform.Rotate(new Vector3(0, 0, turn_angle_y) * turn_speed_y * Time.deltaTime);
            }
            else if (mousePos.y < prev_pos_y && prev_pos_y > -80)
            {
                transform.Rotate(new Vector3(0, 0, -turn_angle_y) * turn_speed_y * Time.deltaTime);
            }

            if (w_pos.x < gameObject.transform.position.x)
            {
                _renderer.flipX = true;
                flipped = true;
            }
            else if (w_pos.x > gameObject.transform.position.x)
            {
                _renderer.flipX = false;
                flipped = false;
            }
            prev_pos_y = mousePos.y;
            prev_pos_x = mousePos.x;
        }

        
        
    }



    void OnGUI()
    {
        Event currentEvent = Event.current;
        Vector2 mousePos = new Vector2();
        Vector2 point = new Vector2();

        // compute where the mouse is in world space
        mousePos.x = currentEvent.mousePosition.x;
        mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y;
        point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0.0f));
        target = point;
        
    }




    void OnTriggerEnter2D(Collider2D flower)
    {
        if(flower.tag == "speed")
        {
            speed += speed_inc_value;
            StartCoroutine(CancelSpeed());
        }
        
        else if(flower.tag == "death")
        {
            SceneManager.LoadScene("DeathScene");
        }

        Destroy(flower.gameObject);
       
    }


    IEnumerator CancelSpeed()
    {
        yield return new WaitForSeconds(5);
        speed -= speed_inc_value;
    }
}
