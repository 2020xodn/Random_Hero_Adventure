using UnityEngine;
using System.Collections;

public class DestroyAsteroid : MonoBehaviour {

	private AudioSource snd;
	private Animator anim;
	private GigaGameManager gm;
	
	public void KillAsteroid()
	{
		Destroy (gameObject);
	}

	void Start() {
		anim = GetComponent<Animator>();
		snd = GetComponent<AudioSource>();
		//gm = GameObject.Find ("Game Manager").GetComponent<GigaGameManager>();
	}

	public void DestroyByCollision() {
		anim.SetBool("IsExploding", true);
		snd.Play();
	}
}
