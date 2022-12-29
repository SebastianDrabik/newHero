using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float vertical;
    private float horizontal;
    public int speed=2;

    public Animator animator;
    public SpriteRenderer spriteRenderer;

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
        }
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
        
        if(vertical!=0 || horizontal != 0)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }

        if (animator.GetFloat("Horizontal") == 0)
        {
            animator.SetFloat("Vertical", vertical);
        }

        if (animator.GetFloat("Vertical") == 0)
        {
            animator.SetFloat("Horizontal", Mathf.Abs(horizontal));
        }


        if (horizontal < 0)
        {
            spriteRenderer.flipX = true;
        }

        if (horizontal > 0 || (horizontal==0 && vertical!=0))
        {
            spriteRenderer.flipX = false;
        }

        //vertical *= Time.deltaTime;
        //horizontal *= Time.deltaTime;

        //transform.Translate(horizontal * speed, vertical * speed, 0);
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(horizontal * speed, vertical * speed);
    }
}
