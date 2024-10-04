using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Controller : MonoBehaviour{

    public float moveSpeed = 5.0f;
    private Rigidbody rb;
    private Transform cameraTransform;
    private Transform target;

    void Start(){

        // Rigidbodyコンポーネントを取得
        rb = GetComponent<Rigidbody>();

        // メインカメラのTransformを取得
        cameraTransform = Camera.main.transform;

        // "Enemy"タグが付いたオブジェクトを探してtargetにセット
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy != null){

            target = enemy.transform;
        }
        else{

            Debug.Log("Enemyタグが付いたオブジェクトが見つかりませんでした。");
        }
    }

    void Update(){

        // 移動入力を取得
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (moveX != 0 || moveZ != 0){

            // カメラの向きを基にした移動方向を計算
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            // Y軸の成分をゼロにして、水平な方向に制限
            forward.y = 0;
            right.y = 0;

            // 入力に基づいて移動方向を計算
            Vector3 movementInput = (forward * moveZ + right * moveX).normalized;

            // Rigidbodyを使って移動
            MoveCharacter(movementInput);
        }

        // 敵を見る処理
        if (target != null){

            CameraUpdate();
        }
    }

    // Rigidbodyを使って移動
    private void MoveCharacter(Vector3 movementInput){

        Vector3 move = movementInput * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + move);
    }

    // 敵を見る処理
    void CameraUpdate(){

        // targetオブジェクトを注視する
        this.transform.LookAt(target);
    }
}