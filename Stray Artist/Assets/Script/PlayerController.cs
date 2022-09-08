using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Tsting
    [SerializeField] float accel;

    //Physic var
    Rigidbody2D rb;
    public Transform _t;

    public float PlayerScaleX;

    //Movement Var
    [SerializeField] float speed = 8f;
    [SerializeField] float RunMutiplier = 1.5f;

    [SerializeField] float Jumpforce = 15f;

    [SerializeField] float MaxAccel = 3f;
    
    public bool isGrounded;
    
    // Start is called before the first frame update
    void Start() {
        isGrounded = true;
        rb = gameObject.GetComponent<Rigidbody2D>();
        _t = transform;

        //Get player's localscale in x axis for later flipping
        PlayerScaleX = _t.localScale.x;
    }
    // Update is called once per frame
    void Update()
    {
        //Check input every frame
        move();
        Jump();
        acceleration();
    }

    void move()
    {
        //Get Input
        float x =  Input.GetAxisRaw("Horizontal");

        //Acceleration
        //if accel is less than max accel and greater than max accel opp direction
        if (accel <= MaxAccel && accel >= -MaxAccel)
        accel += 0.2f * x * Time.deltaTime;

        //is Running
        float run = 1f;
        if(Input.GetKey(KeyCode.LeftShift))
        {
        run = RunMutiplier;
        }

        //horizontal speed calculation
        float hs = x * speed * run * Time.deltaTime * 300;
        Debug.Log(hs);
        //Move Player
        rb.velocity = new Vector2(hs,rb.velocity.y);

        //flip player to moving direction via
        if (x < 0) { _t.localScale = new Vector3(PlayerScaleX, _t.localScale.y,1); }
        else if (x > 0){ _t.localScale = new Vector3(-PlayerScaleX, _t.localScale.y,1); }
    }

    void Jump()
    {
        //Check if player is grounded when jump
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded == true )
        {
            //Add force to the y axis to do a jump action
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, Jumpforce), ForceMode2D.Impulse);
        }
    }

    void acceleration()
    {
        //Get Input
        float x =  Input.GetAxisRaw("Horizontal");

        float acs = accel * Time.deltaTime * 300;
        //if player stop moving
        if(x == 0)
        {
        accel -= (accel * 0.8f * Time.deltaTime);
        }
        Debug.Log(x);

        rb.velocity = new Vector2((rb.velocity.x + acs),rb.velocity.y);
    }

    
    void OnCollisionEnter2D(Collision2D other) {
        //On collision with the ground => enable jump
        if (other.gameObject.tag == "Ground" && isGrounded == false)
         {
             isGrounded = true;
         }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        //exiting the collision with the ground => disable jump
        if (other.gameObject.tag == "Ground")
         {
             isGrounded = false;
         }
    }
}
