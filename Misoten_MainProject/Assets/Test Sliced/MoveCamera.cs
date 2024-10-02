using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField, Header("�X�s�[�h")]
    float Player_Speed = 0.05f;

    Vector3 Player_pos;
    Vector3 Player_rot;

    // Start is called before the first frame update
    void Start()
    {
        Player_pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.W))
            Player_pos += transform.forward * Player_Speed;
        if (Input.GetKey(KeyCode.A))
            Player_pos -= transform.right * Player_Speed;
        if (Input.GetKey(KeyCode.S))
            Player_pos -= transform.forward * Player_Speed;
        if (Input.GetKey(KeyCode.D))
            Player_pos += transform.right * Player_Speed;
        if (Input.GetKey(KeyCode.UpArrow))
            Player_pos += transform.up * Player_Speed;
        if (Input.GetKey(KeyCode.DownArrow))
            Player_pos -= transform.up * Player_Speed;


        if (Input.GetKey(KeyCode.LeftArrow))
            Player_rot.y -= Player_Speed;
        if (Input.GetKey(KeyCode.RightArrow))
            Player_rot.y += Player_Speed;


        transform.rotation = Quaternion.Euler(Player_rot);
        transform.position = Player_pos;
    }
}
