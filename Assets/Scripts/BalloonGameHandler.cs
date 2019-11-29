using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BalloonGameHandler : MonoBehaviour
{
    [SerializeField] GameObject _balloonPrefab;
    [SerializeField] GameObject _player;
    [SerializeField] Basket _catchingNet;
    [SerializeField] BalloonCollecter _balloonCollecter;
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

    bool _anyOrder = true;
    int _currentLetter = 0;
    List<bool> _collectedLetters;

    [SerializeField] private TextMeshPro _displayWord;

    private bool _isDropping;
    private float _droppingTimer = 0;
    private bool caughtBalloon = false;

    // Start is called before the first frame update
    void Start()
    {
        _balloons = new List<Balloon>();
        LoadNewWord();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) Debug.Break();
        if (Input.GetKeyDown(KeyCode.A) || _isDropping || OVRInput.Get(OVRInput.Button.One))
        {
            if (_catchingNet.holdsBalloon == true)
            {
                _catchingNet.caughtBalloon.rigidBody.freezeRotation = false;
                _catchingNet.caughtBalloon.rigidBody.useGravity = true;
                _catchingNet.caughtBalloon.transform.parent = this.transform.parent;
                _catchingNet.ClearBalloon();
                _catchingNet.transform.gameObject.SetActive(false);
                _isDropping = true;
            }
            if (_droppingTimer > 2)
            {
                _droppingTimer = 0;
                _isDropping = false;
                _catchingNet.transform.gameObject.SetActive(true);
                caughtBalloon = false;
            }
            else _droppingTimer += Time.deltaTime;
        }
        
        if (_catchingNet.holdsBalloon && caughtBalloon == false)
        { 
            CatchBalloon(_catchingNet.caughtBalloon);
            caughtBalloon = true;
        }

        for(int i = 0; i < _balloonCollecter.collectedBalloons.Count; i++)
        {
            CollectLetter(_balloonCollecter.collectedBalloons[i].letter);
            _balloons.Remove(_balloonCollecter.collectedBalloons[i]);
            Destroy(_balloonCollecter.collectedBalloons[i]);
        }
        _balloonCollecter.collectedBalloons.Clear();

        for (int i = 0; i < _balloons.Count; i++)
        {
            _balloons[i].Move();
            if(_balloons[i].transform.position.y < 0)
            {
                SpawnBalloon(_balloons[i].letter);
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

        string displayWord = "";

        //spawn the needed balloons
        for(int i = 0; i < words[currentWord].Length; i++)
        {
            if (Random.Range(0, 6) > 3 || (i == words[currentWord].Length - 1 && displayWord == ""))
            {
                _collectedLetters.Add(true);
                displayWord += words[currentWord][i];
            }
            else
            {
                _collectedLetters.Add(false);

                SpawnBalloon(words[currentWord][i]);
                if (displayWord == "") _currentLetter = i;
                displayWord += '_';
            }
        }
        _displayWord.text = displayWord;
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
        bool correct = CollectLetter(balloon.letter, false);

        balloon.prefab.transform.parent = _player.transform;
        balloon.beingHeld = true;
        balloon.hasCorrectLetter = correct;
        balloon.rigidBody.freezeRotation = true;
        balloon.rigidBody.useGravity = false;
        Debug.Log("correct letter: " + correct);

        return correct;
    }

    bool CollectLetter(char letter, bool addToWord = true)
    {
        if (_anyOrder)
        {
            for (int i = 0; i < words[currentWord].Length; i++)
            {
                if (words[currentWord][i] == letter && _collectedLetters[i] == false)
                {
                    if (addToWord)
                    {
                        string displayWord = "";
                        for (int l = 0; l < _displayWord.text.Length; l++)
                        {
                            if (l != i) displayWord += _displayWord.text[l];
                            else displayWord += letter;
                        }
                        _displayWord.text = displayWord;

                        _collectedLetters[i] = true;
                        CheckWin();
                    }
                    return true;
                }
            }
        }
        else
        {
            Debug.Log("to collect: " + words[currentWord][_currentLetter] + " found " + letter);
            if (letter == words[currentWord][_currentLetter])
            {
                if (addToWord)
                {
                    _collectedLetters[_currentLetter] = true;

                    string displayWord = "";
                    for(int i = 0; i < _displayWord.text.Length; i++)
                    {
                        if (i != _currentLetter) displayWord += _displayWord.text[i];
                        else displayWord += letter;
                    }
                    _displayWord.text = displayWord;

                    _currentLetter++;
                    if (_currentLetter == words[currentWord].Length) Win();
                }
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
        
        if (words.Length > currentWord) LoadNewWord();
    }
}
