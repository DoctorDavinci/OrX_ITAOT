using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;

namespace OrX.Kontinuum
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class OrXKontinuum : MonoBehaviour
    {
        public List<string> KontinuumDirectoryList;

        private FtpWebRequest KontinuumRequest = null;
        private FtpWebResponse KontinuumResponse = null;
        private Stream KontinuumStream = null;
        public static OrXKontinuum instance;
        OrXKontinuum KontinuumClient;

        private int _buffer = 2048;

        private string host = null;
        private string user = null;
        private string pass = null;

        public OrXKontinuum(string hostIP, string userName, string password)
        {
            host = hostIP;
            user = userName;
            pass = password;
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            instance = this;
            KontinuumDirectoryList = new List<string>();
        }

        #region Functions

        public void Download(string remoteFile, string localFile)
        {
            try
            {
                KontinuumRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
                KontinuumRequest.Credentials = new NetworkCredential(user, pass);
                KontinuumRequest.UseBinary = true;
                KontinuumRequest.UsePassive = true;
                KontinuumRequest.KeepAlive = true;
                KontinuumRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                KontinuumResponse = (FtpWebResponse)KontinuumRequest.GetResponse();
                KontinuumStream = KontinuumResponse.GetResponseStream();
                FileStream localFileStream = new FileStream(localFile, FileMode.Create);
                byte[] byteBuffer = new byte[_buffer];
                int bytesRead = KontinuumStream.Read(byteBuffer, 0, _buffer);
                try
                {
                    while (bytesRead > 0)
                    {
                        localFileStream.Write(byteBuffer, 0, bytesRead);
                        bytesRead = KontinuumStream.Read(byteBuffer, 0, _buffer);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                localFileStream.Close();
                KontinuumStream.Close();
                KontinuumResponse.Close();
                KontinuumRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }
        public void Upload(string remoteFile, string localFile)
        {
            try
            {
                KontinuumRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
                KontinuumRequest.Credentials = new NetworkCredential(user, pass);
                KontinuumRequest.UseBinary = true;
                KontinuumRequest.UsePassive = true;
                KontinuumRequest.KeepAlive = true;
                KontinuumRequest.Method = WebRequestMethods.Ftp.UploadFile;
                KontinuumStream = KontinuumRequest.GetRequestStream();
                FileStream localFileStream = new FileStream(localFile, FileMode.Create);
                byte[] byteBuffer = new byte[_buffer];
                int bytesSent = localFileStream.Read(byteBuffer, 0, _buffer);
                try
                {
                    while (bytesSent != 0)
                    {
                        KontinuumStream.Write(byteBuffer, 0, bytesSent);
                        bytesSent = localFileStream.Read(byteBuffer, 0, _buffer);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                localFileStream.Close();
                KontinuumStream.Close();
                KontinuumRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }
        public void Delete(string deleteFile)
        {
            try
            {
                KontinuumRequest = (FtpWebRequest)WebRequest.Create(host + "/" + deleteFile);
                KontinuumRequest.Credentials = new NetworkCredential(user, pass);
                KontinuumRequest.UseBinary = true;
                KontinuumRequest.UsePassive = true;
                KontinuumRequest.KeepAlive = true;
                KontinuumRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                KontinuumResponse = (FtpWebResponse)KontinuumRequest.GetResponse();
                KontinuumResponse.Close();
                KontinuumRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }
        public void Rename(string currentFileNameAndPath, string newFileName)
        {
            try
            {
                KontinuumRequest = (FtpWebRequest)WebRequest.Create(host + "/" + currentFileNameAndPath);
                KontinuumRequest.Credentials = new NetworkCredential(user, pass);
                KontinuumRequest.UseBinary = true;
                KontinuumRequest.UsePassive = true;
                KontinuumRequest.KeepAlive = true;
                KontinuumRequest.Method = WebRequestMethods.Ftp.Rename;
                KontinuumRequest.RenameTo = newFileName;
                KontinuumResponse = (FtpWebResponse)KontinuumRequest.GetResponse();
                KontinuumResponse.Close();
                KontinuumRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }
        public void CreateDirectory(string newDirectory)
        {
            try
            {
                KontinuumRequest = (FtpWebRequest)WebRequest.Create(host + "/" + newDirectory);
                KontinuumRequest.Credentials = new NetworkCredential(user, pass);
                KontinuumRequest.UseBinary = true;
                KontinuumRequest.UsePassive = true;
                KontinuumRequest.KeepAlive = true;
                KontinuumRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                KontinuumResponse = (FtpWebResponse)KontinuumRequest.GetResponse();
                KontinuumResponse.Close();
                KontinuumRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }
        public string GetFileCreatedDateTime(string fileName)
        {
            try
            {
                KontinuumRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + fileName);
                KontinuumRequest.Credentials = new NetworkCredential(user, pass);
                KontinuumRequest.UseBinary = true;
                KontinuumRequest.UsePassive = true;
                KontinuumRequest.KeepAlive = true;
                KontinuumRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                KontinuumResponse = (FtpWebResponse)KontinuumRequest.GetResponse();
                KontinuumStream = KontinuumResponse.GetResponseStream();
                StreamReader ftpReader = new StreamReader(KontinuumStream);
                string fileInfo = null;
                try { fileInfo = ftpReader.ReadToEnd(); }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                ftpReader.Close();
                KontinuumStream.Close();
                KontinuumResponse.Close();
                KontinuumRequest = null;
                return fileInfo;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return "";
        }
        public string GetFileSize(string fileName)
        {
            try
            {
                KontinuumRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + fileName);
                KontinuumRequest.Credentials = new NetworkCredential(user, pass);
                KontinuumRequest.UseBinary = true;
                KontinuumRequest.UsePassive = true;
                KontinuumRequest.KeepAlive = true;
                KontinuumRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                KontinuumResponse = (FtpWebResponse)KontinuumRequest.GetResponse();
                KontinuumStream = KontinuumResponse.GetResponseStream();
                StreamReader ftpReader = new StreamReader(KontinuumStream);
                string fileInfo = null;
                try { while (ftpReader.Peek() != -1) { fileInfo = ftpReader.ReadToEnd(); } }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                ftpReader.Close();
                KontinuumStream.Close();
                KontinuumResponse.Close();
                KontinuumRequest = null;
                return fileInfo;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return "";
        }
        public string[] DirectoryListSimple(string directory)
        {
            try
            {
                KontinuumRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + directory);
                KontinuumRequest.Credentials = new NetworkCredential(user, pass);
                KontinuumRequest.UseBinary = true;
                KontinuumRequest.UsePassive = true;
                KontinuumRequest.KeepAlive = true;
                KontinuumRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                KontinuumResponse = (FtpWebResponse)KontinuumRequest.GetResponse();
                KontinuumStream = KontinuumResponse.GetResponseStream();
                StreamReader ftpReader = new StreamReader(KontinuumStream);
                string directoryRaw = null;
                try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                ftpReader.Close();
                KontinuumStream.Close();
                KontinuumResponse.Close();
                KontinuumRequest = null;
                try { string[] directoryList = directoryRaw.Split("|".ToCharArray()); return directoryList; }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return new string[] { "" };
        }
        public string[] DirectoryListDetailed(string directory)
        {
            try
            {
                KontinuumRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + directory);
                KontinuumRequest.Credentials = new NetworkCredential(user, pass);
                KontinuumRequest.UseBinary = true;
                KontinuumRequest.UsePassive = true;
                KontinuumRequest.KeepAlive = true;
                KontinuumRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                KontinuumResponse = (FtpWebResponse)KontinuumRequest.GetResponse();
                KontinuumStream = KontinuumResponse.GetResponseStream();
                StreamReader ftpReader = new StreamReader(KontinuumStream);
                string directoryRaw = null;
                try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                ftpReader.Close();
                KontinuumStream.Close();
                KontinuumResponse.Close();
                KontinuumRequest = null;
                try { string[] directoryList = directoryRaw.Split("|".ToCharArray()); return directoryList; }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return new string[] { "" };
        }

        #endregion

        #region Kontinuum Connect

        // FTP
        public void CreateConnectionFTP(string hostIP, string userName, string password)
        {
            KontinuumClient = new Kontinuum.OrXKontinuum(hostIP, userName, password);
        }
        public void UploadFileFTP(string remoteFile, string localFile)
        {
            Upload(remoteFile, localFile);

        }
        public void DownloadFileFTP(string remoteFile, string localFile)
        {
            Download(remoteFile, localFile);
        }
        public void DeleteFileFTP(string remoteFile)
        {
            Delete(remoteFile);
        }
        public void RenameFileFTP(string remoteFile, string newName)
        {
            Rename(remoteFile, newName);
        }
        public void CreateDirectoryFTP(string remoteDirectory)
        {
            CreateDirectory(remoteDirectory);
        }
        public void GetFileCreatedDateTimeFTP(string remoteFile)
        {
            OrXHoloKron.instance.fileDateTime = GetFileCreatedDateTime(remoteFile);
        }
        public void GetFileSizeFTP(string remoteFile)
        {
            OrXHoloKron.instance.fileSize = GetFileSize(remoteFile);
        }
        public void DirectoryListSimpleFTP(string remoteDirectory)
        {
            KontinuumDirectoryList = new List<string>();
            string[] KontinuumDirectoryListing = DirectoryListDetailed(remoteDirectory);
            for (int i = 0; i < KontinuumDirectoryListing.Count(); i++)
            {
                KontinuumDirectoryList.Add(KontinuumDirectoryListing[i]);
            }
        }
        public void DirectoryListDetailedFTP(string remoteDirectory)
        {
            KontinuumDirectoryList = new List<string>();
            string[] KontinuumDirectoryListing = DirectoryListDetailed(remoteDirectory);
            for (int i = 0; i < KontinuumDirectoryListing.Count(); i++)
            {
                KontinuumDirectoryList.Add(KontinuumDirectoryListing[i]);
            }
        }
        public void DisconnectFTP()
        {
            KontinuumClient = null;
        }

        //WEB
        WebClient fileDownloader;
        bool _downloading = false;
        public void DownloadFile(string _webLoc, string _localSaveLoc)
        {
            _downloading = true;
            string loc = UrlDir.ApplicationRootPath + "GameData/OrX/Kontinuum/" + _localSaveLoc;
            Uri webLoc = new Uri(_webLoc);

            try
            {
                fileDownloader = new WebClient();
                fileDownloader.DownloadFileAsync(webLoc, loc);
                fileDownloader.Dispose();
                OrXHoloKron.instance.connectToKontinuum = false;
                _downloading = false;
                OrXHoloKron.instance.OnScrnMsgUC("Welcome to the Kontinuum " + OrXHoloKron.instance.loginName + " ....");

            }
            catch
            {
                Debug.Log("[OrX Download File - WEB] ==================================================");
                Debug.Log("[OrX Download File - WEB] ===== Unable to establish a connection ....  =====");
                Debug.Log("[OrX Download File - WEB] ==================================================");
                Debug.Log("[OrX Download File - WEB] ===== FILE LINK: " + _webLoc + " =====");

                OrXHoloKron.instance.OnScrnMsgUC("Unable to establish a connection ....");
                OrXHoloKron.instance.OnScrnMsgUC("Check if the file link is valid ....");
            }
            _downloading = false;
        }

        #endregion

    }
}
