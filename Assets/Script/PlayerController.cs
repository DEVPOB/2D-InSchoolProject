using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ref")]
    [Tooltip("ใส่ Rigidbody2D ในตัว Player")] public Rigidbody2D rb;
    [Tooltip("ใส่ PlayerAnimator")] public Animator Anim;
    [Tooltip("ใส่ GameObject ทีชื่อ GroundCheck ลงในนี้")] public Transform GroundPos;
    [Tooltip("ใส่ LayerMask พื้น")] public LayerMask GroundMask;

    [Header("ปรับแต่งค่าต่างๆ")]
    [Tooltip("ปรับค่าความเร็วในการเคลื่อนที่(เดิน)")] public float WalkingSpeed = 5f;
    [Tooltip("ปรับค่าความเร็วในการเคลื่อนที่(วิ่ง)")] public float RunningSpeed  = 8f;
    [Tooltip("ปรับค่าแรงกระโดด")] public float JumpForce = 5f;
    [Tooltip("ปรับแต่งค่าเช็คความสูงจากตัวPlayerถึงพื้น")] public float GroundRadius = 0.1f;
    [Header("Debug")]
    [Tooltip("รับค่า x และ y")] [SerializeField] float x;public float y;
    [Tooltip("เช็คสถานะวิ่งหรือไม่")][SerializeField]bool isRunning;
    [Tooltip("เช็คสถานะยืนอยู่ตรงพื้นหรือไม่")][SerializeField]bool OnGround;
    private void Start() {
        print("ค่าความเร็วในการเดิน : " + WalkingSpeed);
        print("ค่าความเร็วในการวิ่ง : " + RunningSpeed);
        print("ค่าแรงกระโดด : " + JumpForce);
    }
    private void Update() {
        AnimationManager();
        GroundChecking();
        x = Input.GetAxis("Horizontal");
        if(x < 0){
            transform.rotation = Quaternion.Euler(0f,180f,0f);
        }else{
            transform.rotation = Quaternion.Euler(0f,0f,0f);

        }
        if(Input.GetKey(KeyCode.LeftShift)){
            rb.velocity = new Vector2(x * RunningSpeed, rb.velocity.y);
            isRunning = true;
        }else{
            rb.velocity = new Vector2(x * WalkingSpeed, rb.velocity.y);
            isRunning = false;
        }



        if(OnGround == true && Input.GetKeyDown(KeyCode.Space)){
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }
    }
    void GroundChecking(){
        OnGround = Physics2D.OverlapCircle(GroundPos.position, 0.1f, GroundMask);
        if(OnGround){
            Anim.SetBool("Jumping", false);
        }else{
            Anim.SetBool("Jumping", true);
        }

    }
    void AnimationManager(){
        if(isRunning == true){
            Anim.SetFloat("Walking", 0);
            Anim.SetFloat("Running", Mathf.Abs(x));

        }else{
            Anim.SetFloat("Running", 0);
            Anim.SetFloat("Walking", Mathf.Abs(x));
        }
        

    }
    void OnDrawGizmos(){
        Gizmos.DrawWireSphere(GroundPos.position, GroundRadius);
        
        WarningSetUp();
        ErrorWarning();
    }
    void ErrorWarning(){
        if(rb == null){Debug.LogError("ต้องใส่ RigidBody2D ก่อนเล่น");}
        if(Anim == null){Debug.LogError("ต้องใส่ Animator ก่อนเล่น");}
        if(GroundPos == null){Debug.LogError("ต้องใส่ GameObject ทีชื่อ GroundCheck ก่อนเล่น");}
    }
    void WarningSetUp(){
        if(WalkingSpeed == 0){Debug.LogWarning("ความเร็วในการเดินของคุณ คือ 0");}
        if(RunningSpeed == 0){Debug.LogWarning("ความเร็วในการวิ่งของคุณ คือ 0");}
        if(JumpForce == 0){Debug.LogWarning("แรงกระโดดของคุณ คือ 0");}
        if(GroundRadius == 0){Debug.LogWarning("ค่าเช็คความสูงจากตัวPlayerถึงพื้นของคูณ คือ 0");}
    }
}
