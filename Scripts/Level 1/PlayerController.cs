using UnityEngine;
using System.Collections;
public class PlayerController : MonoBehaviour {
  public float speed = 5f;
  public float jumpSpeed = 8f;
  private float movement = 0f;
  public int coinValue;
  public int CheckValue;
  private Rigidbody2D rigidBody;
  public Transform groundCheckPoint;
  public float groundCheckRadius;
  public LayerMask groundLayer;
  private bool isTouchingGround;
  private Animator playerAnimation;
  public Vector3 respawnPoint;
  public LevelManager gameLevelManager;
  // Use this for initialization
  void Start () {
    rigidBody = GetComponent<Rigidbody2D> ();
    playerAnimation = GetComponent<Animator> ();
    gameLevelManager = FindObjectOfType<LevelManager> ();
  }

  // Update is called once per frame
  void Update () {
    isTouchingGround = Physics2D.OverlapCircle (groundCheckPoint.position, groundCheckRadius, groundLayer);
    movement = Input.GetAxis ("Horizontal");
    if (movement > 0f) {
      rigidBody.velocity = new Vector2 (movement * speed, rigidBody.velocity.y);
      transform.localScale = new Vector2(1f,1f);
    }
    else if (movement < 0f) {
      rigidBody.velocity = new Vector2 (movement * speed, rigidBody.velocity.y);
      transform.localScale = new Vector2(-1f,1f);
    }
    else {
      rigidBody.velocity = new Vector2 (0,rigidBody.velocity.y);
    }
    if(Input.GetButtonDown ("Jump") && isTouchingGround){
      rigidBody.velocity = new Vector2(rigidBody.velocity.x,jumpSpeed);
    }
    playerAnimation.SetFloat ("Speed", Mathf.Abs (rigidBody.velocity.x));
    playerAnimation.SetBool ("OnGround", isTouchingGround);
  }

         void OnTriggerEnter2D(Collider2D other){
           if (other.tag == "FallDetector") {
                     gameLevelManager.CheckLiveNumber();
                 }
           if (other.tag == "FinishFlag") {
                     gameLevelManager.CheckFinish();
                 }
           if(other.tag == "CheckPoint"){
                  respawnPoint = gameObject.transform.position;
                  gameLevelManager.AddCheckScore(CheckValue);
                  gameLevelManager.AddrespawnPoint(respawnPoint);
                  Destroy (other.gameObject);
                 }
           if(other.tag == "Coin"){
                    GetComponent<AudioSource>().Play();
                    gameLevelManager.AddCoins(coinValue);
                    gameLevelManager.AddCoinScore(coinValue);
                    Destroy (other.gameObject);
                  }
           if(other.tag == "Bombs"){
                    gameLevelManager.CheckLiveNumber();
                    Destroy (other.gameObject);
                   }
         }
}