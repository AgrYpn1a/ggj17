using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour {
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private float speed;

	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.UpArrow))
        {
            player.transform.position += new Vector3(0, speed, 0);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            player.transform.position += new Vector3(0, -speed, 0);
        }

        player.transform.position += new Vector3(speed, 0, 0);
    }

}
