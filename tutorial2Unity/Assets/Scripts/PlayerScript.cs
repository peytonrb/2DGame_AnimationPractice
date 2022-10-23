using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // access to Text objects
using TMPro; // access to TextMeshProUGUI
using UnityEngine.Tilemaps;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d; // reference to Rigidbody component in GameObject
    // Unity default speed is between -1 and 1 for positive and negative movement, multiplier is required
    public float speed; // Unity needs to access it
    public TextMeshProUGUI score;
    public TextMeshProUGUI lives;
    public GameObject winText;
    public GameObject loseText;
    public AudioSource background;
    public AudioClip sound;
    public AudioClip winSound;
    Animator anim;
    private int scoreValue = 0;
    private int livesValue = 3;
    private bool facingRight = true;

    // GroundCheck variables
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>(); // find component that is Rigidbody2D and save reference to it
        score.text = scoreValue.ToString(); // will be 0 in very first frame due to initializer
        lives.text = "Lives: " + livesValue.ToString();
        winText.SetActive(false);
        loseText.SetActive(false);
        background.clip = sound;
        background.Play();
        background.loop = true;
        anim = GetComponent<Animator>();
    }

    void Update()
    {   
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) {
            anim.SetInteger("State", 1);
        }

        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A)) {
            anim.SetInteger("State", 0);
        }

        if (isOnGround == false && Input.GetAxis("Vertical") > 0.1)
        {
            anim.SetInteger("State", 2);
        } else if (isOnGround == false && Input.GetAxis("Vertical") > 0.5) {
            anim.SetInteger("State", 3);
        }

        // anim.SetFloat("", Mathf.Abs(Input.GetAxis("Horizontal")));
        // anim.SetFloat("State", Input.GetAxis("Vertical"));

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // FixedUpdate is used with physics events
    // inherent to Unity MonoBehavior
    void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical"); // matched name in Unity settings
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        // for any value associated with both movements, apply force to variable rd2d
        // this Rididbody is a physics material, therefore all physics are applied including gravity
        rd2d.AddForce(new Vector2(horizontalMovement * speed, verticalMovement * speed));

        if (facingRight == false && horizontalMovement > 0) {
            Flip();
        } else if (facingRight == true && horizontalMovement < 0) {
            Flip();
        }
    }

    // what GameObject are we colliding with
    // when two things are colliding...
    private void OnCollisionEnter2D(Collision2D collision) {

        // after setting Coin tag to "Coin" in Unity
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString(); // every time "Coin" tag is collided with, score increases
            Destroy(collision.collider.gameObject);

            if (scoreValue == 4 && livesValue != 0)
            {
                transform.position = new Vector3(53.0f, 0.5f, 0.0f);
                livesValue = 3;
                lives.text = "Lives: " + livesValue.ToString();

            }
            if (scoreValue == 8 && livesValue != 0)
            {
                winText.SetActive(true);
                background.Stop();
                background.loop = false;
                background.clip = winSound;
                background.Play();
                background.loop = true;
                rd2d.constraints = RigidbodyConstraints2D.FreezeAll;
            }

        }
        else if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = "Lives: " + livesValue.ToString();
            Destroy(collision.collider.gameObject);

            if (livesValue == 0)
            {
                loseText.SetActive(true);
                Destroy(gameObject);
            }
        }
    }

    // // build in function for when collision continues to happen (such as the floor)
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }

    private void Flip() {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}
