using System.Collections;
using System.Collections.Generic;
using Audio_Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BalloonGameHandler : MonoBehaviour
{
    [SerializeField] GameObject _balloonPrefab;
    [SerializeField] GameObject _colorVisualiser;
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

    [SerializeField] WordLibrary library;
    [SerializeField] List<int> _availableWords;
    [SerializeField] int _currentWord = 0;
    [SerializeField] int _winAmount = 3;
    [SerializeField] private string winPhrase = "Goed Gedaan!";
    [SerializeField] GameObject disabledTillWinUI;
    int _currentLetter = 0;
    List<bool> _collectedLetters;

    [SerializeField] private TextMeshPro _displayWord;

    private bool _isDropping;
    private float _droppingTimer = 0;
    private bool _caughtBalloon = false;
    private bool _completedGame = false;
    [SerializeField] float spawnTime = 2;
    private float spawnTimer;
    bool respawnBalloons = false;

    
    // Audio vars
    [SerializeField] private SFX sfx;

    // Start is called before the first frame update
    void Start()
    {
        disabledTillWinUI.SetActive(false);

        _balloons = new List<Balloon>();

        library.HasCompiled();

        for(int i = 0; i < library.words.Length; i++)
        {
            _availableWords.Add(i);
        }

        LoadNextWord();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) Debug.Break();

        if(_completedGame)
        {
            _displayWord.text = winPhrase;
            return;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            LoadNextWord();
        }
        if (OVRInput.GetUp(OVRInput.RawButton.X) || Input.GetKeyDown(KeyCode.W))
        {
            Win();
        }

        if(respawnBalloons)
        {
            spawnTimer += Time.deltaTime;
            if(spawnTimer >= spawnTime)
            {
                LoadNextWord();
                respawnBalloons = false;
                spawnTimer = 0;
            }
            else return;
        }

        if (_catchingNet.holdsBalloon && _caughtBalloon == false)
        { 
            CatchBalloon(_catchingNet.caughtBalloon);
            _caughtBalloon = true;
        }

        for(int i = 0; i < _balloonCollecter.collectedBalloons.Count; i++)
        {
            CollectLetter(_balloonCollecter.collectedBalloons[i].letters);
            sfx.Play("Conversations/affirmations");
            _balloons.Remove(_balloonCollecter.collectedBalloons[i]);
            Destroy(_balloonCollecter.collectedBalloons[i]);
        }
        _balloonCollecter.collectedBalloons.Clear();

        if ( (Input.GetKeyDown(KeyCode.A) || OVRInput.Get(OVRInput.Button.One)) || _isDropping)
        {
            if (_catchingNet.holdsBalloon == true)
            {
                _catchingNet.caughtBalloon.rigidBody.useGravity = true;
                _catchingNet.caughtBalloon.transform.parent = this.transform.parent;
                _catchingNet.caughtBalloon.rigidBody.constraints = RigidbodyConstraints.None;
                _catchingNet.caughtBalloon.IsInNet = false;
                _catchingNet.ClearBalloon();
                _catchingNet.transform.gameObject.SetActive(false);
                _isDropping = true;
                _caughtBalloon = false;
            }
            if (_droppingTimer > 1)
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
                SpawnBalloon(_catchingNet.caughtBalloon.letters);
            }
            sfx.Play("SFX/Balloon Pop");
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
                SpawnBalloon(_balloons[i].letters);
                Destroy(_balloons[i]);
                _balloons.RemoveAt(i);
            }
        }
    }

    void LoadNextWord()
    {
        int newWord = Random.Range(0, _availableWords.Count);
        _currentWord = _availableWords[newWord];
        _availableWords.RemoveAt(newWord);

        LoadCurrentWord();
    }

    void LoadCurrentWord()
    {
        RemoveAllBalloons();
        
        _balloonPrefab.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_BaseColor", library.colors[_currentWord]);

        _collectedLetters = new List<bool>();
        _currentLetter = 0;

        string displayWord = "";

        //spawn the needed balloons
        for(int i = 0; i < library.GetSyllableCountFor(_currentWord); i++)
        {
            if (Random.Range(0, 7) > 3)
            {
                _collectedLetters.Add(true);
                displayWord += library.wordSounds[library.words[_currentWord]].syllables[i];
            }
            else
            {
                _collectedLetters.Add(false);

                SpawnBalloon(library.wordSounds[library.words[_currentWord]].syllables[i]);
                if (displayWord == "") _currentLetter = i;
                displayWord += '-';
            }
        }
        for(int i = 0; i < library.GetSyllableCountFor(_currentWord) * _spawnRate- library.GetSyllableCountFor(_currentWord); i++)
        {
            float procentage = Random.Range(0.0f, 1.0f);
            string letters;
            if(_difficulty > procentage)
            {
                letters = LetterPronounciation.allSounds[Random.Range(0, LetterPronounciation.allSounds.Length)];
            }
            else
            {
                int letterToGet = Random.Range(0, library.GetSyllableCountFor(_currentWord));
                letters = library.wordSounds[library.words[_currentWord]].syllables[letterToGet];
            }
            SpawnBalloon(letters);
        }
        _displayWord.text = displayWord;
    }

    Balloon SpawnBalloon(Vector3 pPos, string pLetters)
    {
        GameObject balloon = Instantiate(_balloonPrefab, this.transform);
        Balloon balloonScript = balloon.AddComponent<Balloon>();
        balloonScript.Init(balloon, pPos, _player.transform.position, balloon.GetComponentInChildren<TextMeshPro>(), pLetters);
        _balloons.Add(balloonScript);

        return balloonScript;
    }

    Balloon SpawnBalloon(string pLetters)
    {
        //float spawnX = Random.Range(0, 2);
        //float spawnZ = Random.Range(0, 2);

        //if (spawnX == 0) spawnX = _spawnRange;
        //else spawnX = -_spawnRange;
        //if (spawnZ == 0) spawnZ = _spawnRange;
        //else spawnZ = -_spawnRange;
        float spawnX = Random.Range(-2, 3);
        float spawnZ = Random.Range(-2, 3);

        Vector2 spawnPos = new Vector2(spawnX, spawnZ);
        spawnPos.Normalize();
        spawnPos.x *= _spawnRange;
        spawnPos.y *= _spawnRange;

        return SpawnBalloon(new Vector3(spawnPos.x, Random.Range(_spawnHeight-_spawnHeightRange, _spawnHeight+_spawnHeightRange), spawnPos.y), pLetters);
    }

    bool CatchBalloon(Balloon balloon, int pIndex = -1)
    {
        bool correct = CollectLetter(balloon.letters, false);

        balloon.prefab.transform.parent = _player.transform;
        balloon.beingHeld = true;
        balloon.hasCorrectLetter = correct;
        balloon.rigidBody.useGravity = false;
        balloon.rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        balloon.IsInNet = true;
        
        sfx.Play("Letters/" + balloon.letters);
        
        return correct;
    }

    bool CollectLetter(string letters, bool addToWord = true)
    {
        for (int i = 0; i < library.GetSyllableCountFor(_currentWord); i++)
        {
            string sound = library.wordSounds[library.words[_currentWord]].syllables[i];

            if (sound == letters && _collectedLetters[i] == false)
            {
                if (addToWord)
                {
                    _collectedLetters[i] = true;

                    string displayWord = "";
                    //for (int l = 0; l < _displayWord.text.Length; l++)
                    //{
                    //    if (l != i) displayWord += _displayWord.text[l];
                    //    else displayWord += letters;
                    //}
                    for(int l = 0; l < _collectedLetters.Count; l++)
                    {
                        if (_collectedLetters[l]) displayWord += library.wordSounds[library.words[_currentWord]].syllables[l];
                        else displayWord += "-";
                    }

                    _displayWord.text = displayWord;
                    CheckWin();
                }
                return true;
            }
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
        sfx.Play("Words/" + library.words[_currentWord]);
        
        _winAmount--;
        if (_winAmount == 0)
        {
            _completedGame = true;
            if(_colorVisualiser != null) _colorVisualiser.SetActive(false);
            RemoveAllBalloons();
            disabledTillWinUI.SetActive(true);
            return;
        }

        respawnBalloons = true;
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
