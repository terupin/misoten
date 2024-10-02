using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.EventSystems;

public class test_sword : MonoBehaviour
{
    public Transform sowrdTop;  //剣の先端
    public Transform sowrdHit;  //剣の柄

    public Vector3 startPos;  //切り始めの剣の位置
    public Vector3 endPos;  //霧終わりの剣の位置
    
    public LayerMask slice_Mask;  //切れる対象のレイヤー
    private int layer_number;  //レイヤーの番号を取得

    //切断面の色
    public Material Slice_Color;

    public string cut_tag="Cut";
    public Vector3 cutNormal; // Planeの法線

    private void Start()
    {
        //レイヤーをビットマスクから番号に変更する
        layer_number = Mathf.RoundToInt(Mathf.Log(slice_Mask.value, 2));
    }


    //切れるものに当たった時
    private void OnTriggerEnter(Collider other)
    {
        //相手が切れる対象であるかを確認
        if (other.gameObject.tag==cut_tag)
        {
            Debug.Log("当たった");

            //当たった時に存在する刀の場所
            startPos = sowrdTop.position;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("出たよ");

        endPos = sowrdHit.position;

        //剣の軌道に基づくカットする平面の法線を計算
        Vector3 cutNormal = Vector3.Cross(endPos - startPos, startPos);

        //カット平面の作成
        EzySlice.Plane cutPlane=new EzySlice.Plane(startPos,cutNormal);
        DrawPlane(cutPlane);

        //EzySliceで相手をスライスする
        GameObject targetObject = other.gameObject;
        SlicedHull slicedObject = targetObject.Slice(cutPlane, null);  //第２引数は切られた断面のマテリアル

        if (slicedObject != null)
        {
            //スライスされた部分を取得
            GameObject lowerHull = slicedObject.CreateLowerHull(targetObject, null);
            GameObject upperHull = slicedObject.CreateUpperHull(targetObject, null);

            //新しい部分物理コンポーネントを追加
            MakeItPhysical(lowerHull);
            MakeItPhysical(upperHull);

            //元のオブジェクトを削除
            Destroy(targetObject);
        }
    }


    //オブジェクト生成時にMeshColliderとRigidbodyをアタッチする
    private void MakeItPhysical(GameObject obj, Material mat = null)
    {
        //MeshColliderのConvexをtrueにしないと、すり抜けてしまうので注意
        obj.AddComponent<MeshCollider>().convex = true;
        obj.AddComponent<Rigidbody>();
    }


    private void DrawPlane(EzySlice.Plane plane)
    {
        // Planeのメッシュを生成
        GameObject planeObject = new GameObject("Cut Plane");
        MeshFilter meshFilter = planeObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = planeObject.AddComponent<MeshRenderer>();

        // メッシュの頂点、三角形、UVなどを設定
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[4];
        int[] triangles = new int[6];

        // Planeの中心点を取得（平面上の任意の点）
        Vector3 origin = startPos; // 平面の開始位置を使用
        Vector3 normal = cutNormal; // ここでの法線ベクトルを使用

        // Planeの4つの頂点を計算
        Vector3 right = Vector3.Cross(normal, Vector3.up).normalized;
        Vector3 up = Vector3.Cross(normal, right).normalized;

        // メッシュの頂点を設定
        vertices[0] = origin + right * 5 + up * 5; // 左上
        vertices[1] = origin + right * 5 + up * -5; // 左下
        vertices[2] = origin + right * -5 + up * -5; // 右下
        vertices[3] = origin + right * -5 + up * 5; // 右上

        // 三角形を設定
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        triangles[3] = 0;
        triangles[4] = 3;
        triangles[5] = 2;

        // メッシュにデータを適用
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // メッシュの法線を再計算
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        // 材質を設定（必要に応じて変更）
        meshRenderer.material = new Material(Shader.Find("Unlit/Color")) { color = Color.red };

        // Planeの位置を設定
        planeObject.transform.position = origin;
        planeObject.transform.up = normal; // Planeの法線方向を設定
    }


}
