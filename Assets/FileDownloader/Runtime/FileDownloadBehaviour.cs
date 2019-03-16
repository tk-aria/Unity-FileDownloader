using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

namespace AssetOperation
{

    [AddComponentMenu("AssetOperation/FileDownloadBehaviour")]
    public class FileDownloadBehaviour : MonoBehaviour
    {


        [SerializeField]
        private FileDownloader fileDownloader;

        [SerializeField]
        private string downloadUrl;

#if UNITY_EDITOR

        void Start()
        {

            fileDownloader.ConnectDownloadCompletedHandler = OnCompletedDownload;
            fileDownloader.Download(downloadUrl);
        }

        protected virtual void OnCompletedDownload(FileDownloader.DownloadInfo downloadInfo) 
        {
            AssetDatabase.ImportPackage(downloadInfo.storeFilePath, true);
        }

#endif // UNITY_EDITOR
    }
}
