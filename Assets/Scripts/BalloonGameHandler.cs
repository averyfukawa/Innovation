using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BalloonGameHandler : MonoBehaviour
{
    [SerializeField] GameObject _balloonPrefab;
    [SerializeField] GameObject _player;
    [SerializeField] Basket _basket;
    [SerializeField] float spawnRange;
    [SerializeField] float spawnHeight = 6;
    [SerializeField] float spawnHeightRange = 3;
    List<Balloon> _balloons;
    
    [Tooltip("Spawn rate 1 means that only necessary balloons spawn, spawn rate 2 means that double of the required balloons spawn")]
    [SerializeField] int spawnRate = 1;
    
    [Tooltip("difficulty 1 means that only necessary letters spawn, difficulty 2 means that double of the required letters spawn")]
    [SerializeField] int difficulty = 1;

    [SerializeField] string[] words;
    [SerializeField] int currentWord = 0;

    bool _anyOrder = false;
    int _currentLetter = 0;
    List<bool> _collectedLetters;
    
    [SerializeField] private TextMeshProUGUI uiWord; // UI displaying the word, can be changed to in-world later
    [SerializeField] private TextMeshProUGUI uiLetter; // this should be removed later, for the word to be faded out only
    [SerializeField] private TextMeshPro _displayWord;

    private bool _isDropping;
    private bool _turningBack;
    private float _turnBackTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        _balloons = new List<Balloon>();
        LoadNewWord();

        uiWord.text = words[currentWord];
        uiLetter.SetText("Letter: " + words[currentWord][_currentLetter]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) Debug.Break();
        if(Input.GetKeyDown(KeyCode.A) || _isDropping || _turningBack)
        {
            if(_turningBack == false) _isDropping = true;
            if (_isDropping)
            {
                _basket.transform.Rotate(10, 0, 0);
                if (_basket.transform.rotation.eulerAngles.x >= 180)
                {
                    _basket.transform.rotation = Quaternion.Euler(180, _basket.transform.rotation.eulerAngles.y, 0);
                    if (_basket.caughtBalloon != null)
                    {
                        _basket.caughtBalloon.rigidBody.freezeRotation = false;
                        _basket.caughtBalloon.rigidBody.useGravity = true;
                        _basket.caughtBalloon = null;
                    }
                    _turningBack = true;
                    _isDropping = false;
                }
            }
            Debug.Log(_turnBackTimer);
            if (_turningBack && _turnBackTimer >= 4)
            {
                if (_basket.transform.rotation.eulerAngles.x <= 10)
                {
                    _basket.transform.rotation = Quaternion.Euler(0, _basket.transform.rotation.eulerAngles.y, 0);
                    _turningBack = false;
                    _turnBackTimer = 0;
                }
                else _basket.transform.Rotate(-10, 0, 0);
            }
            else if (_turningBack) _turnBackTimer += Time.deltaTime;
        }
        
        if (_basket.caughtBalloon != null)
        { 
            Debug.Log("basket catch!");
            CatchBalloon(_basket.caughtBalloon);
        }

        for (int i = 0; i < _balloons.Count; i++)
        {
            _balloons[i].Move();
            if(_balloons[i].transform.position.y < 0)
            {
                if(_balloons[i].hasCorrectLetter != true) SpawnBalloon(_balloons[i].letter);
                Destroy(_balloons[i]);
                _balloons.RemoveAt(i);
            }
        }
    }

    void LoadNewWord()
    {
        while(_balloons.Count != 0)
        {
            Destroy(_balloons[0]);
            _balloons.Remove(_balloons[0]);
        }

        _collectedLetters = new List<bool>();
        _currentLetter = 0;
        _displayWord.text = words[currentWord];

        //spawn the needed balloons
        for(int i = 0; i < words[currentWord].Length; i++)
        {
            _collectedLetters.Add(false);
            
           SpawnBalloon(words[currentWord][i]);
        }
    }

    Balloon SpawnBalloon(Vector3 pPos, char pChar)
    {
        GameObject balloon = Instantiate(_balloonPrefab, this.transform);
        Balloon balloonScript = balloon.AddComponent<Balloon>();
        balloonScript.Init(balloon, pPos, _player.transform.position, balloon.GetComponentInChildren<TextMeshPro>(), pChar);
        _balloons.Add(balloonScript);

        return balloonScript;
    }

    Balloon SpawnBalloon(char pChar)
    {
        float spawnX = Random.Range(0, 2);
        float spawnZ = Random.Range(0, 2);

        if (spawnX == 0) spawnX = spawnRange;
        else spawnX = -spawnRange;
        if (spawnZ == 0) spawnZ = spawnRange;
        else spawnZ = -spawnRange;

        Vector2 spawnPos = new Vector2(spawnX, spawnZ);
        spawnPos.Normalize();
        spawnPos.x *= spawnRange;
        spawnPos.y *= spawnRange;

        return SpawnBalloon(new Vector3(spawnPos.x, Random.Range(spawnHeight-spawnHeightRange, spawnHeight+spawnHeightRange), spawnPos.y), pChar);
    }

    bool CatchBalloon(Balloon balloon, int pIndex = -1)
    {
        bool correct = CollectLetter(balloon.letter);

        balloon.prefab.transform.parent = _player.transform;
        balloon.beingHeld = true;
        balloon.hasCorrectLetter = correct;
        balloon.rigidBody.freezeRotation = true;
        balloon.rigidBody.useGravity = false;
        Debug.Log("added to player");

        return correct;
    }

    bool CollectLetter(char letter)
    {
        if (_anyOrder)
        {
            for (int i = 0; i < words[currentWord].Length; i++)
            {
                if (words[currentWord][i] == letter && _collectedLetters[i] == false)
                {
                    _collectedLetters[i] = true;
                    CheckWin();
                    return true;
                }
            }
        }
        else
        {
            Debug.Log("to collect: " + words[currentWord][_currentLetter] + " found " + letter);
            if (letter == words[currentWord][_currentLetter])
            {
                _currentLetter++;
                if (_currentLetter == words[currentWord].Length) Win();
                return true;
            }
            else return false;
        }

        return false;
    }

    void CheckWin()
    {
        for(int i = 0; i < _collectedLetters.Count; i++)
        {
            if(_collectedLetters[i] == false)
            {
                return;
            }
        }

        Win();        
    }

    void Win()
    {
        Debug.Log("Win");
        currentWord++;
        
        if (currentWord < words.Length)
        {
            uiWord.text = words[currentWord];
        }
        
        //if (words.Length > currentWord) LoadNewWord();
    }
}
