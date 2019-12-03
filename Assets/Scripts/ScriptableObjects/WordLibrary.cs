using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WordLibrary", menuName = "ScriptableObjects/WordLibrary", order = 0)]
public class WordLibrary : ScriptableObject
{
    public string[] words;
    public Color[] colors;
    public Dictionary<string, LetterPronounciation> wordSounds;

    public void CompileLibrary()
    {
        wordSounds = new Dictionary<string, LetterPronounciation>();
        for(int i = 0; i < words.Length; i++)
        {
            LetterPronounciation sounds = new LetterPronounciation(words[i]);
            sounds.CompileWord(words[i]);
            wordSounds.Add(words[i], sounds);
        }
    }

    public bool HasCompiled(bool doCompile = true)
    {
        if(wordSounds != null && words.Length == wordSounds.Count)
        {
            return true;
        }
        else
        {
            if (doCompile) CompileLibrary();
            return false;
        }
    }

    public int GetSyllableCountFor(int index)
    {
        if (index >= words.Length)
        {
            return 0;
        }
        return wordSounds[words[index]].syllables.Count;
    }

    public int GetSyllableCountFor(string word)
    {
        if (wordSounds.ContainsKey(word)) return wordSounds[word].syllables.Count;
        else return 0;
    }
}
