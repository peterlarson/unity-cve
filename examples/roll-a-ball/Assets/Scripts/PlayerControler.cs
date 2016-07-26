using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class PlayerControler : MonoBehaviour {

    public float speed;
    public Text countText;
    public Text winText;

    private Rigidbody rb;
    private int count;


    void Start ()
    {
        count = 0;
        rb = GetComponent<Rigidbody>();
        SetCountText();
        winText.text = "";
    }

	void FixedUpdate () {
        //float moveHorizontal = Input.GetAxis ("Horizontal");
        //float moveVertical = Input.GetAxis ("Vertical");

        //Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        //rb.AddForce(movement * speed);
	}

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 8)
            winText.text = "You Win!"; 
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }
}
