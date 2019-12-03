using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterPronounciation : MonoBehaviour
{
    public LetterPronounciation(string word)
    {
        syllables = new List<string>();
    }
    
    public List<string> syllables;
    
    public void CompileWord(string word)
    {
        for (int i = 0; i < word.Length; i++)
        {
            if (i == word.Length - 1)
            {
                syllables.Add(word[i].ToString());
                continue;
            }

            if (word[i] == 'a')
            {
                if(word[i+1] == 'a')
                {
                    syllables.Add("aa");
                    i++;
                    continue;
                }
                else if (word[i+1] == 'u')
                {
                    syllables.Add("au");
                    i++;
                    continue;
                }
                else
                {
                    syllables.Add("a");
                    continue;
                }
            }
            else if (word[i] == 'e')
            {
                if (word[i + 1] == 'e')
                {
                    syllables.Add("ee");
                    i++;
                    continue;
                }
                else if (word[i + 1] == 'u')
                {
                    syllables.Add("eu");
                    i++;
                    continue;
                }
                else if (word[i + 1] == 'i')
                {
                    syllables.Add("ei");
                    i++;
                    continue;
                }
                else
                {
                    syllables.Add("e");
                    continue;
                }
            }
            else if (word[i] == 'i')
            {
                if (word[i + 1] == 'e')
                {
                    syllables.Add("ie");
                    i++;
                    continue;
                }
                else if (word[i + 1] == 'j')
                {
                    syllables.Add("ij");
                    i++;
                    continue;
                }
                else
                {
                    syllables.Add("i");
                    continue;
                }
            }
            else if (word[i] == 'o')
            {
                if (word[i + 1] == 'e')
                {
                    syllables.Add("oe");
                    i++;
                    continue;
                }
                else if (word[i + 1] == 'o')
                {
                    syllables.Add("oo");
                    i++;
                    continue;
                }
                else if (word[i + 1] == 'u')
                {
                    syllables.Add("ou");
                    i++;
                    continue;
                }
                else
                {
                    syllables.Add("o");
                    continue;
                }
            }
            else if (word[i] == 'u')
            {
                if (word[i + 1] == 'i')
                {
                    syllables.Add("ui");
                    i++;
                    continue;
                }
                else if (word[i + 1] == 'u')
                {
                    syllables.Add("uu");
                    i++;
                    continue;
                }
                else
                {
                    syllables.Add("u");
                    continue;
                }
            }
            else
            {
                syllables.Add(word[i].ToString());
            }
        }
    }

    static public string[] allSounds =
    {
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
        "aa", "au", "ee", "ei", "eu", "ie", "ij", "oe", "oo", "ou", "ui", "uu"
    };

}
