using UnityEngine;

public class FollowingPlayerCamera : MonoBehaviour
{
    [Header("ปรับแต่งค่าต่างๆ")]
    [Tooltip("ค่าความเร็วของกล้อง")]public float Speed = 100f;
    [Tooltip("เขยิบออกจาก GameObject ข้างต้น")]public Vector3 OffSet = new Vector3(0f,0f,-10f);
    [Header("Ref")]
    [Tooltip("เคลื่อนที่ตาม GameObject ที่ต้องการ")]public Transform Target;
    private void Update() {
        Vector3 TargetPosition = Target.position + OffSet;

        transform.position = Vector3.Lerp(transform.position, TargetPosition, Speed * Time.deltaTime);
    }
    void OnDrawGizmos(){
        if(Target == null){Debug.LogError("ต้องใส่ GameObject ที่ต้องการ ก่อนเล่น");}
        if(OffSet.z > 0f){Debug.LogError("ใส่ตัวเลขค่า Z ให้ต่ำกว่า 0");}
    }
}
