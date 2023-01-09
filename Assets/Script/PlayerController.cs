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
    [Tooltip("รับค่า x และ y")] [SerializeField] float x;
    [Tooltip("เช็คสถานะวิ่งหรือไม่")][SerializeField]bool isRunning;
    [Tooltip("เช็คสถานะยืนอยู่ตรงพื้นหรือไม่")][SerializeField]bool OnGround;
    private void Start() {
        print("ค่าความเร็วในการเดิน : " + WalkingSpeed);
        print("ค่าความเร็วในการวิ่ง : " + RunningSpeed);
        print("ค่าแรงกระโดด : " + JumpForce);
    }
    private void Update() {
        //เรียกใช้ฟังก์ชั้น
        AnimationManager();
        GroundChecking();
        TurnFace();
        // x = รับค่าปุ่มการเคลื่อนที่ในแนวนอน
        x = Input.GetAxis("Horizontal");
        //ถ้ากด LeftShift จะวิ่ง
        if(Input.GetKey(KeyCode.LeftShift)){
            rb.velocity = new Vector2(x * RunningSpeed, rb.velocity.y);
            isRunning = true;
        }else{
            rb.velocity = new Vector2(x * WalkingSpeed, rb.velocity.y);
            isRunning = false;
        }


        //ถ้าแตะพื้นอยู่และกดปุ่ม Space bar จะกระโดด
        if(OnGround == true && Input.GetKeyDown(KeyCode.Space)){
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }
    }
    //สร้างฟังก์ชั้นชื่อ GroundChecking
    void GroundChecking(){
        /**เช็คว่า Player แตะพื้นหรือไม่ด้วยคำสั่ง Physics2D.OverlapCircle (จุดกึ่งกลางของการเช็ค, รัศมีในการเช็ค, LayerMask)**/
        OnGround = Physics2D.OverlapCircle(GroundPos.position, GroundRadius, GroundMask);
    }
    //สร้างฟังก์ชั้นชื่อ TurnFace
    void TurnFace(){
        /** เช็คค่า x โดย x จะรับค่าการเคลื่อนไหวแบบลูกศร หรือ ปุ่ม A / D 
            หากผู้เล่นกดปุ่ม A หรือ ลูกศรด้านซ้าย ค่า x จะติดลบ แต่กลับกัน
            หากผู้เล่นกดปุ่ม D หรือ ลูกศรด้านขวา ค่า x จะไม่ติดลบ **/
        
        /** จึงสามารถนำข้อสังเกตุข้างต้นมาประยุกต์ได้เป็น
            ปรับค่าการหมุนด้วยคำสั่ง Quaternion.Euler(float x, float y, float z);
                            {x = การหมุนในแนวแกน X, y = การหมุนในแนวแกน Y, z = การหมุนในแนวแกน Z} **/
        if(x < 0){
            transform.rotation = Quaternion.Euler(0f,180f,0f);
        }else{
            transform.rotation = Quaternion.Euler(0f,0f,0f);
        }

        /** แปลคำสั่งข้างต้นได้ว่า 
            ถ้า(x น้อยกว่า 0){
                ค่าการหมุนดั้งเดิม เปลื่ยนเป็น หมุน(0, 180, 0);    
            }นอกเหนือจากนี้{
                ค่าการหมุน เปลื่ยนเป็น หมุน(0, 0, 0);
            }**/

    }



    void AnimationManager(){
        if(isRunning == true){
            Anim.SetFloat("Walking", 0);
            Anim.SetFloat("Running", Mathf.Abs(x));

        }else{
            Anim.SetFloat("Running", 0);
            Anim.SetFloat("Walking", Mathf.Abs(x));
        }
        /** แปลคำสั่งข้างต้นได้ว่า 
            ถ้า(กำลังวิ่ง){
                เล่นอนิเมชั่น เดิน เป็น เท็จ(ไม่เล่นอนิเมชั่นเดิน)    
                เล่นอนิเมชั่น วิ่ง เป็น จริง(เล่นอนิเมชั่นวิ่ง)    
            }นอกเหนือจากนี้{
                เล่นอนิเมชั่น เดิน เป็น จริง(เล่นอนิเมชั่นเดิน)    
                เล่นอนิเมชั่น วิ่ง เป็น เท็จ(ไม่เล่นอนิเมชั่นวิ่ง)    
            }**/

        if(OnGround == true){
            Anim.SetBool("Jumping", false);
        }else{
            Anim.SetBool("Jumping", true);
        }
        /** แปลคำสั่งข้างต้นได้ว่า 
            ถ้า(แตะพื้นอยู่จริง){
                เล่นอนิเมชั่น กระโดด เป็น เท็จ(ไม่เล่นอนิเมชั่นกระโดด)    
            }นอกเหนือจากนี้{
                เล่นอนิเมชั่น กระโดด เป็น จริง(เล่นอนิเมชั่นกระโดด)    
            }**/

    }
    // Debug Zone
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
