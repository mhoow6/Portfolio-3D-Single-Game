using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class PatchManager : MonoBehaviour
{
    public Slider downloadSlider;
    public TMP_Text downloadText;
    public TMP_Text loadingText;

    private const float MAX_DOWNLOAD_BYTE = 2.70211e+07f;
    private const float DOWNLOAD_FILES = 10f;
    private string serverAssetBundleDirectory;
    private Hash128 serverAssetBundleHash = new Hash128();
    private string assetBundleDirectory;
    private Hash128 myAssetBundleHash = new Hash128();

    // �����̴� ���̵� �ƿ��뵵
    public Image sliderLoadingBar;
    public Image sliderLoadingFrame;
    public Image sliderLoadingFrameDecoTop;
    public Image sliderLoadingFrameDecoBot;
    public Image sliderLoadingBackBar;
    // public TMP_Text loadingText

    // �ε� ����ũ �뵵. ������ �������� �ʿ����
    private WaitForSeconds completeWaitTime = new WaitForSeconds(0.2f);
    private WaitForSeconds checkWaitTime = new WaitForSeconds(0.02f);
    private const float GAUGE_UP_SPEED = 3.0f;
    private const float MIN_LOADING_TIMER = 0.5f;
    private const float MAX_LOADING_TIMER = 1.5f;

    private void Awake()
    {
        serverAssetBundleDirectory = "file:///" + "C:/Users/mhoow/AppData/LocalLow/JWY/AssetBundles";
        assetBundleDirectory = Application.persistentDataPath + "/Resources";

        downloadSlider.minValue = 0f;
        downloadSlider.maxValue = MAX_DOWNLOAD_BYTE;

        downloadSlider.value = 0f;
    }

    private IEnumerator Start()
    {
        StartCoroutine(LoadingGauge());

        yield return StartCoroutine(PatchAssetBundle("monster.pak", "Monster"));
        ResourceManager.monster = KeepAssetBundle("monster.pak");
        RefreshShader<SkinnedMeshRenderer>(ResourceManager.monster);

        yield return StartCoroutine(PatchAssetBundle("npc.pak", "NPC"));
        ResourceManager.npc = KeepAssetBundle("npc.pak");
        RefreshShader<SkinnedMeshRenderer>(ResourceManager.npc);

        yield return StartCoroutine(PatchAssetBundle("player.pak", "Player"));
        ResourceManager.player = KeepAssetBundle("player.pak");
        RefreshShader<SkinnedMeshRenderer>(ResourceManager.player);

        yield return StartCoroutine(PatchAssetBundle("combinedobject.pak", "CombinedObject"));
        ResourceManager.combinedobject = KeepAssetBundle("combinedobject.pak");
        RefreshShader<MeshRenderer>(ResourceManager.combinedobject);

        yield return StartCoroutine(PatchAssetBundle("terrain.pak", "Terrain"));
        ResourceManager.terrain = KeepAssetBundle("terrain.pak");
        RefreshShader<MeshRenderer>(ResourceManager.terrain);

        yield return StartCoroutine(PatchAssetBundle("rocks.pak", "Rocks"));
        ResourceManager.rocks = KeepAssetBundle("rocks.pak");
        RefreshShader<MeshRenderer>(ResourceManager.rocks);

        yield return StartCoroutine(PatchAssetBundle("vegetation.pak", "Vegetation"));
        ResourceManager.vegetation = KeepAssetBundle("vegetation.pak");
        RefreshShader<MeshRenderer>(ResourceManager.vegetation);

        yield return StartCoroutine(PatchAssetBundle("props.pak", "Props"));
        ResourceManager.props = KeepAssetBundle("props.pak");
        RefreshShader<MeshRenderer>(ResourceManager.props);

        yield return StartCoroutine(PatchAssetBundle("particle.pak", "Particle"));
        ResourceManager.particle = KeepAssetBundle("particle.pak");
        // RefreshShaderFromParticleSystemRenderer(ResourceManager.particle);

        yield return StartCoroutine(PatchAssetBundle("weapon.pak", "Weapon"));
        ResourceManager.weapon = KeepAssetBundle("weapon.pak");
        RefreshShader<MeshRenderer>(ResourceManager.weapon);

        downloadSlider.value = downloadSlider.maxValue;
        downloadText.text = "���� �غ� �Ϸ�";
        StartCoroutine(DownloadCompletedFadeOut());
    }

    private IEnumerator DownloadCompletedFadeOut()
    {
        StartCoroutine(Utility.AlphaBlending(sliderLoadingBar, 0, 1f));
        StartCoroutine(Utility.AlphaBlending(sliderLoadingFrame, 0, 1f));
        StartCoroutine(Utility.AlphaBlending(sliderLoadingFrameDecoTop, 0, 1f));
        StartCoroutine(Utility.AlphaBlending(sliderLoadingFrameDecoBot, 0, 1f));
        StartCoroutine(Utility.AlphaBlending(sliderLoadingBackBar, 0, 1f));
        StartCoroutine(Utility.AlphaBlending(loadingText, 0, 1f));
        yield return StartCoroutine(Utility.AlphaBlending(downloadText, 0, 1f));
        MainMenu.instance.title.ButtonFadeOut();
        yield return null;
    }

    private IEnumerator LoadingGauge()
    {
        float result = 0f;

        while (downloadSlider.value < downloadSlider.maxValue)
        {
            result = downloadSlider.value / MAX_DOWNLOAD_BYTE * 100;

            loadingText.text = "Loading..." + string.Format("{0:0}%", result);
            yield return null;
        }

        // �ֽŹ����� ���ϰ� ��ġ�� �� �ް� �� �� ���ڶ� ���� ä��� ���� ���
        loadingText.text = "Loading..." + string.Format("{0:0}%", 100);
    }

    private IEnumerator PatchAssetBundle(string targetFile, string maniFestName)
    {
        string serverMainfestUrl = serverAssetBundleDirectory + "/" + maniFestName;
        string serverTargetFileUrl = serverAssetBundleDirectory + "/" + targetFile;
        string myManiFestFilePath = assetBundleDirectory + "/" + maniFestName;

        downloadText.text = targetFile + " �˻� ��..";

        // 1. ����������� ���丮�� �� �ִ��� Ȯ��
        FileDirectoryCheck(assetBundleDirectory);

        // 2. �� ��������ҿ� �ִ� Ÿ�� ���¹����� �ؽð��� ���´�.
        if (IsFileInMyAssetBundleDirectory(maniFestName))
            yield return StartCoroutine(GetLocalAssetBundleHash(targetFile, myManiFestFilePath));

        // 2-1. ������ ������ �������� ���� �� �޾ƿ��� ��.
        else
        {
            downloadText.text = targetFile + " �ٿ�ε� ��..";
            yield return StartCoroutine(StoreAssetBundle(serverMainfestUrl));
            downloadText.text = targetFile + " �ٿ�ε� ��..";
            yield return StartCoroutine(StoreAssetBundle(serverTargetFileUrl));
            yield return completeWaitTime;
            yield break;
        }

        // 4. ���� ���ϰ� ���ϱ� ���� ������ �ִ� Ÿ�� ���¹����� �ؽð��� ������
        yield return StartCoroutine(GetServerAssetBundleHash(targetFile, serverMainfestUrl));

        // 5. �ؽð��� ���Ͽ� ������ �ٷ� Ż��
        if (myAssetBundleHash.ToString() == serverAssetBundleHash.ToString())
        {
            yield return FillGaugeSmooth(downloadSlider.maxValue / DOWNLOAD_FILES);
            downloadText.text = targetFile + " �˻� �Ϸ�.";
            yield return checkWaitTime;
            yield break;
        }

        // 5-1. �ؽð��� �ٸ��� �������� ���� �� �޾ƿ��� ��.
        else
        {
            downloadText.text = targetFile + " �ٿ�ε� ��..";
            yield return StartCoroutine(StoreAssetBundle(serverMainfestUrl));
            downloadText.text = targetFile + " �ٿ�ε� ��..";
            yield return StartCoroutine(StoreAssetBundle(serverTargetFileUrl));
            yield return completeWaitTime;
        }
    }

    /// <summary>
    /// ���� ����ҿ� ���丮�� �ִ��� ������ üũ�� �� ���
    /// </summary>
    private void FileDirectoryCheck(string directory)
    {
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
    }

    private bool IsFileInMyAssetBundleDirectory(string fileName)
    {
        string[] _files = Directory.GetFiles(assetBundleDirectory);
        string separator = "\\";

        // \\ �ں��� ���ϸ�.Ȯ���ڸ� ���� �� ����
        for (int i = 0; i < _files.Length; i++)
        {
            int sepIndex = _files[i].IndexOf(separator);

            if (sepIndex != -1)
            {
                string str = _files[i].Substring(sepIndex + separator.Length);

                if (str == fileName)
                    return true;
            }
        }

        return false;
    }

    /// <summary>
    /// <paramref name="targetFile"/>: �������� ������ ���ϸ�.Ȯ����
    /// <paramref name="mainFestUrl"/>: �������� ������ ������ �Ŵ��н�Ʈ ���� Url
    /// <para>
    /// ���� ���¹����� �ؽð��� ������ �� ����մϴ�.
    /// </para>
    /// </summary>
    private IEnumerator GetServerAssetBundleHash(string targetFile, string mainFestUrl)
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(mainFestUrl);
        yield return www.SendWebRequest();

        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
        AssetBundleManifest manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        serverAssetBundleHash = manifest.GetAssetBundleHash(targetFile);

        bundle.Unload(true);

        // Debug.Log("������ [" + targetFile + "] �ؽð�: " + serverAssetBundleHash.ToString());
    }

    /// <summary>
    /// <paramref name="targetFile"/>: ���ÿ��� ������ ���ϸ�.Ȯ����
    /// <paramref name="mainFestUrl"/>: ���ÿ��� ������ ������ �Ŵ��н�Ʈ ���� Url
    /// <para>
    /// ���� ���¹����� �ؽð��� ������ �� ����մϴ�.
    /// </para>
    /// </summary>
    private IEnumerator GetLocalAssetBundleHash(string targetFile, string mainfestPath)
    {
        AssetBundle bundle = AssetBundle.LoadFromFile(mainfestPath);
        AssetBundleManifest manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        yield return null;

        myAssetBundleHash = manifest.GetAssetBundleHash(targetFile);

        bundle.Unload(true);

        // Debug.Log("������ [" + targetFile + "] �ؽð�: " + myAssetBundleHash.ToString());
    }

    /// <summary>
    /// ������ �ִ� ���� ������ ������ ���� ����ҿ� ������ �� ���
    /// </summary>
    private IEnumerator StoreAssetBundle(string url)
    {
        UnityWebRequest www = new UnityWebRequest(url); // GET ������� HTTP ��Ŷ�� �޾ƿ� UnityWebRequest������ ����
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();

        byte[] results = www.downloadHandler.data;

        FileDirectoryCheck(assetBundleDirectory);

        try
        {
            File.WriteAllBytes(assetBundleDirectory + "/" + GetFileNameOnlyFromFilePath(url), results);
        }
        catch (System.Exception e)
        {
            Debug.Log(GetFileNameOnlyFromFilePath(url) + " ���� �ٿ�ε忡 �����Ͽ����ϴ�.");
        }

        yield return StartCoroutine(FillGaugeSmooth(results.Length));
        downloadText.text = GetFileNameOnlyFromFilePath(url) + " �ٿ�ε� �Ϸ�";
    }

    /// <summary>
    /// <para>
    /// �Լ� ��� �������� ����.
    /// </para>
    /// ���� ��ο� �ִ� ���� ������ �ε��Ͽ� [�������̸�, ���ӿ�����Ʈ]�� ��ųʸ� ���·� �����ϴ� �뵵
    /// </summary>
    private Dictionary<string, GameObject> StoreGameObjectFromAssetBundle(string fileName)
    {
        string filePath = assetBundleDirectory + "/" + fileName;

        AssetBundle assetBundle = AssetBundle.LoadFromFile(filePath);

        GameObject[] prefabs = assetBundle.LoadAllAssets<GameObject>();

        Dictionary<string, GameObject> dic = new Dictionary<string, GameObject>();

        for (int i = 0; i < prefabs.Length; i++)
            dic.Add(prefabs[i].name, prefabs[i]);

        return dic;
    }

    /// <summary>
    /// ���ÿ��� ���¹����� �ҷ��� ���� ���¹��� �ν��Ͻ��� �����ϴ� �뵵
    /// </summary>
    /// <param name="fileName">���¹���.Ȯ���</param>
    /// <returns>���¹��� �ν��Ͻ�</returns>
    private AssetBundle KeepAssetBundle(string fileName)
    {
        string filePath = assetBundleDirectory + "/" + fileName;

        AssetBundle assetBundle = AssetBundle.LoadFromFile(filePath);

        return assetBundle;
    }

    /// <summary>
    /// ���̴��� ���������Ͽ� ���¹��� �ε�� ���̴��� ���������ϰ� �մϴ�.
    /// </summary>
    /// <typeparam name="T">Renderer ������Ʈ�� ��ӹ��� ������Ʈ�� ����ϼ���. ��, ParticleSystemRenderer�� ������� �ʴ� ���� �����մϴ�.</typeparam>
    /// <param name="assetbundle">Null�� �ƴ� ���¹����� �־��ּ���.</param>
    private void RefreshShader<T>(AssetBundle assetbundle) where T : UnityEngine.Renderer
    {
    #if UNITY_EDITOR
        GameObject[] all = assetbundle.LoadAllAssets<GameObject>();
        T[] renderer;
        Material[] ms;

        for (int i = 0; i < all.Length; i++)
        {
            // SyntyStudios/Water ���̴��� ���������� �ذ��� �� ��

            renderer = all[i].GetComponentsInChildren<T>();

            for (int j = 0; j < renderer.Length; j++)
            {
                if (renderer[j] != null)
                {
                    ms = renderer[j].sharedMaterials;

                    for (int k = 0; k < ms.Length; k++)
                    {
                        if (ms[k] != null && ms[k].shader.name != "SyntyStudios/Water")
                            ms[k].shader = Shader.Find(ms[k].shader.name);
                    }
                }
            }
        }
    #endif
    }

    /// <summary>
    /// ���̴��� ���������Ͽ� ���¹��� �ε�� ���̴��� ���������ϰ� �մϴ�. (ParticleSystemRenderer ����)
    /// </summary>
    /// <param name="assetbundle">Null�� �ƴ� ���¹����� �־��ּ���.</param>
    private void RefreshShaderFromParticleSystemRenderer(AssetBundle assetbundle)
    {
        GameObject[] all = assetbundle.LoadAllAssets<GameObject>();
        ParticleSystemRenderer renderer;

        for (int i = 0; i < all.Length; i++)
        {
            renderer = all[i].GetComponentInChildren<ParticleSystemRenderer>();

            if (renderer == null)
                return;
            else
                if (renderer.sharedMaterial != null)
                    renderer.sharedMaterial.shader = Shader.Find(renderer.sharedMaterial.shader.name);
                else if (renderer.trailMaterial != null)
                    renderer.trailMaterial.shader = Shader.Find(renderer.trailMaterial.shader.name);
        }
    }

    /// <summary>
    /// �����η� ǥ���� ���ڿ����� "���ϸ�.Ȯ����" �� ���� ���ִ� �뵵�� �Լ�
    /// </summary>
    private string GetFileNameOnlyFromFilePath(string filePath)
    {
        string[] slashed = filePath.Split('/');

        return slashed[slashed.Length - 1];
    }

    private IEnumerator FillGaugeSmooth(float totalLength)
    {
        float goal = downloadSlider.value + totalLength;
        float timer = 0f;
        float finishTime = Random.Range(MIN_LOADING_TIMER, MAX_LOADING_TIMER);

        while (timer < finishTime)
        {
            timer += Time.deltaTime;

            downloadSlider.value = Mathf.Lerp(downloadSlider.value, goal, Time.deltaTime * GAUGE_UP_SPEED);

            yield return null;
        }

        downloadSlider.value = goal;
    }
}
