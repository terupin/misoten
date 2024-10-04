using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Controller : MonoBehaviour{

    public float moveSpeed = 5.0f;
    private Rigidbody rb;
    private Transform cameraTransform;
    private Transform target;

    void Start(){

        // Rigidbody�R���|�[�l���g���擾
        rb = GetComponent<Rigidbody>();

        // ���C���J������Transform���擾
        cameraTransform = Camera.main.transform;

        // "Enemy"�^�O���t�����I�u�W�F�N�g��T����target�ɃZ�b�g
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy != null){

            target = enemy.transform;
        }
        else{

            Debug.Log("Enemy�^�O���t�����I�u�W�F�N�g��������܂���ł����B");
        }
    }

    void Update(){

        // �ړ����͂��擾
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (moveX != 0 || moveZ != 0){

            // �J�����̌�������ɂ����ړ��������v�Z
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            // Y���̐������[���ɂ��āA�����ȕ����ɐ���
            forward.y = 0;
            right.y = 0;

            // ���͂Ɋ�Â��Ĉړ��������v�Z
            Vector3 movementInput = (forward * moveZ + right * moveX).normalized;

            // Rigidbody���g���Ĉړ�
            MoveCharacter(movementInput);
        }

        // �G�����鏈��
        if (target != null){

            CameraUpdate();
        }
    }

    // Rigidbody���g���Ĉړ�
    private void MoveCharacter(Vector3 movementInput){

        Vector3 move = movementInput * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + move);
    }

    // �G�����鏈��
    void CameraUpdate(){

        // target�I�u�W�F�N�g�𒍎�����
        this.transform.LookAt(target);
    }
}