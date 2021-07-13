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

    // 슬라이더 페이드 아웃용도
    public Image sliderLoadingBar;
    public Image sliderLoadingFrame;
    public Image sliderLoadingFrameDecoTop;
    public Image sliderLoadingFrameDecoBot;
    public Image sliderLoadingBackBar;
    // public TMP_Text loadingText

    // 로딩 페이크 용도. 파일이 많아지면 필요없어
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
        
        if (downloadSlider.value == downloadSlider.maxValue)
        {
            downloadText.text = "게임 준비 완료";
            StartCoroutine(DownloadCompletedFadeOut());
        }
        else
            downloadText.text = "서버에서 받은 파일이 잘 못 되었습니다. 운영자에게 문의해주세요.";

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

        // 최신버젼인 파일과 패치를 다 받고 난 뒤 모자란 값을 채우기 위해 사용
        loadingText.text = "Loading..." + string.Format("{0:0}%", 100);
    }

    private IEnumerator PatchAssetBundle(string targetFile, string maniFestName)
    {
        string serverMainfestUrl = serverAssetBundleDirectory + "/" + maniFestName;
        string serverTargetFileUrl = serverAssetBundleDirectory + "/" + targetFile;
        string myManiFestFilePath = assetBundleDirectory + "/" + maniFestName;

        downloadText.text = targetFile + " 검사 중..";

        // 1. 로컬저장소의 디렉토리가 잘 있는지 확인
        FileDirectoryCheck(assetBundleDirectory);

        // 2. 내 로컬저장소에 있는 타겟 에셋번들의 해시값을 얻어온다.
        if (IsFileInMyAssetBundleDirectory(maniFestName))
            yield return StartCoroutine(GetLocalAssetBundleHash(targetFile, myManiFestFilePath));

        // 2-1. 파일이 없으면 서버에서 전부 다 받아오면 끝.
        else
        {
            downloadText.text = targetFile + " 다운로드 중..";
            yield return StartCoroutine(StoreAssetBundle(serverMainfestUrl));
            downloadText.text = targetFile + " 다운로드 중..";
            yield return StartCoroutine(StoreAssetBundle(serverTargetFileUrl));
            yield return completeWaitTime;
            yield break;
        }

        // 4. 기존 파일과 비교하기 위해 서버에 있는 타겟 에셋번들의 해시값을 가져옴
        yield return StartCoroutine(GetServerAssetBundleHash(targetFile, serverMainfestUrl));

        // 5. 해시값을 비교하여 같으면 바로 탈출
        if (myAssetBundleHash.ToString() == serverAssetBundleHash.ToString())
        {
            yield return FillGaugeSmooth(downloadSlider.maxValue / DOWNLOAD_FILES);
            downloadText.text = targetFile + " 검사 완료.";
            yield return checkWaitTime;
            yield break;
        }

        // 5-1. 해시값이 다르면 서버에서 전부 다 받아오면 끝.
        else
        {
            downloadText.text = targetFile + " 다운로드 중..";
            yield return StartCoroutine(StoreAssetBundle(serverMainfestUrl));
            downloadText.text = targetFile + " 다운로드 중..";
            yield return StartCoroutine(StoreAssetBundle(serverTargetFileUrl));
            yield return completeWaitTime;
        }
    }

    /// <summary>
    /// 로컬 저장소에 디렉토리가 있는지 없는지 체크할 때 사용
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

        // \\ 뒤부터 파일명.확장자를 얻을 수 있음
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
    /// <paramref name="targetFile"/>: 서버에서 가져올 파일명.확장자
    /// <paramref name="mainFestUrl"/>: 서버에서 가져올 파일의 매니패스트 파일 Url
    /// <para>
    /// 서버 에셋번들의 해시값을 가져올 때 사용합니다.
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

        // Debug.Log("서버의 [" + targetFile + "] 해시값: " + serverAssetBundleHash.ToString());
    }

    /// <summary>
    /// <paramref name="targetFile"/>: 로컬에서 가져올 파일명.확장자
    /// <paramref name="mainFestUrl"/>: 로컬에서 가져올 파일의 매니패스트 파일 Url
    /// <para>
    /// 로컬 에셋번들의 해시값을 가져올 때 사용합니다.
    /// </para>
    /// </summary>
    private IEnumerator GetLocalAssetBundleHash(string targetFile, string mainfestPath)
    {
        AssetBundle bundle = AssetBundle.LoadFromFile(mainfestPath);
        AssetBundleManifest manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        yield return null;

        myAssetBundleHash = manifest.GetAssetBundleHash(targetFile);

        bundle.Unload(true);

        // Debug.Log("로컬의 [" + targetFile + "] 해시값: " + myAssetBundleHash.ToString());
    }

    /// <summary>
    /// 서버에 있는 에셋 번들을 가져와 로컬 저장소에 저장할 때 사용
    /// </summary>
    private IEnumerator StoreAssetBundle(string url)
    {
        UnityWebRequest www = new UnityWebRequest(url); // GET 방식으로 HTTP 패킷을 받아와 UnityWebRequest형으로 저장
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
            Debug.Log(GetFileNameOnlyFromFilePath(url) + " 파일 다운로드에 실패하였습니다.");
        }

        yield return StartCoroutine(FillGaugeSmooth(results.Length));
        downloadText.text = GetFileNameOnlyFromFilePath(url) + " 다운로드 완료";
    }

    /// <summary>
    /// <para>
    /// 함수 사용 권장하지 않음.
    /// </para>
    /// 로컬 경로에 있는 에셋 번들을 로드하여 [프리팹이름, 게임오브젝트]의 딕셔너리 형태로 보관하는 용도
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
    /// 로컬에서 에셋번들을 불러와 정적 에셋번들 인스턴스에 보관하는 용도
    /// </summary>
    /// <param name="fileName">에셋번들.확장명</param>
    /// <returns>에셋번들 인스턴스</returns>
    private AssetBundle KeepAssetBundle(string fileName)
    {
        string filePath = assetBundleDirectory + "/" + fileName;

        AssetBundle assetBundle = AssetBundle.LoadFromFile(filePath);

        return assetBundle;
    }

    /// <summary>
    /// 쉐이더를 리프레시하여 에셋번들 로드시 쉐이더가 정상적용하게 합니다.
    /// </summary>
    /// <typeparam name="T">Renderer 컴포넌트를 상속받은 컴포넌트만 사용하세요. 단, ParticleSystemRenderer는 사용하지 않는 것을 권장합니다.</typeparam>
    /// <param name="assetbundle">Null이 아닌 에셋번들을 넣어주세요.</param>
    private void RefreshShader<T>(AssetBundle assetbundle) where T : UnityEngine.Renderer
    {
#if UNITY_EDITOR
        GameObject[] all = assetbundle.LoadAllAssets<GameObject>();
        T[] renderer;
        Material[] ms;

        for (int i = 0; i < all.Length; i++)
        {
            // SyntyStudios/Water 쉐이더는 리프레쉬로 해결이 안 됨

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
    /// 쉐이더를 리프레시하여 에셋번들 로드시 쉐이더가 정상적용하게 합니다. (ParticleSystemRenderer 전용)
    /// </summary>
    /// <param name="assetbundle">Null이 아닌 에셋번들을 넣어주세요.</param>
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
    /// 절대경로로 표현된 문자열에서 "파일명.확장자" 만 빼게 해주는 용도의 함수
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
