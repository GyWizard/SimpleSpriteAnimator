using UnityEngine;
using SimpleSpriteAnimator;

public class SimpleCharacterController : MonoBehaviour
{
    public SpriteAnimator SpriteAnimator;
    public float Speed;
    private bool _isAttacking;
    private bool _isMoving;

    public void Awake()
    {
        //Subscribe to the animation events
        SpriteAnimator.OnAnimationEnded += OnEndAttackAnimation;
        SpriteAnimator.OnFrameChanged += PerformAttack;
    }

    public void OnDestroy()
    {
        //Unsubscribe to the animation events
        SpriteAnimator.OnAnimationEnded -= OnEndAttackAnimation;
        SpriteAnimator.OnFrameChanged -= PerformAttack;
    }

    
    //Execute at the 4 frame of Attack animation;
    void PerformAttack((string animationName, int frameIndex) anim)
    {
        if (anim.animationName.Equals("Attack") && (anim.frameIndex == 3))
        {
            Debug.Log("AttackPerformed");
        }
    }

    //Execute at the end of Attack animation;
    void OnEndAttackAnimation(string animationName)
    {
        if (animationName.Equals("Attack"))
        {
            _isAttacking = false;
            //Play Idle animation after attack animation ended
            SpriteAnimator.PlayAnimation("Idle");
        }
    }
    
    void SetAnimation()
    {
        if (_isAttacking)
        {
            return;
        }

        if (_isMoving)
        {
            //Play Walk animation if moving
            SpriteAnimator.PlayAnimation("Walk");
            return;
        }
        //Play Idle animation if no input
        SpriteAnimator.PlayAnimation("Idle");
    }
    void Update()
    {
        ControlPlayer();
        SetAnimation();
    }
    void StartAttack()
    {
        _isAttacking = true;
        SpriteAnimator.PlayAnimation("Attack");
    }
    void ControlPlayer()
    {
        if (_isAttacking)
        {
            return;
        }

        Move(Input.GetAxisRaw("Horizontal"));

        if (Input.GetMouseButtonDown(0))
        {
            StartAttack();
        }
    }
    void Move(float moveDirection)
    {
        _isMoving = moveDirection != 0;
        if (!_isMoving)
        {
            return;
        }

        transform.position += new Vector3(moveDirection, 0, 0) * (Speed * Time.deltaTime);
    }
}