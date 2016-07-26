using UnityEngine;
using System.Collections;

public class BoardController : MonoBehaviour {

    public int speed;
	
	// Update is called once per frame
	void FixedUpdate () {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveVertical, 0.0f, -moveHorizontal);

        //rb.AddForce(movement * speed);
        transform.Rotate(movement * Time.deltaTime * speed);
    }

    
}
