using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Variables")]
    public float speed;
    public float dashDistance;
    public float dashTime;
    public float dashCooldown;
    [Header("Components")]
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rigidbody2D;
    public WeaponHolder weaponHolder;
    public GameObject dashCooldownBar;

    private bool canMove = true;
    private Vector2 moveDir = Vector2.zero;
    private float dashCooldownTimer = 0;
    private Player player;

    public bool CanMove
    {
        get { return canMove; }
        set
        {
            canMove = value;
            if (value == false)
            {
                rigidbody2D.velocity = Vector2.zero;
            }
        }
    }

    public float DashCooldownTimer
    {
        get { return dashCooldownTimer; }
        set
        {
            dashCooldownTimer = value;
            if (dashCooldownTimer < 0) { dashCooldownTimer = 0; }
        }
    }
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        player = this.GetComponent<Player>();
    }

    void Update()
    {
        moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (moveDir != Vector2.zero) { player.bodyAnimator.SetBool("running", true); }
        else { player.bodyAnimator.SetBool("running", false); }

        if (moveDir.x < 0 && CanMove) { spriteRenderer.flipX = true; }
        else if (moveDir.x > 0 && CanMove) { spriteRenderer.flipX = false; }

        if (Input.GetKeyDown(KeyCode.Space)) { Dash(moveDir); }

        if (DashCooldownTimer > 0)
        {
            DashCooldownTimer -= Time.deltaTime;

            float scale = Mathf.Clamp01(DashCooldownTimer / dashCooldown);
            dashCooldownBar.transform.localScale = new Vector3(scale, dashCooldownBar.transform.localScale.y, 1f);
        }
        else
        {
            dashCooldownBar.transform.localScale = new Vector3(0, dashCooldownBar.transform.localScale.y, 1f);
        }
    }

    private void FixedUpdate()
    {
        if (CanMove)
        {
            rigidbody2D.velocity = moveDir * speed;
        }
    }

    private void Dash(Vector2 direction)
    {
        StartCoroutine(PerformDash(direction));
    }

    IEnumerator PerformDash(Vector2 direction)
    {
        if (DashCooldownTimer == 0.0f && moveDir != Vector2.zero)
        {
            DashCooldownTimer = dashCooldown;
            CanMove = false;
            player.immune = true;

            Vector2 startPosition = transform.position;
            Vector2 endPosition = startPosition + direction * dashDistance;

            RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, dashDistance, LayerMask.GetMask("Terrain"));
            if (hit.collider != null)
            {
                float offset = 0.5f;
                endPosition = hit.point + offset * ((Vector2)transform.position - hit.point);
            }

            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * (dashDistance / dashTime);
                transform.position = Vector2.Lerp(startPosition, endPosition, t);
                yield return null;
            }

            CanMove = true;
            player.immune = false;
        }
    }

    public int GetFacingDirectionMultiplier()
    {
        if (spriteRenderer.flipX) { return 1; }
        return -1;
    }

    
}
