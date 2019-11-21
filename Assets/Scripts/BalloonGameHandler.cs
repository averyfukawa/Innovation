using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BalloonGameHandler : MonoBehaviour
{
    [SerializeField] GameObject _balloonPrefab;
    [SerializeField] GameObject _player;
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
        //if(Input.GetKeyDown(KeyCode.S))
        //{
        //    _balloons.Add(SpawnBalloon(new Vector3(Random.Range(-2, 2), Random.Range(0.5f, 2.5f), Random.Range(-2, 2)), ''));
        //}

        if (Input.GetKeyDown(KeyCode.P)) Debug.Break();

        RaycastHit hit;
        //Debug.DrawRay(player.transform.position, player.transform.forward, Color.red);
        if (Input.GetMouseButton(0) && Physics.Raycast(_player.transform.position, _player.transform.forward, out hit))
        {
            Balloon balloon;
            if (hit.transform.gameObject.TryGetComponent<Balloon>(out balloon))
            {
                if(!CollectLetter(balloon.letter) && !_anyOrder && words[currentWord].Contains(balloon.letter.ToString()))
                {
                    float spawnX = Random.Range(0, 2);
                    float spawnZ = Random.Range(0, 2);

                    if (spawnX == 0) spawnX = Random.Range(1.0f, 3.0f);
                    else spawnX = Random.Range(-1.0f, -3.0f);
                    if (spawnZ == 0) spawnZ = Random.Range(1.0f, 3.0f);
                    else spawnZ = Random.Range(-1.0f, -3.0f);

                    SpawnBalloon(new Vector3(spawnX, Random.Range(1.0f, 2.5f), spawnZ), balloon.letter);
                }
                Destroy(balloon);
                _balloons.Remove(balloon);
            }
        }

        for (int i = 0; i < _balloons.Count; i++)
        {
            _balloons[i].Move();
        }

        if (currentWord < words.Length)
        {
            uiLetter.SetText("Letter: " + words[currentWord][_currentLetter]);
        }
        else
        {
            uiLetter.SetText("Letter: Finished (:");
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

        //spawn the needed balloons
        for(int i = 0; i < words[currentWord].Length; i++)
        {
            _collectedLetters.Add(false);
            float spawnX = Random.Range(0, 2);
            float spawnZ = Random.Range(0, 2);

            if (spawnX == 0) spawnX = Random.Range(1.0f, 3.0f);
            else spawnX = Random.Range(-1.0f, -3.0f);
            if (spawnZ == 0) spawnZ = Random.Range(1.0f, 3.0f);
            else spawnZ = Random.Range(-1.0f, -3.0f);

           SpawnBalloon(new Vector3(spawnX, Random.Range(1.0f, 2.5f), spawnZ), words[currentWord][i]);
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
            Debug.Log("should collect: " + words[currentWord][_currentLetter] + " collected " + letter);
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
        
        if (words.Length > currentWord) LoadNewWord();
    }
}
