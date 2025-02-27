using System.Collections;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class OpponetAI : MonoBehaviour
{
    [Header("Oppponetement")]
    public float movementSpeed = 1f;
    public float rotationSpeed = 10f;
    public CharacterController characterController;
    public Animator animator;

    [Header("Opponet Fight")]
    public float attckCooldown = 0.5f;
    public int attackDamages = 5;
    public string[] attackAnimations = { "AttackAnimation", "Attack2Animation", "Attack3Animation", "Attack4Animation" };
    public float dodgeDistance = 2f;
    public int attackCount = 0;
    public int randomNumber;
    public float attackRadius = 2f;
    public FightingController[] fightingController;
    public Transform[] players;
    public bool isTackingDamage;
    private float lastAttackTime;

    [Header("Effects and Sound")]
    public ParticleSystem attack1Effect;
    public ParticleSystem attack2Effect;
    public ParticleSystem attack3Effect;
    public ParticleSystem attack4Effect;

    public AudioClip[] hitSounds;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    void Awake()
    {
        currentHealth = maxHealth;
        healthBar.GiveFullHealth(currentHealth);
        createRandomNumber();
    }

    void Update()
    {

        // if(attackCount == randomNumber) 
        // {
        //    attackCount = 0;
        //    createRandomNumber();
        // }

        for (int i = 0; i < fightingController.Length; i++)
        {
            if(players[i].gameObject.activeSelf && Vector3.Distance(transform.position, players[i].position) <= attackRadius)
            {
                animator.SetBool("Walking", false);

                if(Time.time - lastAttackTime > attckCooldown)
                { 
                    int randomAttackIndex = Random.Range(0, attackAnimations.Length);

                    if (!isTackingDamage) 
                    { 
                        PerformAttack(randomAttackIndex);

                        // Phát hoạt ảnh đánh/sát thương trên người chơi
                        fightingController[i].StartCoroutine(fightingController[i].PlayHitDamageAnimation(attackDamages));
                    }
                }
            }
            else
            {
                if (players[i].gameObject.activeSelf)
                {
                    Vector3 direction = (players[i].position - transform.position).normalized;
                    characterController.Move(direction * movementSpeed * Time.deltaTime);

                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                    animator.SetBool("Walking", true);
                }
            } 
        }
    }

    void PerformAttack(int attackIndex)
    {

            animator.Play(attackAnimations[attackIndex]);

            int damage = attackDamages;
            Debug.Log("Performed attack " + (attackIndex + 1) + " dealing " + damage + "damage");

            lastAttackTime = Time.time;
    }
    void PerfromDodgeFront()
    {
        animator.Play("DodgeFrontAnimation");

        Vector3 dodgeDirection = -transform.forward * dodgeDistance;

        characterController.SimpleMove(dodgeDirection);
    }

    void createRandomNumber() 
    {
        randomNumber = Random.Range(1, 5);
    }

    public IEnumerator PlayHitDamageAnimation(int takeDamage)
    {
        yield return new WaitForSeconds(0.5f);

        //phát ra âm thanh đánh ngẫu nhiên
        if(hitSounds != null && hitSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, hitSounds.Length);
            AudioSource.PlayClipAtPoint(hitSounds[randomIndex], transform.position);
        }

        //giảm sức khỏe
        currentHealth -= takeDamage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

        animator.Play("HitDamageAnimation");
    }

    void Die()
    {
        Debug.Log("Opponent died.");
    }

    public void Attack1Effect()
    {
        attack1Effect.Play();
    }

    public void Attack2Effect()
    {
        attack2Effect.Play();
    }

    public void Attack3Effect()
    {
        attack3Effect.Play();
    }

    public void Attack4Effect()
    {
        attack4Effect.Play();
    }
}
