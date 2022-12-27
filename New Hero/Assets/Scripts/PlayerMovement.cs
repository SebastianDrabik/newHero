using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float vertical;
    private float horizontal;
    public int speed=2;

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
        
        vertical *= Time.deltaTime;
        horizontal *= Time.deltaTime;

        //transform.Translate(horizontal * speed, vertical * speed, 0);
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(horizontal * speed, vertical * speed);
    }
}
