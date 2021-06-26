using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentPlace : MonoBehaviour
{
    public TMP_Text placeText;

    private void OnEnable()
    {
        placeText.text = SceneInfoTableManager.sceneInfoList.Find(scene => scene.scene_index == (ushort)SceneInfoManager.instance.currentScene).scene_name;

        StartCoroutine(CurrentPlaceAlphaBlending());
    }

    private IEnumerator CurrentPlaceAlphaBlending()
    {
        yield return StartCoroutine(Utility.AlphaBlending(placeText, 1, 1f));
        yield return StartCoroutine(Utility.AlphaBlending(placeText, 0, 1f));
        this.gameObject.SetActive(false);
    }
}
