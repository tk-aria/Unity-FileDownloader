using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

namespace AssetOperation
{


    [Serializable][CreateAssetMenu(menuName = "ScriptableObject/AssetOperation/FileDownloader")]
    public class FileDownloader : ScriptableObject
    {
#if UNITY_EDITOR
        #region Event
        public delegate void DownloadCompletedHandler(DownloadInfo downloadInfo);
        #endregion // Event End.

        #region Type
        [Serializable]
        public class DownloadInfo
        {
            public string downloadUrl;
            public string storeFilePath;
            public bool isSuccessDownload;
        }

        [Serializable]
        public class ProgressBar
        {
            public string MainCaprion = "Hold on";
            public string Message = "Downloading ..";
        }

        #endregion // Type End.

        #region Field

        [SerializeField]
        private string storeFolderPath = "ProjectSupports/Download/";

        [SerializeField] 
        private ProgressBar progressBar = new ProgressBar();

        #endregion // Field End.

        private UnityEngine.WWW webRequest;
        private string archiveFileName;
        private DownloadInfo downloadInfo = new DownloadInfo();

        private DownloadCompletedHandler downloadCompletedHandler = null;
        public DownloadCompletedHandler ConnectDownloadCompletedHandler { set { downloadCompletedHandler += value; } }

        private bool working = false;
        public bool IsDownloading{ get { return working; } }


        public virtual void Download(string url)
        {
            working = true;
            archiveFileName = Path.GetFileName(downloadInfo.downloadUrl = url);

            webRequest = new WWW(downloadInfo.downloadUrl);
            EditorApplication.update += OnDownload;
        }

        protected virtual void OnDownload()
        {
        
            EditorUtility.DisplayProgressBar(
                progressBar.MainCaprion, 
                progressBar.Message + archiveFileName + "(" + webRequest.progress*100 + "%)", 
                webRequest.progress);

            if (!string.IsNullOrEmpty(webRequest.error))
            {
                Debug.LogError(webRequest.error);
                Downloaded(false);
            }
            else if (webRequest.isDone)
            {
                // Save as Archive file.
                Directory.CreateDirectory(storeFolderPath);
                downloadInfo.storeFilePath = storeFolderPath + archiveFileName;
                System.IO.File.WriteAllBytes(downloadInfo.storeFilePath, webRequest.bytes);
                Downloaded(true);
            }
        }

        protected virtual void Downloaded(bool isSuccess)
        {

            // ダウンロード結果の格納.
            downloadInfo.isSuccessDownload = isSuccess;

            webRequest.Dispose();
            webRequest = null;
            EditorUtility.ClearProgressBar();
            EditorApplication.update -= OnDownload;

            if(downloadCompletedHandler != null){
                downloadCompletedHandler.Invoke(downloadInfo);
            }
            working = false;
        }


        public static FileDownloader Create()
        {
            return ScriptableObject.CreateInstance<FileDownloader>();
        }

        public static FileDownloader Load(string key)
        {
            return Resources.Load<FileDownloader>("FileDownloader/" + key);
        }

        public static FileDownloader Instantiate(string key)
        {
            return UnityEngine.Object.Instantiate<FileDownloader>(Load(key));
        }

#endif // UNITY_EDITOR
    }

}
