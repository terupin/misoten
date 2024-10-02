using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.EventSystems;

public class test_sword : MonoBehaviour
{
    //切断面のマテリアル
    public Material Slice_Color;

    //切れるもののタグの名前
    [SerializeField, Header("切れるオブジェクトのタグ")]
    public string cut_tag = "Cut";

    public Transform swordTop;  //剣の先端
    public Transform swordHit;  //剣の柄

    public Vector3 startPos;  //切り始めの剣の位置
    public Vector3 endPos;  //霧終わりの剣の位置

    public Vector3 cutNormal; // Planeの法線

    void Start()
    {
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //切れるオブジェクトか？
        if (other.tag == cut_tag)
        {
            Debug.Log("当たった");

            //当たった時に存在する刀の場所
            startPos = this.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("出たよ");

        //出た時に存在する刀の場所
        endPos = this.transform.position;

        //剣の移動ベクトルを計算する
        Vector3 swordMovement = endPos - startPos;

        //剣の柄と先端のベクトルを計算して、剣の向きを取得
        Vector3 swordDirection = swordTop.position - swordHit.position;

        //剣の軌道に垂直な平面を作成
        Vector3 cutNormal = Vector3.Cross(swordMovement, swordDirection).normalized;
        EzySlice.Plane cutPlane = new EzySlice.Plane(cutNormal, endPos);

        //EzySliceで相手をスライスする
        GameObject targetObject = other.gameObject;
        SlicedHull slicedObject = targetObject.Slice(cutPlane, Slice_Color);  //第２引数は切られた断面のマテリアル

        if (slicedObject != null)
        {
            //スライスされた部分を取得
            GameObject upperHull = slicedObject.CreateUpperHull(targetObject, null);
            GameObject lowerHull = slicedObject.CreateLowerHull(targetObject, null);

            //新しい部分物理コンポーネントを追加
            MakeItPhysical(upperHull);
            MakeItPhysical(lowerHull);

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



}