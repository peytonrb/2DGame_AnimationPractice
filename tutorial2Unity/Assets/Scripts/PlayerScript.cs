using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // access to Text objects
using TMPro; // access to TextMeshProUGUI

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d; // reference to Rigidbody component in GameObject
    // Unity default speed is between -1 and 1 for positive and negative movement, multiplier is required
    public float speed; // Unity needs to access it
    public TextMeshProUGUI score;
    private int scoreValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>(); // find component that is Rigidbody2D and save reference to it
        score.text = scoreValue.ToString(); // will be 0 in very first frame due to initializer
    }

    // FixedUpdate is used with physics events
    // inherent to Unity MonoBehavior
    void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical"); // matched name in Unity settings

        // for any value associated with both movements, apply force to variable rd2d
        // this Rididbody is a physics material, therefore all physics are applied including gravity
        rd2d.AddForce(new Vector2(horizontalMovement * speed, verticalMovement * speed));
    }

    // what GameObject are we colliding with
    // when two things are colliding...
    private void OnCollisionEnter2D(Collision2D collision) {
        // after setting Coin tag to "Coin" in Unity
        if (collision.collider.tag == "Coin") {
            scoreValue += 1;
            score.text = scoreValue.ToString(); // every time "Coin" tag is collided with, score increases
            Destroy(collision.collider.gameObject);
        }
    }

    // build in function for when collision continues to happen (such as the floor)
    private void OnCollisionStay2D(Collision2D collision) {
        // can only jump off the floor bc it is the collision object with the player
        if (collision.collider.tag == "Ground") {
            if(Input.GetKey(KeyCode.W)) {
                // built-into Unity (ForceMode2D.Impulse is second parameter of Vector2)
                // Vector2 is used in 2D motion
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse); // sudden force (Impulse), gravity takes effect after
            }
        }
    }
}
