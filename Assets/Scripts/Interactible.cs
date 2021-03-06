﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactible : MonoBehaviour
{
    public bool isInteractive;
    public bool isActive;
    public bool isPickable;
    public bool allowsPlacement;
    public bool allowsMultipleInteractions;
    public string requirementText;
    public string interactionText;
    public bool consumesRequirements;
    public Interactible[] inventoryRequirements;
    public Interactible[] indirectInteractibles;
    public Interactible[] indirectActivations;
    public bool isPuzzle2;
    public bool isRotatable;
    [HideInInspector]
    public int rotations;
    [HideInInspector]
    public int State;
    Collider m_Collider;
	public bool isRotatableQuadrado;
	public bool isSwordPickable;
	public bool CanYouOpenCleopatra;
	public bool CanYouFinishGame;
	
	public Player playerinstance;  
	
	
	public void Start()
	{
		playerinstance = GameObject.FindWithTag("Player").GetComponent<Player>();
		
		//GameObject.Find("espada").GetComponent<Renderer>().enabled = false;
		
	}

    public void Activate()
    {
        isActive = false;
        isInteractive = true;
        m_Collider = GetComponent<Collider>();
        m_Collider.enabled = true;

    }
    public void Deactivate()
    {
        isInteractive = false;
        isActive = false;
        m_Collider = GetComponent<Collider>();
        m_Collider.enabled = false;

    }

    public void Interact()
    {
		
		Rotatequadrados();
		PickSword();
		PutSword();
		OpenCleopatraTomb();
		FinishGame();
		
        if (isPuzzle2)
        {
            if (isInteractive)
            {
                PlayInteractAnimation();
                InteractIndirects();
            }
            else 
            {

                InteractIndirects();

                ActivateIndirects();

                PlayActivateAnimation();

                if (!allowsMultipleInteractions)
                    isInteractive = false;
            }
        }
		
        else
        {
            PlacePlaceables();

            PlayInteractAnimation();

            if (isActive)
            {

                InteractIndirects();

                ActivateIndirects();

                PlayActivateAnimation();

                if (!allowsMultipleInteractions)
                    isInteractive = false;
            }
        }
    }

    private void PlayActivateAnimation()
    {
        Animator animator = GetComponent<Animator>();

        if (animator != null)
            animator.SetTrigger("Activate");
    }

    private void PlayInteractAnimation()
    {
        if (!isRotatable)
        {
            Animator animator = GetComponent<Animator>();
            
            if (animator != null)
                animator.SetTrigger("Interact");
                Debug.Log("Animacao interact");
        }
    }

    private void InteractIndirects()
    {
        if (!isRotatable)
        {
            if (indirectInteractibles != null)
            {
                for (int i = 0; i < indirectInteractibles.Length; ++i)
                    indirectInteractibles[i].Interact();
            }
        } else
            if (indirectInteractibles != null)
        {
            for (int i = 0; i < indirectInteractibles.Length; ++i)
                indirectInteractibles[i].Rotate();
        }

    }

    private void ActivateIndirects()
    {
        if (indirectActivations != null)
        {
            for (int i = 0; i < indirectActivations.Length; ++i)
                indirectActivations[i].Activate();
        }
    }

    private void PlacePlaceables()
    {
        if (allowsPlacement && isActive)
        {
            if (indirectInteractibles != null)
            {
                for (int i = 0; i < indirectInteractibles.Length; ++i)
                    indirectInteractibles[i].gameObject.SetActive(true);
            }
        }
    }
  
    private void Rotate()
    {
        if (isRotatable)
        {
            if(Input.GetKeyDown(KeyCode.F)) rotations += 1;
            Animator animator = GetComponent<Animator>();
            if (rotations == 9) rotations = 1;

            switch (rotations)
            {
                case 1 :
                    animator.SetTrigger("rotate45");
                    break;
                case 2:
                    animator.SetTrigger("rotate90");
                    break;
                case 3:
                    animator.SetTrigger("rotate135");
                    break;
                case 4:
                    animator.SetTrigger("rotate180");
                    break;
                case 5:
                    animator.SetTrigger("rotate225");
                    break;
                case 6:
                    animator.SetTrigger("rotate270");
                    break;
                case 7:
                    animator.SetTrigger("rotate315");
                    break;
                case 8:
                    animator.SetTrigger("rotate360");
                    break;
            }

        }
    }
	
	private void Rotatequadrados() 
	{
		if(isInteractive)
		{
			if(isRotatableQuadrado)
			{
					if(Input.GetKeyDown(KeyCode.F))
					{
						transform.Rotate(0,0,90);
					}
			}
		}	
	}
	
	private void PickSword() 
	{
		if(isInteractive)
		{
			if(isSwordPickable)
			{
				Debug.Log("Picked sword!");
			}
		}	
	}
	
	private void PutSword()
	{
		if(isInteractive)
		{
			if(allowsPlacement && isActive)
			{
		
				if(playerinstance.GetComponent<Player>().HasInInventory(playerinstance.PegaPickable) &&
				 playerinstance.GetComponent<Player>().HasInInventory(playerinstance.MeioPickable) &&
					playerinstance.GetComponent<Player>().HasInInventory(playerinstance.LaminaPickable))
				{
					Debug.Log("Mete as pecas!");
					//PlacePlaceables();
					//GameObject.Find("espada").SetActive(true);
				}
				
			}
		}	
	}
	
	private void OpenCleopatraTomb()
	{
		if (isInteractive)
		{
			if(CanYouOpenCleopatra)
			{
				Debug.Log("Abriu");
			}
		}
	}
	
	public void FinishGame()
	{
		if(CanYouFinishGame)
		{
			Debug.Log("Finish");
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
	}


}
