using UnityEngine;
using System.Collections;
using UnityEngine.Events;


namespace GameStartStudio
{


    // 1.AssetBundleManger
    // 2.ObjectManager
    // 3.ResourceManager
    // 4.Pool
    public class ResourceManager : MonoSingleton<ResourceManager>
    {
        

    // 同步资源加载
    public T Load<T>(string path) where T : Object // object
    {
        T resource = Resources.Load<T>(path);
        return resource is GameObject ? Instantiate(resource) : resource;
    }

    public void LoadAsync<T>(string path, UnityAction<T> callback) where T : Object
    {
        // 开启异步加载的携程
        StartCoroutine(LoadAsyncInternal(path, callback));
    }

    // 异步资源加载
    private IEnumerator LoadAsyncInternal<T>(string path, UnityAction<T> callback) where T : Object
    {
        ResourceRequest resourceRequest = Resources.LoadAsync<T>(path);
        yield return resourceRequest;
        if (resourceRequest.asset is GameObject)
        {
            callback(Instantiate(resourceRequest.asset) as T);
        }

        else
        {
            callback(resourceRequest.asset as T);
        }
    }

    //public PopupText popupText;

}
}