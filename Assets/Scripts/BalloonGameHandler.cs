using System.Collections;
using System.Collections.Generic;
using Audio_Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BalloonGameHandler : MonoBehaviour
{
    [SerializeField] GameObject _balloonPrefab;
    [SerializeField] GameObject _player;
    [SerializeField] Basket _catchingNet;
    [SerializeField] BalloonCollector _balloonCollecter;
    [SerializeField] float _spawnRange;
    [SerializeField] float _spawnHeight = 6;
    [SerializeField] float _spawnHeightRange = 3;
    List<Balloon> _balloons;
    
    [Tooltip("Spawn rate 1 means that only necessary balloons spawn, spawn rate 2 means that double of the required balloons spawn")]
    [SerializeField] int _spawnRate = 1;
    
    [Tooltip("difficulty in procentage, 0 means no unnecessary letters, 1 means all extra letters are unnecessary")]
    [Range(0.0f, 1.0f)][SerializeField] float _difficulty = 1;

    [SerializeField] string[] _words;
    [SerializeField] Color[] _wordColors;
    [SerializeField] List<int> _availableWords;
    [SerializeField] int _currentWord = 0;
    [SerializeField] int _winAmount = 3;

    bool _anyOrder = true;
    int _currentLetter = 0;
    List<bool> _collectedLetters;

    [SerializeField] private TextMeshPro _displayWord;

    private bool _isDropping;
    private float _droppingTimer = 0;
    private bool _caughtBalloon = false;
    private bool _completedGame = false;

    
    // Audio vars
    [SerializeField] private SFX sounds;

    // Start is called before the first frame update
    void Start()
    {
        _balloons = new List<Balloon>();
        if(_availableWords == null || (_availableWords != null && _availableWords.Count == 0))
        {
            for(int i = 0; i < _words.Length; i++)
            {
                _availableWords.Add(i);
            }
        }

        LoadNextWord();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) Debug.Break();

        if(_completedGame)
        {
            _displayWord.text = "Gefeliciteerd! Je hebt alle ballonnen verzameld!";
            return;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            LoadNextWord();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Win();
        }

        if (_catchingNet.holdsBalloon && _caughtBalloon == false)
        { 
            CatchBalloon(_catchingNet.caughtBalloon);
            _caughtBalloon = true;
        }

        for(int i = 0; i < _balloonCollecter.collectedBalloons.Count; i++)
        {
            CollectLetter(_balloonCollecter.collectedBalloons[i].letter);
            sounds.Play("Conversations/affirmations");
            _balloons.Remove(_balloonCollecter.collectedBalloons[i]);
            Destroy(_balloonCollecter.collectedBalloons[i]);
        }
        _balloonCollecter.collectedBalloons.Clear();

        if (Input.GetKeyDown(KeyCode.A) || _isDropping || OVRInput.Get(OVRInput.Button.One))
        {
            if (_catchingNet.holdsBalloon == true)
            {
                _catchingNet.caughtBalloon.rigidBody.freezeRotation = false;
                _catchingNet.caughtBalloon.rigidBody.useGravity = true;
                _catchingNet.caughtBalloon.transform.parent = this.transform.parent;
                _catchingNet.caughtBalloon.IsInNet = false;
                _catchingNet.ClearBalloon();
                _catchingNet.transform.gameObject.SetActive(false);
                _isDropping = true;
                _caughtBalloon = false;
            }
            if (_droppingTimer > 2)
            {
                _droppingTimer = 0;
                _isDropping = false;
                _catchingNet.transform.gameObject.SetActive(true);
            }
            else _droppingTimer += Time.deltaTime;
        }

        if (_catchingNet.holdsBalloon && (Input.GetKeyDown(KeyCode.R) || OVRInput.Get(OVRInput.Button.Two)))
        {
            if (_catchingNet.caughtBalloon.hasCorrectLetter)
            {
                SpawnBalloon(_catchingNet.caughtBalloon.letter);
            }
            Destroy(_catchingNet.caughtBalloon);
            _balloons.Remove(_catchingNet.caughtBalloon);
            _catchingNet.ClearBalloon();
            _caughtBalloon = false;
        }

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

    void LoadNextWord()
    {
        Debug.Log("loading new word");
        int newWord = Random.Range(0, _availableWords.Count-1);
        Debug.Log("loading " + newWord);
        _currentWord = _availableWords[newWord];
        _availableWords.RemoveAt(newWord);

        LoadCurrentWord();
    }

    void LoadCurrentWord()
    {
        RemoveAllBalloons();

        if (_wordColors.Length > _currentWord) _balloonPrefab.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_BaseColor", _wordColors[_currentWord]);

        _collectedLetters = new List<bool>();
        _currentLetter = 0;

        string displayWord = "";

        //spawn the needed balloons
        for(int i = 0; i < _words[_currentWord].Length; i++)
        {
            if (Random.Range(0, 6) > 3 || (i == _words[_currentWord].Length - 1 && displayWord == ""))
            {
                _collectedLetters.Add(true);
                displayWord += _words[_currentWord][i];
            }
            else
            {
                _collectedLetters.Add(false);

                SpawnBalloon(_words[_currentWord][i]);
                if (displayWord == "") _currentLetter = i;
                displayWord += '_';
            }
        }
        Debug.Log("Adding random letters");
        for(int i = 0; i < _words[_currentWord].Length*_spawnRate-_words[_currentWord].Length; i++)
        {
            float procentage = Random.Range(0.0f, 1.0f);
            char letter;
            if(_difficulty > procentage)
            {
                letter = (char)Random.Range('a', 'z');

                for(int j = 0; j < _words[_currentWord].Length; j++)
                {
                    if(letter == _words[_currentWord][j])
                    {
                        j = 0;
                        if (letter != 'z') letter++;
                        else letter = 'a';
                    }
                }
            }
            else
            {
                int letterToGet = Random.Range(0, _words[_currentWord].Length);
                letter = _words[_currentWord][letterToGet];
            }
            SpawnBalloon(letter);
        }
        Debug.Log("spawned all balloons!");
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

        if (spawnX == 0) spawnX = _spawnRange;
        else spawnX = -_spawnRange;
        if (spawnZ == 0) spawnZ = _spawnRange;
        else spawnZ = -_spawnRange;

        Vector2 spawnPos = new Vector2(spawnX, spawnZ);
        spawnPos.Normalize();
        spawnPos.x *= _spawnRange;
        spawnPos.y *= _spawnRange;

        return SpawnBalloon(new Vector3(spawnPos.x, Random.Range(_spawnHeight-_spawnHeightRange, _spawnHeight+_spawnHeightRange), spawnPos.y), pChar);
    }

    bool CatchBalloon(Balloon balloon, int pIndex = -1)
    {
        bool correct = CollectLetter(balloon.letter, false);

        balloon.prefab.transform.parent = _player.transform;
        balloon.beingHeld = true;
        balloon.hasCorrectLetter = correct;
        balloon.rigidBody.freezeRotation = true;
        balloon.rigidBody.useGravity = false;
        balloon.IsInNet = false;
        Debug.Log("correct letter: " + correct);
        sounds.Play("Letters/" + balloon.letter);

        return correct;
    }

    bool CollectLetter(char letter, bool addToWord = true)
    {
        if (_anyOrder)
        {
            for (int i = 0; i < _words[_currentWord].Length; i++)
            {
                if (_words[_currentWord][i] == letter && _collectedLetters[i] == false)
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
                    Debug.Log("This is char letter: " + letter);
                    return true;
                }
            }
        }
        else
        {
            Debug.Log("to collect: " + _words[_currentWord][_currentLetter] + " found " + letter);
            if (letter == _words[_currentWord][_currentLetter])
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
                    if (_currentLetter == _words[_currentWord].Length) Win();
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
        // Play audio
        Debug.Log(_words[_currentWord]);
        sounds.Play("Words/" + _words[_currentWord]);
        
        _winAmount--;
        if (_winAmount == 0)
        {
            _completedGame = true;
            RemoveAllBalloons();
            return;
        }

        if (_words.Length > _currentWord) LoadNextWord();
    }

    void RemoveAllBalloons()
    {
        while (_balloons.Count != 0)
        {
            Destroy(_balloons[0]);
            _balloons.Remove(_balloons[0]);
            _catchingNet.ClearBalloon();
            _caughtBalloon = false;
        }
    }
}
