using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;


namespace AssetOperation
{

    public class FileDownloadManager : CodeTemplates.UnitySerializeWindow
    {

        private const string editorConfAssetName = "Assets/FileDownloadManagerEditorConf.asset";

        [SerializeField][Header("FileDownloadManager Settings")][Space(16)]
        private FileDownloader fileDownloader;

        [SerializeField]
        private string downloadUrl;


        [MenuItem("Window/FileDownloadManager")]
        static void Create()
        {

            var editorConf = AssetDatabase.LoadAssetAtPath<EditorConf>(editorConfAssetName);
            var instance = ShowDisplay(typeof(FileDownloadManager), editorConf);
        
        }

        public override void OnOpen()
        {

            fileDownloader.ConnectDownloadCompletedHandler = OnCompletedDownload;
            editorConf.footerButtons[0].onClickHandler += () =>
                {
                    fileDownloader.Download(downloadUrl);
                };
        }

        public override void OnClose()
        {

        }

        public override void OnUpdate()
        {
         
        }

        public override void OnRender()
        {

        }

        public IEnumerator Download()
        {
            fileDownloader.Download(downloadUrl);
            while (fileDownloader.IsDownloading)
            {
                yield return null;
            }

            Debug.Log("DownloadEnd");
        }

        protected virtual void OnCompletedDownload(FileDownloader.DownloadInfo downloadInfo)
        {
            AssetDatabase.ImportPackage(downloadInfo.storeFilePath, true); 
        }


    }
}
