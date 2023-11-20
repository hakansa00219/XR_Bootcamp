using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameEventManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private GameObject _failedPanel;
    [SerializeField] private GameObject _successPanel;
    [SerializeField] private float _canvasFadeTime = 2f;


    [Header("Audio")]
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioClip _caughtMusic;
    [SerializeField] private AudioClip _successMusic;

    [Header("Player")]
    [SerializeField] private PlayerInput _playerInput;

    private FirstPersonController _fpController;
    private bool _isFadingIn = false;
    private float _fadeLevel = 0;
    // Start is called before the first frame update
    void Start()
    {
        EnemyController[] enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);

        foreach (EnemyController enemy in enemies)
        {
            enemy.onInvestigate.AddListener(EnemyInvestigating);
            enemy.onPlayerFound.AddListener(PlayerFound);
            enemy.onReturnToPatrol.AddListener(EnemyReturnToPatrol);
        }

        if(_playerInput)
        {
            _fpController = _playerInput.GetComponent<FirstPersonController>();
        }


        /*
         * if(player) {
         *      _playerInput = player.GetComponent<PlayerInput>();
         *  }
         *  else
         *  {
         *      Debug.LogWarning("There is no player in the scene");
         *  }
         */

        _canvasGroup.alpha = 0;
        _failedPanel.SetActive(false);
        _successPanel.SetActive(false);

    }

    private void EnemyReturnToPatrol()
    {
    }

    private void PlayerFound(Transform enemyThatFoundPlayer)
    {
        _isFadingIn = true;

        _failedPanel.SetActive(true);

        _fpController.CinemachineCameraTarget.transform.LookAt(enemyThatFoundPlayer);

        DeactivateInput();
        PlayBGM(_caughtMusic);
    }

    public void GoalReached()
    {
        _isFadingIn = true;

        _successPanel.SetActive(true);

        DeactivateInput();
        PlayBGM(_successMusic);
    }

    private void DeactivateInput()
    {
        _playerInput.DeactivateInput();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void PlayBGM(AudioClip newBGM)
    {
        if (_bgmSource.clip == newBGM) return;

        _bgmSource.clip = newBGM;
        _bgmSource.Play();
    }

    private void EnemyInvestigating()
    {
    }
    public void RestartScene()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isFadingIn)
        {
            if (_fadeLevel < 1)
            {
                _fadeLevel += Time.deltaTime / _canvasFadeTime;
                
            }
        }
        else
        {
            if (_fadeLevel > 0f)
            {
                _fadeLevel -= Time.deltaTime / _canvasFadeTime;

            }
        }
        _canvasGroup.alpha = _fadeLevel;
    }
}
