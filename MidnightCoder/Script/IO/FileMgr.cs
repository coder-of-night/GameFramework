using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;
//using ICSharpCode.SharpZipLib.Zip;
//using ICSharpCode.SharpZipLib.Core;


namespace MidnightCoder.Game
{
    public class FileMgr : TSingleton<FileMgr>
    {
        private List<string> m_SearchDirList = new List<string>();
        private string m_StreamingAssetsPath;

        public override void OnSingletonInit()
        {
            try
            {
                m_SearchDirList.Add(FilePath.persistentDataPath4Res);
                m_StreamingAssetsPath = FilePath.streamingAssetsPath;
            }
            catch (System.Exception e)
            {

                Debug.LogError("FileMgr OnSingletonInit==》" + e.Message + "/n" + e.StackTrace);
            }

        }

        //
        public byte[] ReadSync(string fileRelativePath)
        {
            string absoluteFilePath = FindFilePathInExteral(fileRelativePath);
            if (!string.IsNullOrEmpty(absoluteFilePath))
            {
                return ReadSyncExtenal(fileRelativePath);
            }

            return ReadSyncInternal(fileRelativePath);
        }
        //
        private byte[] ReadSyncExtenal(string fileRelativePath)
        {
            string absoluteFilePath = FindFilePathInExteral(fileRelativePath);

            if (!string.IsNullOrEmpty(absoluteFilePath))
            {
                FileInfo fileInfo = new FileInfo(absoluteFilePath);
                return ReadFile(fileInfo);
            }

            return null;
        }
        //
        private byte[] ReadSyncInternal(string fileRelativePath)
        {
            string absoluteFilePath = FindFilePathInternal(fileRelativePath);

            if (!string.IsNullOrEmpty(absoluteFilePath))
            {
                FileInfo fileInfo = new FileInfo(absoluteFilePath);
                return ReadFile(fileInfo);
            }

            return null;
        }


        //
        private byte[] ReadFile(FileInfo fileInfo)
        {
            using (FileStream fileStream = fileInfo.OpenRead())
            {
                byte[] byteData = new byte[fileStream.Length];
                fileStream.Read(byteData, 0, byteData.Length);
                return byteData;
            }
        }
        //
        private string FindFilePathInExteral(string file)
        {
            string filePath;
            for (int i = 0; i < m_SearchDirList.Count; ++i)
            {
                filePath = string.Format("{0}/{1}", m_SearchDirList[i], file);
                if (File.Exists(filePath))
                {
                    return filePath;
                }
            }

            return string.Empty;
        }

        //
        private string FindFilePathInternal(string file)
        {
            string filePath = string.Format("{0}{1}", m_StreamingAssetsPath, file);

            if (File.Exists(filePath))
            {
                return filePath;
            }

            return null;
        }

    }
}
