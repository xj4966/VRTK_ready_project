using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextWriter3D : MonoBehaviour
{
  public string text;
  public GameObject LetterContainer;
  public float kernal = -0.693f;

  GameObject[] letters;
  string prevText;
  float prevKernal = -0.693f;
  GameObject wordParent;

  private void Start()
  {
    SetUpMeshes();

    TranslateTextToMesh("TEST");
  }

  private void Update()
  {
    if(text != prevText || kernal != prevKernal)
    {
      Destroy(wordParent);
      TranslateTextToMesh(text);
      prevText = text;
      prevKernal = kernal;
    }
  }

  void SetUpMeshes()
  {
    letters = new GameObject[LetterContainer.transform.childCount];

    for (int i = 0; i < LetterContainer.transform.childCount; i++)
    {
      letters[i] = LetterContainer.transform.GetChild(i).gameObject;
    }
  }

  void TranslateTextToMesh(string text)
  {
    if (text.Length > 0)
    {
      text = text.ToUpper();

      char[] activeLetters = text.ToCharArray();
      wordParent = new GameObject();
      wordParent.transform.parent = transform;
      wordParent.transform.localPosition = Vector3.zero;

      for (int i = 0; i < activeLetters.Length; i++)
      {
        if((int)activeLetters[i] - 32 != 0)
        {
          GameObject tempLetter = Instantiate(letters[(int)activeLetters[i] - 65], wordParent.transform);
          tempLetter.transform.localPosition += new Vector3(i * kernal, 0, 0);
        }
      }
    }
  }
}
