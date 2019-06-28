using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	public AudioClip musicSound;
	private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
		audioSource = gameObject.GetComponent<AudioSource>();
		//audioSource.PlayOneShot(musicSound);

	}

    // Update is called once per frame
    void Update()
    {

	}
	public void MusicPlay()
	{
		audioSource.PlayOneShot(musicSound);
	}
}
