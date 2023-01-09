using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {
	
	public AudioClip crack;
	public Sprite[] hitSprites;
	public static int breakableCount = 0;
	public GameObject smoke;

	private LevelManager levelManager;
	private int timesHit;
	private int maxHits;
	private bool isBreakable;
			
	// Use this for initialization
	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		timesHit = 0;
		maxHits = hitSprites.Length + 1;
		isBreakable = (this.tag == "Breakable");
		//Keep track of breakable bricks
		if(isBreakable){
			breakableCount++;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter2D(Collision2D collision){
		AudioSource.PlayClipAtPoint (crack, transform.position, 0.5f);
		if(isBreakable){
			HandleHits ();
		}
	}
	
	void HandleHits(){
		timesHit++;
		if(timesHit >= maxHits){
			breakableCount--;
			levelManager.BrickDestroyed();
			PuffSmoke ();
			Destroy (gameObject);
		}else{
			LoadSprites();
		}
	}
	
	void PuffSmoke(){
		GameObject smokePuff = Instantiate(smoke, gameObject.transform.position, Quaternion.identity) as GameObject;
		smokePuff.GetComponent<ParticleSystem>().startColor = gameObject.GetComponent<SpriteRenderer>().color;
	}
	
	void LoadSprites(){
		int spriteIndex = timesHit - 1;
		if(hitSprites[spriteIndex] != null){
			this.GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
		}else{
			Debug.LogError("Error: Sprite not found!");
		}
	}
	
	//TODO remove this method
	void SimulateWin(){
		levelManager.LoadNextLevel();
	}
}
