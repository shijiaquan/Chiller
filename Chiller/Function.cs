using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.IO.Ports;
using System.Threading;
using System.Text.RegularExpressions;
using System.Management;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Collections;

namespace Chiller
{
    /// <summary>
    /// Function 的摘要说明。
    /// <para>主要包括：</para> 
    /// <para>1.ASE的XML文件读写</para> 
    /// <para>2.支持上传与下载文件</para> 
    /// <para>3.INI文件的读写</para> 
    /// <para>4.注册表的操作</para> 
    /// 该操作需要管理员权限，需进行以下设定
    /// <para>1.进入项目属性页-选择“安全性”栏目-将“启用ClickOnce安全设置”勾选上</para> 
    /// <para>2.在Properties目录下就自动生成了app.manifest文件，打开该文件，将requestedPrivileges节点的requestedExecutionLevel的level的值修改为 highestAvailable即可</para> 
    /// <para>3.进入项目属性页-选择“安全性”栏目-将“启用ClickOnce安全设置”取消勾选</para> 
    /// </summary>
    public class Function
    {
        /// <summary>
        /// 上传与下载文件操作类
        /// <para>主要包括：FTP方式/共享方式</para> 
        /// <para>1.FTP支持上传、下载、添加文件夹、删除文件夹、删除文件、重复名</para> 
        /// <para>2.共享方式支持上传、下载、连接服务器</para> 
        /// </summary>
        public class UDFile
        {
            //导入判断网络是否连接的 .dll  
            [DllImport("wininet.dll", EntryPoint = "InternetGetConnectedState")]
            //判断网络状况的方法,返回值true为连接，false为未连接  
            public extern static bool InternetGetConnectedState(out int conState, int reder);

            public static class FTP
            {

                private static FtpWebRequest GetRequest(string URI, string username, string password)
                {
                    //根据服务器信息FtpWebRequest创建类的对象
                    FtpWebRequest result = (FtpWebRequest)FtpWebRequest.Create(URI);
                    //提供身份验证信息
                    result.Credentials = new System.Net.NetworkCredential(username, password);
                    //设置请求完成之后是否保持到FTP服务器的控制连接，默认值为true
                    result.KeepAlive = false;
                    return result;
                }

                private static string GetStringResponse(FtpWebRequest ftp)
                {
                    //Get the result, streaming to a string
                    string result = "";
                    using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
                    {
                        long size = response.ContentLength;
                        using (Stream datastream = response.GetResponseStream())
                        {
                            using (StreamReader sr = new StreamReader(datastream, System.Text.Encoding.Default))
                            {
                                result = sr.ReadToEnd();
                                sr.Close();
                            }
                        }

                        response.Close();
                    }

                    return result;
                }

                /// <summary>
                /// 测试FTP是否可以连接成功
                /// </summary>
                /// <returns>true 连接成功，false 连接失败 </returns> 
                public static bool CheckServer(string ftpHostIP, string username, string password)
                {
                    ftpHostIP = ftpHostIP.Replace('\\', '/');

                    if (ftpHostIP.Substring(ftpHostIP.Length - 1, 1) != "/")
                    {
                        ftpHostIP += "/";
                    }
                    if (ftpHostIP.Substring(0, 2) == "//")
                    {
                        ftpHostIP = ftpHostIP.Substring(2, ftpHostIP.Length);
                    }
                    FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpHostIP));
                    req.Credentials = new NetworkCredential(username, password);
                    req.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;

                    req.KeepAlive = false;

                    try
                    {
                        req.GetResponse();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                /// <summary>
                /// 上传文件
                /// </summary>
                /// <param name="ftpHostIP">ftp地址</param>
                /// <param name="fileinfo">需要上传的文件</param>
                /// <param name="username">ftp用户名</param>
                /// <param name="password">ftp密码</param>
                public static bool Upload(string ftpHostIP, FileInfo UploadFile, string username, string password)
                {
                    ftpHostIP = ftpHostIP.Replace('\\', '/');

                    if (ftpHostIP.Substring(ftpHostIP.Length - 1, 1) != "/")
                    {
                        ftpHostIP += "/";
                    }
                    if (ftpHostIP.Substring(0, 2) == "//")
                    {
                        ftpHostIP = ftpHostIP.Substring(2, ftpHostIP.Length);
                    }

                    bool Flag_State = false;

                    if (UploadFile.Exists == false)
                    {
                        Flag_State = false;
                        return Flag_State;
                    }

                    string target = Guid.NewGuid().ToString();  //使用临时文件名
                    string URI = "FTP://" + ftpHostIP + "/" + target;
                    URI = URI.Replace('\\', '/');
                    try
                    {
                        ///WebClient webcl = new WebClient();
                        System.Net.FtpWebRequest ftp = GetRequest(URI, username, password);

                        //设置FTP命令 设置所要执行的FTP命令，
                        //ftp.Method = System.Net.WebRequestMethods.Ftp.ListDirectoryDetails;//假设此处为显示指定路径下的文件列表
                        ftp.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
                        //指定文件传输的数据类型
                        ftp.UseBinary = true;
                        ftp.UsePassive = true;

                        //告诉ftp文件大小
                        ftp.ContentLength = UploadFile.Length;
                        //缓冲大小设置为2KB
                        const int BufferSize = 2048;
                        byte[] content = new byte[BufferSize - 1 + 1];
                        int dataRead;

                        //打开一个文件流 (System.IO.FileStream) 去读上传的文件
                        using (FileStream fs = UploadFile.OpenRead())
                        {
                            try
                            {
                                //把上传的文件写入流
                                using (Stream rs = ftp.GetRequestStream())
                                {
                                    do
                                    {
                                        //每次读文件流的2KB
                                        dataRead = fs.Read(content, 0, BufferSize);
                                        rs.Write(content, 0, dataRead);
                                    } while (!(dataRead < BufferSize));
                                    rs.Close();
                                }

                                ftp = null;
                                //设置FTP命令
                                ftp = GetRequest(URI, username, password);
                                ftp.Method = System.Net.WebRequestMethods.Ftp.Rename; //改名
                                ftp.RenameTo = UploadFile.Name;

                                try
                                {
                                    ftp.GetResponse();
                                    Flag_State = true;
                                }
                                catch (Exception ex)
                                {
                                    Ini.WriteException(ex);
                                    Flag_State = false;
                                    ftp = GetRequest(URI, username, password);
                                    ftp.Method = System.Net.WebRequestMethods.Ftp.DeleteFile; //删除
                                    ftp.GetResponse();
                                }
                                finally
                                {
                                    //fileinfo.Delete();
                                }
                                // 可以记录一个日志  "上传" + fileinfo.FullName + "上传到" + "FTP://" + hostname + "/" + targetDir + "/" + fileinfo.Name + "成功." );
                                ftp = null;
                            }
                            catch (Exception ex)
                            {
                                Ini.WriteException(ex);
                                Flag_State = false;
                            }
                            finally
                            {
                                fs.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Ini.WriteException(ex);
                        Flag_State = false;
                    }
                    return Flag_State;

                }

                /// <summary>
                /// 下载文件
                /// </summary>
                /// <param name="ftpHostIP">ftp地址</param>
                /// <param name="localDir">下载至本地路径</param>
                /// <param name="FtpDir">ftp目标文件路径</param>
                /// <param name="FtpFile">从ftp要下载的文件名</param>
                /// <param name="username">ftp用户名</param>
                /// <param name="password">ftp密码</param>
                public static void Download(string ftpHostIP, string localDir, string FtpDir, string FtpFile, string username, string password)
                {
                    ftpHostIP = ftpHostIP.Replace('\\', '/');

                    if (ftpHostIP.Substring(ftpHostIP.Length - 1, 1) != "/")
                    {
                        ftpHostIP += "/";
                    }
                    if (ftpHostIP.Substring(0, 2) == "//")
                    {
                        ftpHostIP = ftpHostIP.Substring(2, ftpHostIP.Length);
                    }

                    string URI = "FTP://" + ftpHostIP + FtpDir + "/" + FtpFile;
                    URI = URI.Replace('\\', '/');
                    string tmpname = Guid.NewGuid().ToString();
                    string localfile = localDir + "\\" + tmpname;

                    System.Net.FtpWebRequest ftp = GetRequest(URI, username, password);
                    ftp.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
                    ftp.UseBinary = true;
                    ftp.UsePassive = false;

                    using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            //loop to read & write to file
                            using (FileStream fs = new FileStream(localfile, FileMode.CreateNew))
                            {
                                try
                                {
                                    byte[] buffer = new byte[2048];
                                    int read = 0;
                                    do
                                    {
                                        read = responseStream.Read(buffer, 0, buffer.Length);
                                        fs.Write(buffer, 0, read);
                                    } while (!(read == 0));
                                    responseStream.Close();
                                    fs.Flush();
                                    fs.Close();
                                }
                                catch (Exception ex)
                                {
                                    Ini.WriteException(ex);
                                    //catch error and delete file only partially downloaded
                                    fs.Close();
                                    //delete target file as it's incomplete
                                    File.Delete(localfile);
                                }
                            }

                            responseStream.Close();
                        }

                        response.Close();
                    }

                    try
                    {
                        File.Delete(localDir + @"\" + FtpFile);
                        File.Move(localfile, localDir + @"\" + FtpFile);

                        ftp = null;
                        ftp = GetRequest(URI, username, password);
                        ftp.Method = System.Net.WebRequestMethods.Ftp.DeleteFile;
                        ftp.GetResponse();

                    }
                    catch (Exception ex)
                    {
                        Ini.WriteException(ex);
                        File.Delete(localfile);
                        throw ex;
                    }

                    // 记录日志 "从" + URI.ToString() + "下载到" + localDir + @"\" + FtpFile + "成功." );
                    ftp = null;
                }

                /// <summary>
                /// 搜索远程文件
                /// </summary>
                /// <param name="ftpHostIP">服务器地址</param>
                /// <param name="targetDir">服务器地址下的目录</param>
                /// <param name="username">ftp用户名</param>
                /// <param name="password">ftp密码</param>
                /// <param name="SearchPattern">搜索模式</param>
                /// <returns></returns>
                public static List<string> ListDirectory(string ftpHostIP, string targetDir, string username, string password, string SearchPattern)
                {
                    List<string> result = new List<string>();
                    try
                    {
                        ftpHostIP = ftpHostIP.Replace('\\', '/');

                        if (ftpHostIP.Substring(ftpHostIP.Length - 1, 1) != "/")
                        {
                            ftpHostIP += "/";
                        }
                        if (ftpHostIP.Substring(0, 2) == "//")
                        {
                            ftpHostIP = ftpHostIP.Substring(2, ftpHostIP.Length);
                        }

                        string URI = "FTP://" + ftpHostIP + "/" + targetDir + "/" + SearchPattern;
                        URI = URI.Replace('\\', '/');
                        System.Net.FtpWebRequest ftp = GetRequest(URI, username, password);
                        ftp.Method = System.Net.WebRequestMethods.Ftp.ListDirectory;
                        ftp.UsePassive = true;
                        ftp.UseBinary = true;

                        string str = GetStringResponse(ftp);
                        str = str.Replace("\r\n", "\r").TrimEnd('\r');
                        str = str.Replace("\n", "\r");
                        if (str != string.Empty)
                            result.AddRange(str.Split('\r'));

                        return result;
                    }
                    catch (Exception ex)
                    {
                        Ini.WriteException(ex);
                    }
                    return null;
                }

                /// </summary>
                /// 在ftp服务器上创建目录
                /// </summary>
                /// <param name="ftpHostIP">ftp地址</param>
                /// <param name="dirName">创建的目录名称</param>
                /// <param name="username">ftp用户名</param>
                /// <param name="password">ftp密码</param>
                public static void MakeDir(string ftpHostIP, string dirName, string username, string password)
                {
                    try
                    {
                        ftpHostIP = ftpHostIP.Replace('\\', '/');

                        if (ftpHostIP.Substring(ftpHostIP.Length - 1, 1) != "/")
                        {
                            ftpHostIP += "/";
                        }
                        if (ftpHostIP.Substring(0, 2) == "//")
                        {
                            ftpHostIP = ftpHostIP.Substring(2, ftpHostIP.Length);
                        }

                        string URI = "ftp://" + ftpHostIP + "/" + dirName;

                        URI = URI.Replace('\\', '/');

                        System.Net.FtpWebRequest ftp = GetRequest(URI, username, password);
                        ftp.Method = WebRequestMethods.Ftp.MakeDirectory;

                        FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                        response.Close();
                    }
                    catch (Exception ex)
                    {
                        Ini.WriteException(ex);
                    }
                }

                /// <summary>
                /// 删除目录
                /// </summary>
                /// <param name="ftpHostIP">ftp地址</param>
                /// <param name="dirName">创建的目录名称</param>
                /// <param name="username">用户名</param>
                /// <param name="password">密码</param>
                public static void DelDir(string ftpHostIP, string dirName, string username, string password)
                {
                    try
                    {
                        ftpHostIP = ftpHostIP.Replace('\\', '/');

                        if (ftpHostIP.Substring(ftpHostIP.Length - 1, 1) != "/")
                        {
                            ftpHostIP += "/";
                        }
                        if (ftpHostIP.Substring(0, 2) == "//")
                        {
                            ftpHostIP = ftpHostIP.Substring(2, ftpHostIP.Length);
                        }

                        string URI = "ftp://" + ftpHostIP + "/" + dirName;
                        URI = URI.Replace('\\', '/');
                        System.Net.FtpWebRequest ftp = GetRequest(URI, username, password);
                        ftp.Method = WebRequestMethods.Ftp.RemoveDirectory;
                        FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                        response.Close();
                    }
                    catch (Exception ex)
                    {
                        Ini.WriteException(ex);
                    }
                }

                /// <summary>
                /// 文件重命名
                /// </summary>
                /// <param name="ftpHostIP">ftp地址</param>
                /// <param name="currentFilename">当前目录名称</param>
                /// <param name="newFilename">重命名目录名称</param>
                /// <param name="username">用户名</param>
                /// <param name="password">密码</param>
                public static void Rename(string ftpHostIP, string currentFilename, string newFilename, string username, string password)
                {
                    try
                    {
                        ftpHostIP = ftpHostIP.Replace('\\', '/');

                        if (ftpHostIP.Substring(ftpHostIP.Length - 1, 1) != "/")
                        {
                            ftpHostIP += "/";
                        }
                        if (ftpHostIP.Substring(0, 2) == "//")
                        {
                            ftpHostIP = ftpHostIP.Substring(2, ftpHostIP.Length);
                        }

                        FileInfo fileInf = new FileInfo(currentFilename);
                        string URI = "ftp://" + ftpHostIP + "/" + fileInf.Name;
                        URI = URI.Replace('\\', '/');
                        System.Net.FtpWebRequest ftp = GetRequest(URI, username, password);
                        ftp.Method = WebRequestMethods.Ftp.Rename;

                        ftp.RenameTo = newFilename;
                        FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();

                        response.Close();
                    }
                    catch (Exception ex)
                    {
                        Ini.WriteException(ex);
                    }
                }

                /// <summary>
                /// 判断ftp服务器上该目录是否存在
                /// </summary>
                /// <param name="ftpHostIP"> </param>
                /// <param name="dirName"> </param>
                /// <param name="username"> </param>
                /// <param name="password"> </param>
                /// <returns></returns>
                public static bool FtpIsExistsFile(string ftpHostIP, string dirName, string username, string password)
                {
                    bool flag = true;
                    try
                    {
                        ftpHostIP = ftpHostIP.Replace('\\', '/');

                        if (ftpHostIP.Substring(ftpHostIP.Length - 1, 1) != "/")
                        {
                            ftpHostIP += "/";
                        }
                        if (ftpHostIP.Substring(0, 2) == "//")
                        {
                            ftpHostIP = ftpHostIP.Substring(2, ftpHostIP.Length);
                        }

                        string URI = "ftp:" + ftpHostIP + "/" + dirName;
                        URI = URI.Replace('\\', '/');

                        System.Net.FtpWebRequest ftp = GetRequest(URI, username, password);
                        ftp.Method = WebRequestMethods.Ftp.ListDirectory;

                        FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                        response.Close();
                    }
                    catch (Exception ex)
                    {
                        Ini.WriteException(ex);
                        flag = false;
                    }
                    return flag;
                }
            }
            public static class Share
            {
                //连接服务器
                /// <summary>  
                /// 连接远程共享文件夹  
                /// </summary>  
                /// <param name="UrlPath">远程共享文件夹的路径</param>  
                /// <param name="UserName">用户名</param>  
                /// <param name="PassWord">密码</param>  
                /// <returns></returns>  
                public static bool Connect(string UrlPath, string UserName, string PassWord)
                {
                    bool Flag_State = false;
                    Process proc = new Process();
                    try
                    {
                        proc.StartInfo.FileName = "cmd.exe";
                        proc.StartInfo.UseShellExecute = false;
                        proc.StartInfo.RedirectStandardInput = true;
                        proc.StartInfo.RedirectStandardOutput = true;
                        proc.StartInfo.RedirectStandardError = true;
                        proc.StartInfo.CreateNoWindow = true;
                        proc.Start();
                        string dosLine = @"net use " + UrlPath + " /User:" + UserName + " " + PassWord + " /PERSISTENT:YES";
                        proc.StandardInput.WriteLine(dosLine);
                        proc.StandardInput.WriteLine("exit");
                        while (!proc.HasExited)
                        {
                            proc.WaitForExit(1000);
                        }
                        string errormsg = proc.StandardError.ReadToEnd();
                        proc.StandardError.Close();
                        if (string.IsNullOrEmpty(errormsg))
                        {
                            Flag_State = true;
                        }
                        else
                        {
                            Flag_State = false;
                            throw new Exception(errormsg);
                        }
                    }
                    catch (Exception ex)
                    {
                        Ini.WriteException(ex);
                        Flag_State = false;
                    }
                    finally
                    {
                        proc.Close();
                        proc.Dispose();
                    }
                    return Flag_State;
                }

                //第一种上传和下载的方式
                /// <summary>  
                /// 将本地文件上传到远程服务器共享目录，不可更改扩展名  
                /// </summary>  
                /// <param name="FileNamePath">本地文件的绝对路径，包含扩展名</param>  
                /// <param name="UrlPath">远程服务器共享文件路径，包含文件扩展名</param>  
                /// <returns></returns>  
                public static bool UpLoad(string FileNamePath, string UrlNamePath)
                {
                    bool Flag_State = false;
                    string newFileName = FileNamePath.Substring(FileNamePath.LastIndexOf(@"\") + 1);//取文件名称

                    if (UrlNamePath.EndsWith(@"\") == false)
                    {
                        UrlNamePath = UrlNamePath + @"\";
                    }
                    UrlNamePath = UrlNamePath + newFileName;

                    WebClient myWebClient = new WebClient();

                    NetworkCredential cread = new NetworkCredential();

                    myWebClient.Credentials = cread;
                    FileStream fs = new FileStream(FileNamePath, FileMode.Open, FileAccess.Read);
                    BinaryReader r = new BinaryReader(fs);

                    try
                    {
                        byte[] postArray = r.ReadBytes((int)fs.Length);
                        Stream postStream = myWebClient.OpenWrite(UrlNamePath);
                        if (postStream.CanWrite)
                        {
                            postStream.Write(postArray, 0, postArray.Length);
                            Flag_State = true;
                        }
                        else
                        {
                            Flag_State = false;
                        }

                        postStream.Close();
                    }
                    catch (Exception ex)
                    {
                        Ini.WriteException(ex);
                        Flag_State = false;
                    }
                    return Flag_State;
                }
                /// <summary>  
                /// 从远程服务器下载文件到本地  
                /// </summary>  
                /// <param name="UrlNamePath">远程服务器路径（共享文件夹路径）</param>  
                /// <param name="FileNamePath">下载到本地后的文件路径，包含文件的扩展名</param>  
                /// <returns></returns>  
                public static bool DownLoad(string UrlNamePath, string FileNamePath)
                {
                    bool Flag_State = false;
                    string FileName = UrlNamePath.Substring(UrlNamePath.LastIndexOf("\\") + 1);
                    string PATH = FileNamePath + FileName;
                    try
                    {
                        WebRequest SC = WebRequest.Create(UrlNamePath);
                        Flag_State = true;
                    }
                    catch (Exception ex)
                    {
                        Ini.WriteException(ex);
                        Flag_State = false;
                    }
                    try
                    {
                        //client.DownloadFile(URL, PATH);
                    }
                    catch (Exception ex)
                    {
                        Ini.WriteException(ex);
                    }
                    return Flag_State;
                }

                //另一种上传和下载的方式
                /// <summary>  
                /// 将本地文件上传到远程服务器共享目录,可更改扩展名  
                /// </summary>  
                /// <param name="FileNamePath">本地文件的绝对路径，包含扩展名</param>  
                /// <param name="UrlPath">远程服务器共享文件路径，不包含文件扩展名</param>  
                /// <param name="FileName">上传到远程服务器后的文件扩展名</param>  
                /// <returns></returns>  
                public static bool UpLoad(string FileNamePath, string UrlPath, string FileName)
                {
                    bool Flag_State = false;
                    try
                    {
                        FileStream inFileStream = new FileStream(FileNamePath, FileMode.Open);    //此处假定本地文件存在，不然程序会报错     

                        if (!Directory.Exists(UrlPath))        //判断上传到的远程服务器路径是否存在  
                        {
                            Directory.CreateDirectory(UrlPath);
                        }
                        UrlPath = UrlPath + FileName;             //上传到远程服务器共享文件夹后文件的绝对路径  

                        FileStream outFileStream = new FileStream(UrlPath, FileMode.OpenOrCreate);

                        byte[] buf = new byte[inFileStream.Length];

                        int byteCount;

                        while ((byteCount = inFileStream.Read(buf, 0, buf.Length)) > 0)
                        {
                            outFileStream.Write(buf, 0, byteCount);
                        }

                        inFileStream.Flush();

                        inFileStream.Close();

                        outFileStream.Flush();

                        outFileStream.Close();

                        Flag_State = true;
                    }
                    catch (Exception ex)
                    {
                        Ini.WriteException(ex);
                        Flag_State = false;
                    }
                    return Flag_State;
                }
                /// <summary>  
                /// 从远程服务器下载文件到本地,可从指定文件夹中指定某文件  
                /// </summary>  
                /// <param name="UrlPath">远程服务器路径，不包含文件名及扩展名</param>  
                /// <param name="FileNamePath">下载到本地后的文件路径</param>  
                /// <param name="FileName">远程服务器中的文件名称，包含扩展名</param>  
                /// <returns></returns>  
                public static bool DownLoad(string UrlPath, string FileNamePath, string FileName)
                {
                    bool Flag_State = false;
                    string DownFile = UrlPath + "\\" + FileName;
                    string DownPath = FileNamePath + "\\" + FileName;

                    try
                    {
                        if (!Directory.Exists(UrlPath))
                        {
                            Directory.CreateDirectory(UrlPath);
                        }
                        if (!Directory.Exists(FileNamePath))
                        {
                            Directory.CreateDirectory(FileNamePath);
                        }
                        FileStream inFileStream = new FileStream(DownFile, FileMode.Open);    //远程服务器文件  此处假定远程服务器共享文件夹下确实包含本文件，否则程序报错  

                        FileStream outFileStream = new FileStream(DownPath, FileMode.OpenOrCreate);   //从远程服务器下载到本地的文件  

                        byte[] buf = new byte[inFileStream.Length];

                        int byteCount;

                        while ((byteCount = inFileStream.Read(buf, 0, buf.Length)) > 0)
                        {
                            outFileStream.Write(buf, 0, byteCount);
                        }

                        inFileStream.Flush();

                        inFileStream.Close();

                        outFileStream.Flush();

                        outFileStream.Close();

                        Flag_State = true;
                    }
                    catch (Exception ex)
                    {
                        Ini.WriteException(ex);
                        Flag_State = false;
                    }
                    return Flag_State;
                }
            }

            /// <summary>
            /// 打开文件夹并定位文件
            /// </summary>
            /// <param name="FileName">定位文件路径</param>
            /// <returns></returns>
            public static void PositionFile(string FileName)
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                psi.Arguments = " /select," + FileName;
                System.Diagnostics.Process.Start(psi);
            }
        }

        /// <summary>
        /// INI文件操作类
        /// <para>主要包括：</para> 
        /// <para>1.加密方式写入内容</para> 
        /// <para>2.解密方式读出内容</para> 
        /// <para>3.不加密方式写入内容</para> 
        /// <para>4.不解密方式读出内容</para> 
        /// <para>5.记录系统运行异常，并保存至文件</para> 
        /// </summary>
        public class Ini
        {
            public static string Key_64 = "aabbccdd";                                                       //MD5文件加密key值
            public static string Iv_64 = "aabbccdd";                                                        //MD5文件加密IN向量值
            public static string Exception_location = Application.StartupPath + @"\Exception.ini";        //记录当前发生过异常的记录文件

            #region    Ini文件读写操作函数
            [DllImport("kernel32")]
            public static extern int WritePrivateProfileString(string section, string key, string val, string filePath);
            [DllImport("kernel32")]
            public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

            #region 文件写入操作
            /// <summary> 
            /// <para>section 写入的名称</para>
            /// <para>key 写入的K值</para>
            /// <para>value 写入的内容</para>
            /// <para>path 写入的路径</para>
            /// </summary>
            public static void IniWrite(string section, string key, string value, string path)      //INI文件加密写入操作
            {
                string data = Encode(value, Key_64, Iv_64);

                WritePrivateProfileString(section, key, data, path);
            }
            /// <summary> 
            /// <para>section 写入的名称</para>
            /// <para>key 写入的K值</para>
            /// <para>value 写入的内容</para>
            /// <para>path 写入的路径</para>
            /// </summary>
            public static void IniWrite(string section, int key, int value, string path)      //INI文件加密写入操作
            {
                string data = Encode(value.ToString(), Key_64, Iv_64);

                WritePrivateProfileString(section, key.ToString(), data, path);
            }
            /// <summary> 
            /// <para>section 写入的名称</para>
            /// <para>key 写入的K值</para>
            /// <para>value 写入的内容</para>
            /// <para>path 写入的路径</para>
            /// </summary>
            public static void IniWrite(string section, string key, int value, string path)      //INI文件加密写入操作
            {
                string data = Encode(value.ToString(), Key_64, Iv_64);

                WritePrivateProfileString(section, key, data, path);
            }
            /// <summary> 
            /// <para>section 写入的名称</para>
            /// <para>key 写入的K值</para>
            /// <para>value 写入的内容</para>
            /// <para>path 写入的路径</para>
            /// </summary>
            public static void IniWrite(string section, string key, double value, string path)      //INI文件加密写入操作
            {
                string data = Encode(value.ToString(), Key_64, Iv_64);

                WritePrivateProfileString(section, key, data, path);
            }
            /// <summary> 
            /// <para>section 写入的名称</para>
            /// <para>key 写入的K值</para>
            /// <para>value 写入的内容</para>
            /// <para>path 写入的路径</para>
            /// </summary>
            public static void IniWrite(string section, string key, bool value, string path)      //INI文件加密写入操作
            {
                string data = Encode(value.ToString(), Key_64, Iv_64);

                WritePrivateProfileString(section, key, data, path);
            }
            #endregion

            #region 文件读取操作
            /// <summary> 
            /// <para>section 读取的名称</para>
            /// <para>key 读取的K值</para>
            /// <para>path 读取的路径</para>
            /// </summary>
            public static string IniRead(string section, string skey, string path)                  //INI文件加密读取操作
            {
                StringBuilder temp = new StringBuilder(500);
                int i = GetPrivateProfileString(section, skey, "", temp, 500, path);
                string data = Decode(temp.ToString(), Key_64, Iv_64);
                data = data.Replace("\r", "");
                data = data.Replace("\n", "");
                return data;
            }
            /// <summary> 
            /// <para>section 读取的名称</para>
            /// <para>key 读取的K值</para>
            /// <para>path 读取的路径</para>
            /// </summary>
            public static string IniRead(string section, int skey, string path)                  //INI文件加密读取操作
            {
                StringBuilder temp = new StringBuilder(500);
                int i = GetPrivateProfileString(section, skey.ToString(), "", temp, 500, path);
                string data = Decode(temp.ToString(), Key_64, Iv_64);
                data = data.Replace("\r", "");
                data = data.Replace("\n", "");
                return data;
            }
            #endregion

            public static long IniDel_Sec(string section, string path)                              //INI文件删除操作
            {
                return WritePrivateProfileString(section, null, null, path);
            }

            public static long IniDel_Key(string section, string key, string path)
            {
                return WritePrivateProfileString(section, key, null, path);
            }

            /// <summary> 
            /// <para>section 写入的名称</para>
            /// <para>key 写入的K值</para>
            /// <para>value 写入的内容</para>
            /// <para>path 写入的路径</para>
            /// </summary>
            public static void RecordWrite(string section, string key, string value, string path)      //INI文件无加密写入操作
            {

                WritePrivateProfileString(section, key, value, path);
            }
            /// <summary> 
            /// <para>section 读取的名称</para>
            /// <para>key 读取的K值</para>
            /// <para>path 读取的路径</para>
            /// </summary>
            public static string RecordRead(string section, string skey, string path)                  //INI文件无加密读取操作
            {
                StringBuilder temp = new StringBuilder(500);
                int i = GetPrivateProfileString(section, skey, "", temp, 500, path);
                string data = temp.ToString();
                data = data.Replace("\r", "");
                data = data.Replace("\n", "");
                return data;
            }

            #endregion

            #region MD5加密

            public static string Encode(string data, string Key_64, string Iv_64)
            {
                string KEY_64 = Key_64;// "VavicApp";

                string IV_64 = Iv_64;// "VavicApp";

                try
                {
                    byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);

                    byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

                    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();

                    int i = cryptoProvider.KeySize;

                    MemoryStream ms = new MemoryStream();

                    CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);

                    StreamWriter sw = new StreamWriter(cst);

                    sw.Write(data);

                    sw.Flush();

                    cst.FlushFinalBlock();

                    sw.Flush();

                    return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);

                }

                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                    return err.Message;
                }

            }

            #endregion

            #region MD5解密

            public static string Decode(string data, string Key_64, string Iv_64)
            {

                string KEY_64 = Key_64;// "VavicApp";密钥

                string IV_64 = Iv_64;// "VavicApp"; 向量

                try
                {

                    byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);

                    byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

                    byte[] byEnc;

                    byEnc = Convert.FromBase64String(data); //把需要解密的字符串转为8位无符号数组

                    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();

                    MemoryStream ms = new MemoryStream(byEnc);

                    CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);

                    StreamReader sr = new StreamReader(cst);

                    return sr.ReadToEnd();

                }
                catch (Exception ex)
                {
                    Ini.WriteException(ex);
                    return "Err";
                }

            }

            #endregion

            public static void WriteException(Exception ex)
            {
                int ExceptionNum = int.Parse(RecordRead("Exception", "ExceptionNum", Exception_location));
                ExceptionNum++;
                RecordWrite(ExceptionNum.ToString(), "发生时间：", DateTime.Now.ToString(), Exception_location);
                RecordWrite(ExceptionNum.ToString(), "错误信息1：", ex.Message, Exception_location);
                RecordWrite(ExceptionNum.ToString(), "错误信息2：", ex.StackTrace, Exception_location);
                RecordWrite("Exception", "ExceptionNum", ExceptionNum.ToString(), Exception_location);
            }
        }

        /// <summary>
        /// 注册表操作类
        /// <para>主要包括：</para> 
        /// <para>1.创建注册表项</para> 
        /// <para>2.删除注册表项</para> 
        /// <para>3.读取某键下的全部子健列表</para> 
        /// <para>4.读取某键下的健值内容</para> 
        /// <para>5.写入某键下的健值内容</para> 
        /// <para>6.删除某键下的键值</para> 
        /// <para>7.判断注册表项是否存在</para> 
        /// <para>8.判断注册表键值是否存在</para> 
        /// 
        /// <para>版本:1.0</para> 
        /// </summary>
        public class Regedit
        {
            #region 创建注册表项
            /// <summary>
            /// 创建注册表项
            /// </summary>
            /// <param name="subkey">创建注册表项的路径</param>
            /// <param name="name">需要创建项的名称</param>
            public static bool Add_Book(string subkey, string name)
            {
                bool ret = false;
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }
                    if (name != "" && name != null)
                    {
                        Registry.LocalMachine.CreateSubKey(subkey + name);
                        ret = IsBookExist(subkey, name);
                    }
                    else
                    {
                        ret = false;
                    }
                }
                catch
                {
                    ret = false;
                }
                return ret;
            }
            /// <summary>
            /// 创建注册表项
            /// </summary>
            /// <param name="subkey">创建注册表项的路径,以\结尾</param>
            /// <param name="subdirectory">子文件夹名称</param>
            /// <param name="name">需要创建项的名称</param>
            public static bool Add_Book(string subkey, string subdirectory, string name)
            {
                bool ret = false;
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }
                    if (name != "" && name != null)
                    {
                        Registry.LocalMachine.CreateSubKey(subkey + subdirectory + "\\" + name);
                        ret = IsBookExist(subkey + subdirectory, name);
                    }
                    else
                    {
                        ret = false;
                    }
                }
                catch
                {
                    ret = false;
                }
                return ret;
            }
            #endregion

            #region 删除注册表项

            /// <summary>
            /// 删除注册表项
            /// </summary>
            /// <param name="subkey">注册表项的路径目录</param>
            public static bool Del_Book(string subkey)
            {
                bool ret = true;
                try
                {
                    string[] NewSubkey = subkey.Split('\\');
                    string name = "";
                    string path = "";

                    if (NewSubkey[NewSubkey.Length - 1] == "")
                    {
                        for (int i = 0; i < NewSubkey.Length - 2; i++)
                        {
                            path += NewSubkey[i] + "\\";
                        }
                        name = NewSubkey[NewSubkey.Length - 2];
                    }
                    else
                    {
                        for (int i = 0; i < NewSubkey.Length - 1; i++)
                        {
                            path += NewSubkey[i] + "\\";
                        }
                        name = NewSubkey[NewSubkey.Length - 1];
                    }

                    Registry.LocalMachine.DeleteSubKeyTree(subkey, false);
                    if (IsBookExist(subkey) == false)
                    {
                        ret = true;
                    }
                    else
                    {
                        ret = false;
                    }

                }
                catch
                {
                    ret = false;
                }
                return ret;
            }

            /// <summary>
            /// 删除注册表项
            /// </summary>
            /// <param name="subkey">注册表项的路径目录</param>
            /// <param name="name">需要删除项的名称</param>
            public static bool Del_Book(string subkey, string name)
            {
                bool ret = true;
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }

                    Registry.LocalMachine.DeleteSubKeyTree(subkey + name, false);
                    if (IsBookExist(subkey, name) == false)
                    {
                        ret = true;
                    }
                    else
                    {
                        ret = false;
                    }
                }
                catch
                {
                    ret = false;
                }
                return ret;
            }

            /// <summary>
            /// 删除注册表项
            /// </summary>
            /// <param name="subkey">注册表项的路径目录</param>
            /// <param name="subdirectory">注册表项的的子文件夹</param>
            /// <param name="name">需要删除项的名称</param>
            public static bool Del_Book(string subkey, string subdirectory, string name)
            {
                bool ret = true;
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }

                    Registry.LocalMachine.DeleteSubKeyTree(subkey + subdirectory + "\\" + name, false);
                    if (IsBookExist(subkey + subdirectory, name) == false)
                    {
                        ret = true;
                    }
                    else
                    {
                        ret = false;
                    }
                }
                catch
                {
                    ret = false;
                }
                return ret;
            }
            #endregion

            #region 复制注册表项以及下列所有项和键

            /// <summary>
            /// 复制注册表项以及下列所有项和键
            /// </summary>
            /// <param name="OldSubkey">原注册表项的路径目录</param>
            /// <param name="OldName">需要复制项的名称</param>
            /// <param name="NewSubkey">原注册表项的路径目录</param>
            /// <param name="NewName">需要存放到项的新名称</param>
            public static bool Copy_Book(string OldSubkey, string OldName, string NewSubkey, string NewName)
            {
                bool ret = true;
                try
                {
                    if (OldSubkey.Substring(OldSubkey.Length - 1, 1) != "\\")
                    {
                        OldSubkey += "\\";
                    }
                    if (NewSubkey.Substring(NewSubkey.Length - 1, 1) != "\\")
                    {
                        NewSubkey += "\\";
                    }

                    string[] booklist = Get_Book_List(OldSubkey, OldName);
                    if (booklist.Length >= 1)
                    {
                        for (int i = 0; i < booklist.Length; i++)
                        {
                            if (Copy_Book(OldSubkey + OldName, booklist[i], NewSubkey + NewName, booklist[i]) == false)
                            {
                                ret = false;
                                break;
                            }
                        }
                    }
                    else if (booklist.Length == 0)
                    {
                        string value;
                        string[] Value_List = Get_Value_List(OldSubkey, OldName);
                        for (int i = 0; i < Value_List.Length; i++)
                        {
                            value = Get_Value(OldSubkey, OldName, Value_List[i]);
                            ret = Set_Value(NewSubkey, NewName, Value_List[i], value);
                        }
                    }
                }
                catch
                {
                    ret = false;
                }
                return ret;
            }

            #endregion

            #region 读取某键下的全部子健列表
            /// <summary>
            /// 读取指定表下的全部项列表
            /// </summary>
            /// <param name="subkey">需要读取的项目录路径</param>
            /// <returns></returns>
            public static string[] Get_Book_List(string subkey)
            {
                try
                {
                    RegistryKey Surface = Registry.LocalMachine.OpenSubKey(subkey, true);
                    if (Surface != null)
                    {
                        string[] list = Surface.GetSubKeyNames();
                        Surface.Close();
                        return list;
                    }
                    else
                    {
                        string[] list = null;
                        return list;
                    }
                }
                catch
                {
                    string[] list = null;
                    return list;
                }
            }

            /// <summary>
            /// 读取指定表下的全部项列表
            /// </summary>
            /// <param name="subkey">需要读取的项目录路径</param>
            /// <param name="subdirectory">需要读取的项子文件夹名称</param>
            /// <returns></returns>
            public static string[] Get_Book_List(string subkey, string subdirectory)
            {
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }
                    RegistryKey Surface = Registry.LocalMachine.OpenSubKey(subkey + subdirectory + "\\", true);
                    if (Surface != null)
                    {
                        string[] list = Surface.GetSubKeyNames();
                        Surface.Close();
                        return list;
                    }
                    else
                    {
                        string[] list = null;
                        return list;
                    }
                }
                catch
                {
                    string[] list = null;
                    return list;
                }
            }

            /// <summary>
            /// 读取指定表下的全部项列表
            /// </summary>
            /// <param name="subkey">需要读取的项目录路径</param>
            /// <param name="subdirectory">项目录路径的子文件夹名称</param>
            /// <param name="name">需要读取的项子文件夹名称</param>
            /// <returns></returns>
            public static string[] Get_Book_List(string subkey, string subdirectory, string name)
            {
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }
                    RegistryKey Surface = Registry.LocalMachine.OpenSubKey(subkey + subdirectory + "\\" + name + "\\", true);
                    if (Surface != null)
                    {
                        string[] list = Surface.GetSubKeyNames();
                        Surface.Close();
                        return list;
                    }
                    else
                    {
                        string[] list = null;
                        return list;
                    }
                }
                catch
                {
                    string[] list = null;
                    return list;
                }
            }
            #endregion

            #region 读取某键下的健值内容
            /// <summary>
            /// 读取某键下的健值内容
            /// </summary>
            /// <param name="subkey">需要读取键的目录路径</param>
            /// <param name="name">需要读取键的名称</param>
            /// <returns></returns>
            public static string Get_Value(string subkey, string name)
            {
                string Data = "Err";
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }
                    if (IsValueExist(subkey, name) == true)
                    {
                        RegistryKey Surface = Registry.LocalMachine.OpenSubKey(subkey, true);
                        if (Surface != null)
                        {
                            Data = Surface.GetValue(name).ToString();
                            Surface.Close();
                            return Data;
                        }
                        else
                        {
                            return Data;
                        }
                    }
                    else
                    {
                        return Data;
                    }
                }
                catch
                {
                    return Data;
                }
            }

            /// <summary>
            /// 读取某键下的健值内容
            /// </summary>
            /// <param name="subkey">需要读取键的目录路径</param>
            /// <param name="subdirectory">需要读取键的子文件夹</param>
            /// <param name="name">需要读取键的名称</param>
            /// <returns></returns>
            public static string Get_Value(string subkey, string subdirectory, string name)
            {
                string Data = null;
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }
                    if (IsValueExist(subkey + subdirectory, name) == true)
                    {
                        RegistryKey Surface = Registry.LocalMachine.OpenSubKey(subkey + subdirectory + "\\", true);
                        if (Surface != null)
                        {
                            Data = Surface.GetValue(name).ToString();
                            Surface.Close();
                            return Data;
                        }
                        else
                        {
                            return Data;
                        }
                    }
                    else
                    {
                        return Data;
                    }
                }
                catch
                {
                    return Data;
                }
            }

            /// <summary>
            /// 读取某键下的健值内容
            /// </summary>
            /// <param name="subkey">需要读取键的目录路径</param>
            /// <param name="subdirectory1">需要读取键的子文件夹1</param>
            /// <param name="subdirectory2">需要读取键的子文件夹2</param>
            /// <param name="name">需要读取键的名称</param>
            /// <returns></returns>
            public static string Get_Value(string subkey, string subdirectory1, string subdirectory2, string name)
            {
                string Data = null;
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }
                    if (IsValueExist(subkey + subdirectory1 + "\\" + subdirectory2, name) == true)
                    {
                        RegistryKey Surface = Registry.LocalMachine.OpenSubKey(subkey + subdirectory1 + "\\" + subdirectory2 + "\\", true);
                        if (Surface != null)
                        {
                            Data = Surface.GetValue(name).ToString();
                            Surface.Close();
                            return Data;
                        }
                        else
                        {
                            return Data;
                        }
                    }
                    else
                    {
                        return Data;
                    }
                }
                catch
                {
                    return Data;
                }
            }

            /// <summary>
            /// 读取某键下的健值内容
            /// </summary>
            /// <param name="subkey">需要读取键的目录路径</param>
            /// <param name="subdirectory1">需要读取键的子文件夹1</param>
            /// <param name="subdirectory2">需要读取键的子文件夹2</param>
            /// <param name="subdirectory3">需要读取键的子文件夹3</param>
            /// <param name="name">需要读取键的名称</param>
            /// <returns></returns>
            public static string Get_Value(string subkey, string subdirectory1, string subdirectory2, string subdirectory3, string name)
            {
                string Data = null;
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }
                    if (IsValueExist(subkey + subdirectory1 + "\\" + subdirectory2 +  "\\" + subdirectory3 + "\\", name) == true)
                    {
                        RegistryKey Surface = Registry.LocalMachine.OpenSubKey(subkey + subdirectory1 + "\\" + subdirectory2 + "\\" + subdirectory3 + "\\", true);
                        if (Surface != null)
                        {
                            Data = Surface.GetValue(name).ToString();
                            Surface.Close();
                            return Data;
                        }
                        else
                        {
                            return Data;
                        }
                    }
                    else
                    {
                        return Data;
                    }
                }
                catch
                {
                    return Data;
                }
            }

            /// <summary>
            /// 读取某键下的健值内容
            /// </summary>
            /// <param name="subkey">需要读取键的目录路径</param>
            /// <param name="subdirectory1">需要读取键的子文件夹1</param>
            /// <param name="subdirectory2">需要读取键的子文件夹2</param>
            /// <param name="subdirectory3">需要读取键的子文件夹3</param>
            /// <param name="subdirectory4">需要读取键的子文件夹4</param>
            /// <param name="name">需要读取键的名称</param>
            /// <returns></returns>
            public static string Get_Value(string subkey, string subdirectory1, string subdirectory2, string subdirectory3, string subdirectory4, string name)
            {
                string Data = null;
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }
                    if (IsValueExist(subkey + subdirectory1 + "\\" + subdirectory2 + "\\" + subdirectory3 + "\\" + subdirectory4 + "\\", name) == true)
                    {
                        RegistryKey Surface = Registry.LocalMachine.OpenSubKey(subkey + subdirectory1 + "\\" + subdirectory2 + "\\" + subdirectory3 + "\\" + subdirectory4 + "\\", true);
                        if (Surface != null)
                        {
                            Data = Surface.GetValue(name).ToString();
                            Surface.Close();
                            return Data;
                        }
                        else
                        {
                            return Data;
                        }
                    }
                    else
                    {
                        return Data;
                    }
                }
                catch
                {
                    return Data;
                }
            }

            /// <summary>
            /// 读取某键下的健值内容
            /// </summary>
            /// <param name="subkey">需要读取键的目录路径</param>
            /// <param name="subdirectory1">需要读取键的子文件夹1</param>
            /// <param name="subdirectory2">需要读取键的子文件夹2</param>
            /// <param name="subdirectory3">需要读取键的子文件夹3</param>
            /// <param name="subdirectory4">需要读取键的子文件夹4</param>
            /// <param name="subdirectory5">需要读取键的子文件夹5</param>
            /// <param name="name">需要读取键的名称</param>
            /// <returns></returns>
            public static string Get_Value(string subkey, string subdirectory1, string subdirectory2, string subdirectory3, string subdirectory4, string subdirectory5, string name)
            {
                string Data = null;
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }
                    if (IsValueExist(subkey + subdirectory1 + "\\" + subdirectory2 + "\\" + subdirectory3 + "\\" + subdirectory4 + "\\" + subdirectory5 + "\\", name) == true)
                    {
                        RegistryKey Surface = Registry.LocalMachine.OpenSubKey(subkey + subdirectory1 + "\\" + subdirectory2 + "\\" + subdirectory3 + "\\" + subdirectory4 + "\\" + subdirectory5 + "\\", true);
                        if (Surface != null)
                        {
                            Data = Surface.GetValue(name).ToString();
                            Surface.Close();
                            return Data;
                        }
                        else
                        {
                            return Data;
                        }
                    }
                    else
                    {
                        return Data;
                    }
                }
                catch
                {
                    return Data;
                }
            }

            /// <summary>
            /// 读取某键下的健值内容
            /// </summary>
            /// <param name="subkey">需要读取键的目录路径</param>
            /// <param name="subdirectory1">需要读取键的子文件夹1</param>
            /// <param name="subdirectory2">需要读取键的子文件夹2</param>
            /// <param name="subdirectory3">需要读取键的子文件夹3</param>
            /// <param name="subdirectory4">需要读取键的子文件夹4</param>
            /// <param name="subdirectory5">需要读取键的子文件夹5</param>
            /// <param name="subdirectory6">需要读取键的子文件夹6</param>
            /// <param name="name">需要读取键的名称</param>
            /// <returns></returns>
            public static string Get_Value(string subkey, string subdirectory1, string subdirectory2, string subdirectory3, string subdirectory4, string subdirectory5, string subdirectory6, string name)
            {
                string Data = null;
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }
                    if (IsValueExist(subkey + subdirectory1 + "\\" + subdirectory2 + "\\" + subdirectory3 + "\\" + subdirectory4 + "\\" + subdirectory5 + "\\" + subdirectory6 + "\\", name) == true)
                    {
                        RegistryKey Surface = Registry.LocalMachine.OpenSubKey(subkey + subdirectory1 + "\\" + subdirectory2 + "\\" + subdirectory3 + "\\" + subdirectory4 + "\\" + subdirectory5 + "\\" + subdirectory6 + "\\", true);
                        if (Surface != null)
                        {
                            Data = Surface.GetValue(name).ToString();
                            Surface.Close();
                            return Data;
                        }
                        else
                        {
                            return Data;
                        }
                    }
                    else
                    {
                        return Data;
                    }
                }
                catch
                {
                    return Data;
                }
            }

            #endregion

            #region 读取某表下的全部键列表
            /// <summary>
            /// 读取某表下的全部键列表
            /// </summary>
            /// <param name="subkey">需要读取键列表的目录路径</param>
            /// <returns>返回为一个目录组</returns>
            public static string[] Get_Value_List(string subkey)
            {
                try
                {
                    RegistryKey Surface = Registry.LocalMachine.OpenSubKey(subkey, true);
                    if (Surface != null)
                    {
                        string[] list = Surface.GetValueNames();
                        Surface.Close();
                        return list;
                    }
                    else
                    {
                        string[] list = null;
                        return list;
                    }
                }
                catch
                {
                    string[] list = null;
                    return list;
                }
            }

            /// <summary>
            /// 读取某表下的全部键列表
            /// </summary>
            /// <param name="subkey">需要读取键列表的目录路径</param>
            /// <param name="subdirectory">需要读取键列表的子文件夹名称</param>
            /// <returns>返回为一个目录组</returns>
            public static string[] Get_Value_List(string subkey, string subdirectory)
            {
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }
                    RegistryKey Surface = Registry.LocalMachine.OpenSubKey(subkey + subdirectory + "\\", true);
                    if (Surface != null)
                    {
                        string[] list = Surface.GetValueNames();
                        Surface.Close();
                        return list;
                    }
                    else
                    {
                        string[] list = null;
                        return list;
                    }
                }
                catch
                {
                    string[] list = null;
                    return list;
                }
            }

            #endregion

            #region 写入某键下的健值内容
            /// <summary>
            /// 写入某键下的健值内容
            /// </summary>
            /// <param name="subkey">需要写入键的目录路径</param>
            /// <param name="name">需要写入内容的键名称</param>
            /// <param name="value">需要写入的内容</param> 
            /// <returns>返回true为成功，返回false为失败</returns>
            public static bool Set_Value(string subkey, string name, object value)
            {
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }
                    RegistryKey Surface = Registry.LocalMachine.CreateSubKey(subkey);
                    Surface.SetValue(name, value);
                    if (Get_Value(subkey, name) == value.ToString())
                    {
                        Surface.Close();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }

            /// <summary>
            /// 写入某键下的健值内容
            /// </summary>
            /// <param name="subkey">需要写入键的目录路径</param>
            /// <param name="subdirectory">需要写入键的子文件夹</param>
            /// <param name="name">需要写入内容的键名称</param>
            /// <param name="value">需要写入的内容</param> 
            /// <returns>返回true为成功，返回false为失败</returns>
            public static bool Set_Value(string subkey, string subdirectory, string name, object value)
            {
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }
                    RegistryKey Surface = Registry.LocalMachine.CreateSubKey(subkey + subdirectory + "\\");
                    Surface.SetValue(name, value);
                    if (Get_Value(subkey + subdirectory + "\\", name) == value.ToString())
                    {
                        Surface.Close();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
            /// <summary>
            /// 写入某键下的健值内容
            /// </summary>
            /// <param name="subkey">需要写入键的目录路径</param>
            /// <param name="subdirectory1">需要写入键的子文件夹1</param>
            /// <param name="subdirectory2">需要写入键的子文件夹2</param>
            /// <param name="name">需要写入内容的键名称</param>
            /// <param name="value">需要写入的内容</param> 
            /// <returns>返回true为成功，返回false为失败</returns>
            public static bool Set_Value(string subkey, string subdirectory1, string subdirectory2, string name, object value)
            {
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }
                    RegistryKey Surface = Registry.LocalMachine.CreateSubKey(subkey + subdirectory1 + "\\" + subdirectory2 + "\\");
                    Surface.SetValue(name, value);
                    if (Get_Value(subkey + subdirectory1 + "\\" + subdirectory2 + "\\", name) == value.ToString())
                    {
                        Surface.Close();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
            /// <summary>
            /// 写入某键下的健值内容
            /// </summary>
            /// <param name="subkey">需要写入键的目录路径</param>
            /// <param name="subdirectory1">需要写入键的子文件夹1</param>
            /// <param name="subdirectory2">需要写入键的子文件夹2</param>
            /// <param name="subdirectory3">需要写入键的子文件夹3</param>
            /// <param name="name">需要写入内容的键名称</param>
            /// <param name="value">需要写入的内容</param> 
            /// <returns>返回true为成功，返回false为失败</returns>
            public static bool Set_Value(string subkey, string subdirectory1, string subdirectory2, string subdirectory3, string name, object value)
            {
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }
                    RegistryKey Surface = Registry.LocalMachine.CreateSubKey(subkey + subdirectory1 + "\\" + subdirectory2 + "\\" + subdirectory3 + "\\");
                    Surface.SetValue(name, value);
                    if (Get_Value(subkey + subdirectory1 + "\\" + subdirectory2 + "\\" + subdirectory3 + "\\", name) == value.ToString())
                    {
                        Surface.Close();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }

            /// <summary>
            /// 写入某键下的健值内容
            /// </summary>
            /// <param name="subkey">需要写入键的目录路径</param>
            /// <param name="subdirectory1">需要写入键的子文件夹1</param>
            /// <param name="subdirectory2">需要写入键的子文件夹2</param>
            /// <param name="subdirectory3">需要写入键的子文件夹3</param>
            /// <param name="subdirectory4">需要写入键的子文件夹4</param>
            /// <param name="name">需要写入内容的键名称</param>
            /// <param name="value">需要写入的内容</param> 
            /// <returns>返回true为成功，返回false为失败</returns>
            public static bool Set_Value(string subkey, string subdirectory1, string subdirectory2, string subdirectory3, string subdirectory4, string name, object value)
            {
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }
                    RegistryKey Surface = Registry.LocalMachine.CreateSubKey(subkey + subdirectory1 + "\\" + subdirectory2 + "\\" + subdirectory3 + "\\"+ subdirectory4 + "\\");
                    Surface.SetValue(name, value);
                    if (Get_Value(subkey + subdirectory1 + "\\" + subdirectory2 + "\\" + subdirectory3 + "\\" + subdirectory4 + "\\", name) == value.ToString())
                    {
                        Surface.Close();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }

            /// <summary>
            /// 写入某键下的健值内容
            /// </summary>
            /// <param name="subkey">需要写入键的目录路径</param>
            /// <param name="subdirectory1">需要写入键的子文件夹1</param>
            /// <param name="subdirectory2">需要写入键的子文件夹2</param>
            /// <param name="subdirectory3">需要写入键的子文件夹3</param>
            /// <param name="subdirectory4">需要写入键的子文件夹4</param>
            /// <param name="subdirectory5">需要写入键的子文件夹5</param>
            /// <param name="subdirectory6">需要写入键的子文件夹6</param>
            /// <param name="name">需要写入内容的键名称</param>
            /// <param name="value">需要写入的内容</param> 
            /// <returns>返回true为成功，返回false为失败</returns>
            public static bool Set_Value(string subkey, string subdirectory1, string subdirectory2, string subdirectory3, string subdirectory4, string subdirectory5,  string name, object value)
            {
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }
                    RegistryKey Surface = Registry.LocalMachine.CreateSubKey(subkey + subdirectory1 + "\\" + subdirectory2 + "\\" + subdirectory3 + "\\" + subdirectory4 + "\\" + subdirectory5 + "\\");
                    Surface.SetValue(name, value);
                    if (Get_Value(subkey + subdirectory1 + "\\" + subdirectory2 + "\\" + subdirectory3 + "\\" + subdirectory4 + "\\" + subdirectory5 + "\\" , name) == value.ToString())
                    {
                        Surface.Close();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }

            /// <summary>
            /// 写入某键下的健值内容
            /// </summary>
            /// <param name="subkey">需要写入键的目录路径</param>
            /// <param name="subdirectory1">需要写入键的子文件夹1</param>
            /// <param name="subdirectory2">需要写入键的子文件夹2</param>
            /// <param name="subdirectory3">需要写入键的子文件夹3</param>
            /// <param name="subdirectory4">需要写入键的子文件夹4</param>
            /// <param name="subdirectory5">需要写入键的子文件夹5</param>
            /// <param name="subdirectory6">需要写入键的子文件夹6</param>
            /// <param name="name">需要写入内容的键名称</param>
            /// <param name="value">需要写入的内容</param> 
            /// <returns>返回true为成功，返回false为失败</returns>
            public static bool Set_Value(string subkey, string subdirectory1, string subdirectory2, string subdirectory3, string subdirectory4, string subdirectory5, string subdirectory6, string name, object value)
            {
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }
                    RegistryKey Surface = Registry.LocalMachine.CreateSubKey(subkey + subdirectory1 + "\\" + subdirectory2 + "\\" + subdirectory3 + "\\" + subdirectory4 + "\\"+ subdirectory5 + "\\"+ subdirectory6 + "\\");
                    Surface.SetValue(name, value);
                    if (Get_Value(subkey + subdirectory1 + "\\" + subdirectory2 + "\\" + subdirectory3 + "\\" + subdirectory4 + "\\" + subdirectory5 + "\\" + subdirectory6 + "\\", name) == value.ToString())
                    {
                        Surface.Close();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
            #endregion

            #region 删除某键下的键值
            /// <summary>
            /// 删除注册表中指定的子健下的键值
            /// </summary>
            /// <param name="subkey">需要删除键的目录路径</param>
            /// <returns>返回true为成功，返回false为失败</returns>
            public static bool Del_Value(string subkey)
            {
                try
                {

                    string[] NewSubkey = subkey.Split('\\');
                    string name = "";
                    string path = "";

                    if (NewSubkey[NewSubkey.Length - 1] == "")
                    {
                        for (int i = 0; i < NewSubkey.Length - 2; i++)
                        {
                            path += NewSubkey[i] + "\\";
                        }
                        name = NewSubkey[NewSubkey.Length - 2];
                    }
                    else
                    {
                        for (int i = 0; i < NewSubkey.Length - 1; i++)
                        {
                            path += NewSubkey[i] + "\\";
                        }
                        name = NewSubkey[NewSubkey.Length - 1];
                    }
                    if (IsValueExist(path, name) == true)
                    {
                        RegistryKey Surface = Registry.LocalMachine.OpenSubKey(path, true);
                        Surface.DeleteValue(name, true);
                        Surface.Close();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
            /// <summary>
            /// 删除注册表中指定的子健下的键值
            /// </summary>
            /// <param name="subkey">需要删除键的目录路径</param>
            /// <param name="name">需要删除值的键名称</param>
            /// <returns>返回true为成功，返回false为失败</returns>
            public static bool Del_Value(string subkey, string name)
            {
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }
                    RegistryKey Surface = Registry.LocalMachine.OpenSubKey(subkey, true);
                    Surface.DeleteValue(name, true);
                    Surface.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            /// <summary>
            /// 删除注册表中指定的子健下的键值
            /// </summary>
            /// <param name="subkey">需要删除键的目录路径</param>
            /// <param name="subdirectory">需要删除键的子文件夹名称</param>
            /// <param name="name">需要删除值的键名称</param>
            /// <returns>返回true为成功，返回false为失败</returns>
            public static bool Del_Value(string subkey, string subdirectory, string name)
            {
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }
                    RegistryKey Surface = Registry.LocalMachine.OpenSubKey(subkey + subdirectory + "\\", true);
                    Surface.DeleteValue(name, true);
                    Surface.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            #endregion

            #region 判断注册表项是否存在
            /// <summary>
            /// 判断注册表项是否存在
            /// </summary>
            /// <param name="subkey">需要判断项的目录路径</param>
            /// <param name="name">需要判断是否存在的项名称</param>
            /// <returns>返回true为成功，返回false为失败</returns>
            public static bool IsBookExist(string subkey, string name)
            {
                try
                {
                    if (subkey.Substring(subkey.Length - 1, 1) != "\\")
                    {
                        subkey += "\\";
                    }
                    RegistryKey Surface = Registry.LocalMachine.OpenSubKey(subkey + name, true);
                    if (Surface != null)
                    {
                        Surface.Close();
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch
                {
                    return false;
                }
            }
            /// <summary>
            /// 判断注册表项是否存在
            /// </summary>
            /// <param name="subkey">需要判断项的项路径</param>
            /// <returns>返回true为成功，返回false为失败</returns>
            public static bool IsBookExist(string subkey)
            {
                try
                {
                    RegistryKey Surface = Registry.LocalMachine.OpenSubKey(subkey, true);
                    if (Surface != null)
                    {
                        Surface.Close();
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch
                {
                    return false;
                }
            }
            #endregion

            #region 判断注册表键值是否存在
            /// <summary>
            /// 判断注册表键是否存在
            /// </summary>
            /// <param name="subkey">需要判断项的目录路径</param>
            /// <param name="name">需要判断是否存在的键名称</param>
            /// <returns>返回true为成功，返回false为失败</returns>
            public static bool IsValueExist(string subkey, string name)
            {
                bool ret = false;
                try
                {
                    if (IsBookExist(subkey) == true)
                    {
                        RegistryKey Surface = Registry.LocalMachine.OpenSubKey(subkey, true);
                        string[] list = Surface.GetValueNames();
                        for (int i = 0; i < list.Length; i++)
                        {
                            if (list[i] == name)
                            {
                                ret = true;
                            }
                        }
                    }
                    else
                    {
                        ret = false;
                    }
                }
                catch
                {
                    ret = false;
                }
                return ret;
            }

            /// <summary>
            /// 判断注册表键是否存在
            /// </summary>
            /// <param name="subkey">需要判断键的目录路径</param>
            /// <param name="subdirectory">需要判断键的子文件夹名称</param>
            /// <param name="name">需要判断是否存在的键名称</param>
            /// <returns>返回true为成功，返回false为失败</returns>
            public static bool IsValueExist(string subkey, string subdirectory, string name)
            {
                bool ret = false;
                try
                {
                    if (IsBookExist(subkey + subdirectory + "\\") == true)
                    {
                        RegistryKey Surface = Registry.LocalMachine.OpenSubKey(subkey + subdirectory + "\\", true);
                        string[] list = Surface.GetValueNames();
                        for (int i = 0; i < list.Length; i++)
                        {
                            if (list[i] == name)
                            {
                                ret = true;
                            }
                        }
                    }
                    else
                    {
                        ret = false;
                    }
                }
                catch
                {
                    ret = false;
                }
                return ret;
            }
            #endregion

            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
            static extern int MessageBox(IntPtr hwnd, string text, string caption, uint type);

            /// <summary>    
            /// 从注册表导出到文件，在导出的过程是异步的，不受操作进程管理    
            /// </summary>    
            /// <param name="SavingFilePath">从注册表导出的文件，如果是已存在的，会提示覆盖；    
            /// 如果不存在由参数指定名字的文件，将自动创建一个。导出的文件的扩展名应当是.REG的</param>    
            /// <param name="regPath">指定注册表的某一键被导出，如果指定null值，将导出整个注册表</param>    
            /// <returns>成功返回0，用户中断返回1</returns>    
            public static int ExportReg(string SavingFilePath, string regPath)
            {
                //如果文件存在，MSG提示是否覆盖，不覆盖，中断操作    
                //如果注册表路径为空，导出全部    
                if (File.Exists(SavingFilePath))
                    if (MessageBox(IntPtr.Zero,
                        string.Format("存在名为：{0}的文件，是否覆盖 ？", SavingFilePath),
                        string.Format("进程：{0} pid: {1}",
                        Process.GetCurrentProcess().ProcessName,
                        Process.GetCurrentProcess().Id), 0x00000004 | 0x00200000 | 0x00000020 | 0x00000100) == 7)
                    {
                        return 1; //说明，在应用的地方，用对话框，再操作,再调用一次    
                    }

                Process.Start("regedit", string.Format(" /E {0} {1} ", SavingFilePath, regPath));
                //Console.WriteLine(10);//异步的，非同步执行    
                // Feng.Regedit.RegExportImport.ExportReg(@"c:/789.reg",@"HKEY_LOCAL_MACHINE/Software/Microsoft/Windows/CurrentVersion/Run");    
                // Feng.Regedit.  RegExportImport.ExportReg(@"c:/789.reg",null);    
                return 0;
            }
            /// <summary>    
            /// 从文件导入的注册表    
            /// </summary>    
            /// <param name="SavedFilePath">指定在磁盘上存在的文件，如果指定的文件不存在，将抛出异常</param>    
            /// <param name="regPath">指定注册表的键（包含在SavedFilePath文件中保存的关键字），如果该参数设置为null将导入整个 SavedFilePath文件    
            /// 中保存的所有关于注册表的关键字</param>    
            /// <returns>成功返回0</returns>    
            public static int ImportReg(string SavedFilePath, string regPath)
            {
                if (!File.Exists(SavedFilePath))
                {
                    throw new ArgumentException("参数 SavedFilePath 指定无效路径");
                }
                else
                {
                    Process.Start("regedit", string.Format(" /C {0} {1}", SavedFilePath, regPath));//线程外的    
                    return 0;
                }
            }
        }

        /// <summary>
        /// 日志记录、支持其他线程访问
        /// <para>主要包括：</para> 
        /// <para>1.Success表示成功类信息，绿色字体</para> 
        /// <para>2.Message表示无任何状态，白色字体</para> 
        /// <para>3.Error表示错误状态，红色字体</para> 
        /// <para>4.Warning表示警告状态，黄色字体</para> 
        /// </summary>
        public class Log
        {
            private delegate void LogAppendDelegate(RichTextBox box, Color color, string text);
            private delegate void LogClearDelegate(RichTextBox box);
            private delegate void LogEndDelegate(RichTextBox box);

            /// <summary> 
            /// 追加显示文本 
            /// </summary> 
            /// <param name="box">所需追加文本的控件</param> 
            /// <param name="color">文本颜色</param> 
            /// <param name="text">显示文本</param> 
            private static void LogAppend(RichTextBox box, Color color, string text)
            {
                box.SelectionColor = color;
                box.AppendText(text);
                box.AppendText("\n");
            }
            /// <summary> 
            /// 清除控件内所有文本信息 
            /// </summary> 
            /// <param name="box">所需清除文本内容的控件</param> 
            private static void LogClear(RichTextBox box)
            {
                box.Clear();
            }
            /// <summary> 
            /// 显示最新的消息 
            /// </summary> 
            /// <param name="box">需要查找字符串的控件</param> 
            private static void LogEnd(RichTextBox box)
            {
                box.SelectionStart = box.TextLength;
                box.ScrollToCaret();
            }
            /// <summary> 
            /// 查找控件内文本信息 
            /// </summary> 
            /// <param name="box">需要查找字符串的控件</param> 
            /// <param name="text">需要查找的字符串</param> 
            public static bool Find(RichTextBox box, string text)
            {
                bool ret = false;
                try
                {
                    box.Invoke((EventHandler)(delegate
                    {
                        foreach (var line in box.Lines)
                        {
                            if (line.Contains(text))//忽略大小写
                            {
                                ret = true;
                            }
                        }
                    }
                    ));
                }
                catch
                {

                }
                return ret;
            }
            /// <summary> 
            /// 查找控件内文本信息，中间会以-隔开时间与字符串
            /// </summary> 
            /// <param name="box">需要查找字符串的控件</param> 
            /// <param name="time">需要查找的时间字符串</param> 
            /// <param name="text">需要查找的字符串</param> 
            public static bool Find(RichTextBox box, string time, string text)
            {
                bool ret = false;
                try
                {
                    box.Invoke((EventHandler)(delegate
                    {
                        foreach (var line in box.Lines)
                        {
                            if (line.Contains(" " + time + "   -   " + text))//忽略大小写
                            {
                                ret = true;
                            }
                        }
                    }
                    ));
                }
                catch
                {

                }
                return ret;
            }
            /// <summary> 
            /// 将显示位置放置在最新消息处 
            /// </summary> 
            /// <param name="box">所需清除文本内容的控件</param> 
            public static void End(RichTextBox box)
            {
                if (box.IsHandleCreated)
                {
                    LogEndDelegate la = new LogEndDelegate(LogEnd);
                    box.Invoke(la, box);
                }
            }
            /// <summary> 
            /// 清除控件内所有文本信息 
            /// </summary> 
            /// <param name="box">所需清除文本内容的控件</param> 
            public static void Clear(RichTextBox box)
            {
                LogClearDelegate la = new LogClearDelegate(LogClear);
                box.Invoke(la, box);
            }
            /// <summary> 
            /// 显示成功日志 
            /// </summary> 
            /// <param name="box">RichTextBox的窗体控件</param> 
            /// <param name="text">显示文本</param> 
            public static void Success(RichTextBox box, string text)
            {
                LogAppendDelegate la = new LogAppendDelegate(LogAppend);
                box.Invoke(la, box, Color.Green, " " + DateTime.Now.ToString("G") + "   -   " + text);
            }
            /// <summary> 
            /// 显示信息 
            /// </summary> 
            /// <param name="box">RichTextBox的窗体控件</param> 
            /// <param name="text">显示文本</param> 
            public static void Message(RichTextBox box, string text)
            {
                LogAppendDelegate la = new LogAppendDelegate(LogAppend);
                box.Invoke(la, box, Color.Black, " " + DateTime.Now.ToString("G") + "   -   " + text);
            }
            /// <summary> 
            /// 显示信息 
            /// </summary> 
            /// <param name="box">RichTextBox的窗体控件</param> 
            /// <param name="text">显示文本</param> 
            /// <param name="CL">需要显示的颜色</param> 
            public static void Message(RichTextBox box, Color color, string text)
            {
                LogAppendDelegate la = new LogAppendDelegate(LogAppend);
                box.Invoke(la, box, color, " " + DateTime.Now.ToString("G") + "   -   " + text);
            }
            /// <summary> 
            /// 显示信息 
            /// </summary> 
            /// <param name="box">RichTextBox的窗体控件</param> 
            /// <param name="color">需要显示的颜色</param> 
            /// <param name="time">需要显示的时间</param> 
            /// <param name="text">显示文本</param> 
            public static void Message(RichTextBox box, Color color, string time, string text)
            {
                LogAppendDelegate la = new LogAppendDelegate(LogAppend);
                box.Invoke(la, box, color, " " + time + "   -   " + text);
            }
            /// <summary> 
            /// 显示错误日志 
            /// </summary> 
            /// <param name="box">RichTextBox的窗体控件</param> 
            /// <param name="text">显示文本</param> 
            public static void Error(RichTextBox box, string text)
            {
                LogAppendDelegate la = new LogAppendDelegate(LogAppend);
                box.Invoke(la, box, Color.Red, " " + DateTime.Now.ToString("G") + "   -   " + text);
            }
            /// <summary> 
            /// 显示警告信息 
            /// </summary> 
            /// <param name="box">RichTextBox的窗体控件</param> 
            /// <param name="text">显示文本</param> 
            public static void Warning(RichTextBox box, string text)
            {
                LogAppendDelegate la = new LogAppendDelegate(LogAppend);
                box.Invoke(la, box, Color.Violet, " " + DateTime.Now.ToString("G") + "   -   " + text);
            }
        }

        public class Win
        {
            public const Int32 AW_HOR_POSITIVE = 0x00000001; // 从左到右打开窗口
            public const Int32 AW_HOR_NEGATIVE = 0x00000002; // 从右到左打开窗口
            public const Int32 AW_VER_POSITIVE = 0x00000004; // 从上到下打开窗口
            public const Int32 AW_VER_NEGATIVE = 0x00000008; // 从下到上打开窗口
            public const Int32 AW_CENTER = 0x00000010; //若使用了AW_HIDE标志，则使窗口向内重叠；若未使用AW_HIDE标志，则使窗口向外扩展。
            public const Int32 AW_HIDE = 0x00010000; //隐藏窗口，缺省则显示窗口。
            public const Int32 AW_ACTIVATE = 0x00020000; //激活窗口。在使用了AW_HIDE标志后不要使用这个标志。
            public const Int32 AW_SLIDE = 0x00040000; //使用滑动类型。缺省则为滚动动画类型。当使用AW_CENTER标志时，这个标志就被忽略。
            public const Int32 AW_BLEND = 0x00080000; //使用淡出效果。只有当hWnd为顶层窗口的时候才可以使用此标志。
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            /****************************handle to window***duration of animation****animation type**********/
            public static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

            //Function.Win32.AnimateWindow(this.Handler, 100, Function.Win32.AW_SLIDE | Function.Win32.AW_HIDE | Function.Win32.AW_BLEND);关闭动画

            //API 常數定義

            public const int SW_HIDE = 0;
            public const int SW_NORMAL = 1;
            public const int SW_MAXIMIZE = 3;
            public const int SW_SHOWNOACTIVATE = 4;
            public const int SW_SHOW = 5;
            public const int SW_MINIMIZE = 6;
            public const int SW_RESTORE = 9;
            public const int SW_SHOWDEFAULT = 10;

            [DllImport("user32.dll")]
            private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

            [DllImport("user32.dll")]
            public static extern int ShowWindow(int hwnd, int nCmdShow);

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            private static extern bool OpenIcon(IntPtr hWnd);

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            private static extern bool IsIconic(IntPtr hWnd);

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            private static extern int SetForegroundWindow(IntPtr hWnd);

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

            /// <summary>
            /// 判断是否打开Mdi的窗口,如打开则得到窗口
            /// </summary>
            /// <param name="asFormName"></param>
            /// <returns></returns>
            public static bool HaveOpened(Form asFormName)
            {
                bool a = false;
                foreach (Form frm in Application.OpenForms)
                {
                    if (asFormName != null)
                    {
                        if (frm.Name == asFormName.Name)
                        {
                            a = true;
                            break;
                        }
                    }
                }
                return a;
            }

            /// <summary>
            /// 判断是否打开的窗口,如打开则得到窗口,没打开则打开窗口
            /// </summary>
            /// <param name="window">判断以及打开的窗体</param>
            /// <param name="home">指父窗体一般用this</param>
            /// <param name="name">设定未打开时的窗体名称</param>
            /// <param name="Dialog">窗口是否需要锁定</param>
            /// <returns></returns>
            public static void OpenWindow(Form window, bool Dialog)
            {
                IntPtr hWnd = new IntPtr(0);

                hWnd = FindWindow(null, window.Text);
                //判断这个窗体是否有效
                if (hWnd == IntPtr.Zero)
                {
                    window.TopMost = true;
                    window.ShowIcon = true;
                    window.ShowInTaskbar = false;
                    window.MaximizeBox = false;
                    window.MinimizeBox = false;
                    window.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                    window.StartPosition = FormStartPosition.CenterScreen;
                    if (Dialog == false)
                    {
                        window.Show();
                    }
                    else
                    {
                        window.ShowDialog();
                    }

                }

                else
                {
                    bool isIcon = IsIconic(hWnd);

                    if (!isIcon)
                    {
                        SetForegroundWindow(hWnd);
                    }
                    else
                    {
                        OpenIcon(hWnd);
                    }
                }
            }

            /// <summary>
            /// 判断是否打开的窗口,如打开则得到窗口,没打开则打开窗口
            /// </summary>
            /// <param name="window">判断以及打开的窗体</param>
            /// <param name="home">指父窗体一般用this</param>
            /// <param name="name">设定未打开时的窗体名称</param>
            /// <param name="Dialog">窗口是否需要锁定</param>
            /// <returns></returns>
            public static void OpenWindow(Form window, Form home, string name, bool Dialog)
            {
                IntPtr hWnd = new IntPtr(0);

                hWnd = FindWindow(null, name);
                //判断这个窗体是否有效
                if (hWnd == IntPtr.Zero)
                {
                    window.TopMost = true;
                    window.Text = name;
                    window.ShowIcon = true;
                    window.ShowInTaskbar = false;
                    window.MaximizeBox = false;
                    window.MinimizeBox = false;
                    window.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                    window.StartPosition = FormStartPosition.CenterScreen;
                    if (Dialog == false)
                    {
                        if (home != null)
                        {
                            window.Show(home);
                        }
                        else
                        {
                            window.Show();
                        }
                    }
                    else
                    {
                        if (home != null)
                        {
                            window.ShowDialog(home);
                        }
                        else
                        {
                            window.ShowDialog();
                        }
                    }

                }

                else
                {
                    bool isIcon = IsIconic(hWnd);

                    if (!isIcon)
                    {
                        SetForegroundWindow(hWnd);
                    }
                    else
                    {
                        OpenIcon(hWnd);
                    }
                }
            }
            /// <summary>
            /// 判断是否打开的窗口,如打开则得到窗口,没打开则打开窗口
            /// </summary>
            /// <param name="window">判断打开的窗体名称</param>
            /// <param name="home">指父窗体一般用this</param>
            /// <param name="name">设定未打开时的窗体名称</param>
            /// <param name="Dialog">窗口是否需要锁定</param>
            /// <param name="IsOne">窗口是否为单个</param>
            /// <returns></returns>
            public static void OpenWindow(Form window, Form home, string name, bool Dialog, bool IsOne)
            {
                if (IsOne == true)
                {
                    IntPtr hWnd = new IntPtr(0);

                    hWnd = FindWindow(null, name);
                    //判断这个窗体是否有效
                    if (hWnd == IntPtr.Zero)
                    {
                        window.TopMost = true;
                        window.Text = name;
                        window.ShowIcon = true;
                        window.ShowInTaskbar = false;
                        window.MaximizeBox = false;
                        window.MinimizeBox = false;
                        window.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                        window.StartPosition = FormStartPosition.CenterScreen;
                        if (Dialog == false)
                        {
                            if (home != null)
                            {
                                window.Show(home);
                            }
                            else
                            {
                                window.Show();
                            }
                        }
                        else
                        {
                            if (home != null)
                            {
                                window.ShowDialog(home);
                            }
                            else
                            {
                                window.ShowDialog();
                            }
                        }
                    }
                    else
                    {
                        bool isIcon = IsIconic(hWnd);

                        ShowWindowAsync(hWnd, SW_NORMAL);
                        ShowWindowAsync(hWnd, SW_SHOW);

                        if (!isIcon)
                        {
                            SetForegroundWindow(hWnd);
                        }
                        else
                        {
                            OpenIcon(hWnd);
                        }
                    }
                }
                else
                {
                    window.TopMost = true;
                    window.Text = name;
                    window.ShowIcon = true;
                    window.ShowInTaskbar = false;
                    window.MaximizeBox = false;
                    window.MinimizeBox = false;
                    window.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                    window.StartPosition = FormStartPosition.CenterScreen;
                    if (Dialog == false)
                    {
                        if (home != null)
                        {
                            window.Show(home);
                        }
                        else
                        {
                            window.Show();
                        }
                    }
                    else
                    {
                        if (home != null)
                        {
                            window.ShowDialog(home);
                        }
                        else
                        {
                            window.ShowDialog();
                        }
                    }
                }
            }
            /// <summary>
            /// 判断是否打开的窗口,如打开则得到窗口,没打开则打开窗口
            /// </summary>
            /// <param name="window">判断打开的窗体名称</param>
            /// <param name="home">指父窗体一般用this</param>
            /// <param name="name">设定未打开时的窗体名称</param>
            /// <param name="Dialog">窗口是否需要锁定</param>
            /// <param name="Loc">窗体打开的初始位置</param>
            /// <returns></returns>
            public static void OpenWindow(Form window, Form home, string name, bool Dialog, Point Loc)
            {
                IntPtr hWnd = new IntPtr(0);

                hWnd = FindWindow(null, name);
                //判断这个窗体是否有效
                if (hWnd == IntPtr.Zero)
                {
                    window.TopMost = true;
                    window.Text = name;
                    window.ShowIcon = true;
                    window.ShowInTaskbar = false;
                    window.MaximizeBox = false;
                    window.MinimizeBox = false;
                    window.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                    window.StartPosition = FormStartPosition.Manual;
                    window.Left = Loc.X;
                    window.Top = Loc.Y;
                    if (Dialog == false)
                    {
                        if (home != null)
                        {
                            window.Show(home);
                        }
                        else
                        {
                            window.Show();
                        }
                    }
                    else
                    {
                        if (home != null)
                        {
                            window.ShowDialog(home);
                        }
                        else
                        {
                            window.ShowDialog();
                        }
                    }
                }
                else
                {
                    bool isIcon = IsIconic(hWnd);

                    if (!isIcon)
                    {
                        SetForegroundWindow(hWnd);
                    }
                    else
                    {
                        OpenIcon(hWnd);
                    }
                }
            }
            /// <summary>
            /// 判断是否打开的窗口,如打开则得到窗口,没打开则打开窗口
            /// </summary>
            /// <param name="window">判断打开的窗体名称</param>
            /// <param name="home">指父窗体一般用this</param>
            /// <param name="name">设定未打开时的窗体名称</param>
            /// <param name="Dialog">窗口是否需要锁定</param>
            /// <param name="IsOne">窗口是否为单个</param>
            /// <param name="Loc">设定窗体初始打开位置</param>
            /// <returns></returns>
            public static void OpenWindow(Form window, Form home, string name, bool Dialog, bool IsOne, Point Loc)
            {
                if (IsOne == true)
                {
                    IntPtr hWnd = new IntPtr(0);

                    hWnd = FindWindow(null, name);
                    //判断这个窗体是否有效
                    if (hWnd == IntPtr.Zero)
                    {
                        window.TopMost = true;
                        window.Text = name;
                        window.ShowIcon = true;
                        window.ShowInTaskbar = false;
                        window.MaximizeBox = false;
                        window.MinimizeBox = false;
                        window.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                        window.StartPosition = FormStartPosition.Manual;
                        window.Left = Loc.X;
                        window.Top = Loc.Y;
                        if (Dialog == false)
                        {
                            if (home != null)
                            {
                                window.Show(home);
                            }
                            else
                            {
                                window.Show();
                            }
                        }
                        else
                        {
                            if (home != null)
                            {
                                window.ShowDialog(home);
                            }
                            else
                            {
                                window.ShowDialog();
                            }
                        }
                    }
                    else
                    {
                        bool isIcon = IsIconic(hWnd);

                        ShowWindowAsync(hWnd, SW_NORMAL);
                        ShowWindowAsync(hWnd, SW_SHOW);

                        if (!isIcon)
                        {
                            SetForegroundWindow(hWnd);
                        }
                        else
                        {
                            OpenIcon(hWnd);
                        }
                    }
                }
                else
                {
                    window.TopMost = true;
                    window.Text = name;
                    window.ShowIcon = true;
                    window.ShowInTaskbar = false;
                    window.MaximizeBox = false;
                    window.MinimizeBox = false;
                    window.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                    window.StartPosition = FormStartPosition.Manual;
                    window.Left = Loc.X;
                    window.Top = Loc.Y;
                    if (Dialog == false)
                    {
                        if (home != null)
                        {
                            window.Show(home);
                        }
                        else
                        {
                            window.Show();
                        }
                    }
                    else
                    {
                        if (home != null)
                        {
                            window.ShowDialog(home);
                        }
                        else
                        {
                            window.ShowDialog();
                        }
                    }
                }
            }

            /// <summary>
            /// 设定窗体状态、显示、最小化、隐藏
            /// </summary>
            /// <param name="name">设定窗体的名称</param>
            ///  <param name="state">设定窗体状态，0-正常显示、1-最小化，2-隐藏至托盘</param>
            /// <returns></returns>
            public static void Operation_Window(string name, int state)
            {
                IntPtr hWnd = new IntPtr(0);

                hWnd = FindWindow(null, name);
                //判断这个窗体是否有效
                if (hWnd != IntPtr.Zero)
                {
                    if (state == 0)
                    {
                        ShowWindowAsync(hWnd, SW_NORMAL);
                        ShowWindowAsync(hWnd, SW_SHOW);
                    }
                    else if (state == 1)
                    {
                        ShowWindowAsync(hWnd, SW_NORMAL);
                        ShowWindowAsync(hWnd, SW_HIDE);
                    }
                    else if (state == 2)
                    {
                        ShowWindowAsync(hWnd, SW_NORMAL);
                        ShowWindowAsync(hWnd, SW_HIDE);
                    }

                }
            }

            /// <summary>
            /// 设置IP地址
            /// </summary>
            /// <param name="ip">设置的IP地址</param>
            /// <param name="submask">设置的子网掩码地址</param>
            /// <param name="getway">设定的网关地址</param>
            /// <param name="dns">设定的DNS地址</param>
            public static bool SetIPAddress(string IP, string SubMask, string GetWay, string DNS1, string DNS2, string NetName)
            {
                bool Ret = false;
                ManagementBaseObject NewIp = null;
                ManagementBaseObject NewSubMask = null;
                ManagementBaseObject NewDNS = null;

                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true && mo["Description"].ToString() == NetName)
                    {
                        NewIp = mo.GetMethodParameters("EnableStatic");
                        NewIp["IPAddress"] = new string[] { IP };
                        NewIp["SubnetMask"] = new string[] { SubMask };
                        NewIp = mo.InvokeMethod("EnableStatic", NewIp, null);

                        NewSubMask = mo.GetMethodParameters("SetGateways");
                        NewSubMask["DefaultIPGateway"] = new string[] { GetWay };
                        NewSubMask = mo.InvokeMethod("SetGateways", NewSubMask, null);

                        NewDNS = mo.GetMethodParameters("SetDNSServerSearchOrder");
                        NewDNS["DNSServerSearchOrder"] = new string[] { DNS1, DNS2 };
                        NewDNS = mo.InvokeMethod("SetDNSServerSearchOrder", NewDNS, null);

                        if (NewIp["returnvalue"].ToString() == "0" || NewIp["returnvalue"].ToString() == "1" &&
                            NewSubMask["returnvalue"].ToString() == "0" || NewSubMask["returnvalue"].ToString() == "1" &&
                            NewDNS["returnvalue"].ToString() == "0" || NewDNS["returnvalue"].ToString() == "1")
                        {
                            Ret = true;
                        }
                        else
                        {
                            Ret = false;
                        }

                        break;
                    }
                }
                return Ret;
            }

            /// <summary>
            /// 启用DHCP服务
            /// </summary>
            public static void SetDHCP(string NetName)
            {
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true && mo["Description"].ToString() == NetName)
                    {
                        mo.InvokeMethod("SetDNSServerSearchOrder", null);
                        mo.InvokeMethod("EnableStatic", null);
                        mo.InvokeMethod("SetGateways", null);
                        mo.InvokeMethod("EnableDHCP", null);
                        break;
                    }
                }
            }

            /// <summary>
            /// 获取本地网卡及IP地址信息
            /// </summary>
            public static string[,] GetEetList()
            {
                string[,] List;
                NetworkInterface[] NetNum = NetworkInterface.GetAllNetworkInterfaces();
                List = new string[NetNum.Length, 9];
                for (int i = 0; i < NetNum.Length; i++)
                {
                    #region  网卡类型
                    string fRegistryKey = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\" + NetNum[i].Id + "\\Connection";
                    RegistryKey rk = Registry.LocalMachine.OpenSubKey(fRegistryKey, false);

                    if (rk != null)
                    {
                        // 区分 PnpInstanceID   

                        // 如果前面有 PCI 就是本机的真实网卡  

                        // MediaSubType 为 01 则是常见网卡，02为无线网卡。  

                        string fPnpInstanceID = rk.GetValue("PnpInstanceID", "").ToString();

                        int fMediaSubType = Convert.ToInt32(rk.GetValue("MediaSubType", 0));

                        if (fPnpInstanceID.Length > 3 && fPnpInstanceID.Substring(0, 3) == "PCI")
                        {
                            List[i, 0] = NetNum[i].Id;
                            List[i, 1] = NetNum[i].Name;
                            List[i, 2] = NetNum[i].Description;
                            List[i, 3] = NetNum[i].NetworkInterfaceType.ToString();
                            List[i, 4] = NetNum[i].IsReceiveOnly.ToString();
                            List[i, 5] = NetNum[i].SupportsMulticast.ToString();
                            List[i, 6] = NetNum[i].Speed.ToString();
                            List[i, 7] = NetNum[i].GetPhysicalAddress().ToString();

                            IPInterfaceProperties fIPInterfaceProperties = NetNum[i].GetIPProperties();

                            UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = fIPInterfaceProperties.UnicastAddresses;

                            foreach (UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection)
                            {

                                if (UnicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    List[i, 8] = UnicastIPAddressInformation.Address.ToString();
                                }
                            }
                        }
                        else if (fMediaSubType == 1)
                        {

                        }
                        else if (fMediaSubType == 2)
                        {
                            List[i, 0] = NetNum[i].Id;
                            List[i, 1] = NetNum[i].Name;
                            List[i, 2] = NetNum[i].Description;
                            List[i, 3] = NetNum[i].NetworkInterfaceType.ToString();
                            List[i, 4] = NetNum[i].IsReceiveOnly.ToString();
                            List[i, 5] = NetNum[i].SupportsMulticast.ToString();
                            List[i, 6] = NetNum[i].Speed.ToString();
                            List[i, 7] = NetNum[i].GetPhysicalAddress().ToString();

                            IPInterfaceProperties fIPInterfaceProperties = NetNum[i].GetIPProperties();

                            UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = fIPInterfaceProperties.UnicastAddresses;

                            foreach (UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection)
                            {

                                if (UnicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    List[i, 8] = UnicastIPAddressInformation.Address.ToString();
                                }
                            }
                        }
                    }

                    #endregion
                }
                return List;
            }

        }

        /// <summary>
        /// 文本框限制输入，可只输入数字或者小数
        /// </summary>
        public class Input
        {
           /// <summary>
           /// 只可输入0-9
           /// </summary>
           /// <param name="sender"></param>
           /// <param name="e"></param>
            public static void In_Put_Int(object sender, KeyPressEventArgs e)
            {
                #region     只抓取0-9

                //判断输入按键是不是要输入的类型
                //如果输入的数字不是0-9或者是退格键就取消输入

                TextBox In_put = (TextBox)sender;

                if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8 && (int)e.KeyChar != 45)
                {
                    e.Handled = true;
                }
                else
                {

                    if (In_put.Text.Length == 1 && (int)e.KeyChar == 48)
                    {
                        if (In_put.Text.Substring(0, 1) == "0")
                        {
                            e.Handled = true;
                        }
                    }
                    if (In_put.Text.Length > 0)
                    {
                        if ((int)e.KeyChar == 45)
                        {
                            e.Handled = true;
                        }
                    }

                }
                #endregion

            }
            /// <summary>
            /// 只可输入±、0-9与小数点
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public static void In_Put_Float(object sender, KeyPressEventArgs e)
            {
                #region     数据抓取    0-9     退格键     小数点

                //判断输入按键是不是要输入的类型
                //如果输入的数字不是 0-9、退格键、小数点 就取消输入

                //TextBox In_put = (TextBox)sender;

                //if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8 && (int)e.KeyChar != 46)
                //    e.Handled = true;
                ////小数点的处理。
                //if ((int)e.KeyChar == 46)                           //小数点
                //{
                //    if (In_put.Text.Length <= 0)
                //        e.Handled = true;   //小数点不能在第一位
                //    else
                //    {
                //        double f;
                //        double oldf;
                //        bool b1 = false, b2 = false;
                //        b1 = double.TryParse(In_put.Text, out oldf);
                //        b2 = double.TryParse(In_put.Text + e.KeyChar.ToString(), out f);
                //        if (b2 == false)
                //        {
                //            if (b1 == true)
                //                e.Handled = true;
                //            else
                //                e.Handled = false;
                //        }
                //    }
                //}

                var tb = sender as TextBox;

                if (tb == null)
                {
                    e.Handled = true;
                    return;
                }

                if ((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == 8 || e.KeyChar == 46) //数字、Backspace、小数点  
                {
                    var editText = (tb.Text);

                    if (e.KeyChar != 8)
                    {
                        var selStart = tb.SelectionStart;
                        var selLength = tb.SelectionLength;

                        if (selLength > 0) //存在选择的内容，进行替换。  
                        {
                            editText = editText.Remove(selStart, selLength);

                            tb.Text = editText;
                            tb.SelectionLength = 0;
                            tb.SelectionStart = selStart;
                        }

                        editText = editText.Insert(selStart, e.KeyChar.ToString());

                        try
                        {
                            //校验新数据是否合法。  
                            var newValue = double.Parse(editText);
                            e.Handled = !(newValue >= 0);
                        }
                        catch (Exception)
                        {
                            e.Handled = true;
                            return;
                        }
                        e.Handled = false;
                    }
                    else
                        e.Handled = false;
                }
                else
                {
                    //正负数切换  
                    if (e.KeyChar == 45)
                    {
                        if (tb.SelectionLength != tb.Text.Length)
                        {
                            tb.Text = tb.Text.Contains("-") ? tb.Text.Replace("-", "") : tb.Text.Insert(0, "-");
                        }
                        else
                        {
                            tb.Text = "-";
                        }
                        tb.Select(tb.Text.Length, 0);
                    }

                    e.Handled = true;
                }
                #endregion

            }

            /// <summary>
            /// 输入完成后检查是否在有效值之内
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            /// <param name="a">值1</param>
            /// <param name="b">值2</param>
            public static void In_Put_Judgment(object sender, EventArgs e, double a, double b)
            {
                #region     输入完成后检查是否符合

                TextBox In_put = (TextBox)sender;
                try
                {
                    if (In_put.Text != "")
                    {
                        if (double.Parse(In_put.Text) > a || double.Parse(In_put.Text) < b)
                        {
                            MessageBox.Show("输入错误" + "\n" + "\n" + "请输入" + b.ToString() + "-" + a.ToString() + "之间的有效数值", "提示");
                            In_put.Text = b.ToString();
                            In_put.Focus();
                            In_put.SelectAll();
                        }
                    }
                    else
                    {
                        MessageBox.Show("请输入有效数值", "提示");
                        In_put.Text = b.ToString();
                        In_put.Focus();
                        In_put.SelectAll();
                    }
                }
                catch
                {
                    MessageBox.Show("请输入有效数值", "提示");
                    In_put.Text = b.ToString();
                    In_put.Focus();
                    In_put.SelectAll();
                }
                #endregion
            }

            /// <summary>
            /// 选中点击输入框的内容
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public static void Focus_In(object sender, EventArgs e)
            {
                TextBox In_put = (TextBox)sender;
                In_put.SelectAll();
            }
        }

        public class Other
        {
            /// <summary> 
            /// 设定控件的前景色
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="TColor">当State = True 时的颜色</param> 
            /// <param name="FColor">当State = Flase 时的颜色</param> 
            /// <param name="SUI">需要设定的控件</param> 
            public static void SetFColor(bool State, Color TColor, Color FColor, ref Label SUI)
            {
                if (State == true)
                {
                    SUI.ForeColor = TColor;
                }
                else
                {
                    SUI.ForeColor = FColor;
                }
            }

            /// <summary> 
            /// 设定控件的前景色
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="TColor">当State = True 时的颜色</param> 
            /// <param name="FColor">当State = Flase 时的颜色</param> 
            /// <param name="SUI">需要设定的控件</param> 
            public static void SetColor(bool State, Color TColor, Color FColor, ref Color SUI)
            {
                if (State == true)
                {
                    SUI = TColor;
                }
                else
                {
                    SUI = FColor;
                }
            }

            /// <summary> 
            /// 设定控件的前景色
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="TColor">当State = True 时的颜色</param> 
            /// <param name="FColor">当State = Flase 时的颜色</param> 
            /// <param name="SUI">需要设定的控件</param> 
            public static void SetFColor(bool State, Color TColor, Color FColor, ref TextBox SUI)
            {
                if (State == true)
                {
                    SUI.ForeColor = TColor;
                }
                else
                {
                    SUI.ForeColor = FColor;
                }
            }
            /// <summary> 
            /// 设定控件的前景色
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="TColor">当State = True 时的颜色</param> 
            /// <param name="FColor">当State = Flase 时的颜色</param> 
            /// <param name="SUI">需要设定的控件</param> 
            public static void SetFColor(bool State, Color TColor, Color FColor, ref Button SUI)
            {
                if (State == true)
                {
                    SUI.ForeColor = TColor;
                }
                else
                {
                    SUI.ForeColor = FColor;
                }
            }
            /// <summary> 
            /// 设定控件的背景色
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="TColor">当State = True 时的颜色</param> 
            /// <param name="FColor">当State = Flase 时的颜色</param> 
            /// <param name="SUI">需要设定的控件</param> 
            public static void SetBColor(bool State, Color TColor, Color FColor, ref Label SUI)
            {
                if (State == true)
                {
                    SUI.BackColor = TColor;
                }
                else
                {
                    SUI.BackColor = FColor;
                }
            }
            /// <summary> 
            /// 设定控件的背景色
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="TColor">当State = True 时的颜色</param> 
            /// <param name="FColor">当State = Flase 时的颜色</param> 
            /// <param name="SUI">需要设定的控件</param> 
            public static void SetBColor(bool State, Color TColor, Color FColor, ref TextBox SUI)
            {
                if (State == true)
                {
                    SUI.BackColor = TColor;
                }
                else
                {
                    SUI.BackColor = FColor;
                }
            }
            /// <summary> 
            /// 设定控件的背景色
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="TColor">当State = True 时的颜色</param> 
            /// <param name="FColor">当State = Flase 时的颜色</param> 
            /// <param name="SUI">需要设定的控件</param> 
            public static void SetBColor(bool State, Color TColor, Color FColor, ref Button SUI)
            {
                if (State == true)
                {
                    SUI.BackColor = TColor;
                }
                else
                {
                    SUI.BackColor = FColor;
                }
            }
            /// <summary> 
            /// 设定控件的背景色
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="TColor">当State = True 时的颜色</param> 
            /// <param name="FColor">当State = Flase 时的颜色</param> 
            /// <param name="SUI">需要设定的控件</param> 
            public static void SetBColor(bool State, Color TColor, Color FColor, ref HslControls.HslButton SUI)
            {
                if (State == true)
                {
                    SUI.OriginalColor = TColor;
                }
                else
                {
                    SUI.OriginalColor = FColor;
                }
            }
            /// <summary> 
            /// 设定控件的活动色
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="TColor">当State = True 时的颜色</param> 
            /// <param name="FColor">当State = Flase 时的颜色</param> 
            /// <param name="SUI">需要设定的控件</param> 
            public static void SetActiveColor(bool State, Color TColor, Color FColor, ref HslControls.HslButton SUI)
            {
                if (State == true)
                {
                    SUI.ActiveColor = TColor;
                }
                else
                {
                    SUI.ActiveColor = FColor;
                }
            }

            /// <summary> 
            /// 设定控件的Text属性
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的文字</param> 
            /// <param name="Fstr">当State = Flase 时的文字</param> 
            /// <param name="SUI">需要设定的控件</param> 
            public static void SetText(bool State, float Tstr, string Fstr, ref Label SUI)
            {
                if (State == true)
                {
                    SUI.Text = Tstr.ToString();
                }
                else
                {
                    SUI.Text = Fstr;
                }
            }
            /// <summary> 
            /// 设定控件的Text属性
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的文字</param> 
            /// <param name="Fstr">当State = Flase 时的文字</param> 
            /// <param name="SUI">需要设定的控件</param> 
            public static void SetText(bool State, string Tstr, string Fstr, ref Label SUI)
            {
                if (State == true)
                {
                    SUI.Text = Tstr;
                }
                else
                {
                    SUI.Text = Fstr;
                }
            }
            /// <summary> 
            /// 设定控件的Text属性
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的文字</param> 
            /// <param name="Fstr">当State = Flase 时的文字</param> 
            /// <param name="SUI">需要设定的控件</param> 
            public static void SetText(bool State, string Tstr, string Fstr, ref TextBox SUI)
            {
                if (State == true)
                {
                    SUI.Text = Tstr;
                }
                else
                {
                    SUI.Text = Fstr;
                }
            }
            /// <summary> 
            /// 设定控件的Text属性
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的文字</param> 
            /// <param name="Fstr">当State = Flase 时的文字</param> 
            /// <param name="SUI">需要设定的控件</param> 
            public static void SetText(bool State, string Tstr, string Fstr, ref Button SUI)
            {
                if (State == true)
                {
                    SUI.Text = Tstr;
                }
                else
                {
                    SUI.Text = Fstr;
                }
            }

            /// <summary> 
            /// 设定控件的Checked属性
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的状态</param> 
            /// <param name="Fstr">当State = Flase 时的状态</param> 
            /// <param name="SUI">需要设定的控件</param> 
            public static void SetVable(bool State, bool Tstr, bool Fstr, ref CheckBox SUI)
            {
                if (State == true)
                {
                    SUI.Checked = Tstr;
                }
                else
                {
                    SUI.Checked = Fstr;
                }
            }

            /// <summary> 
            /// 设定控件的Checked属性
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的状态</param> 
            /// <param name="Fstr">当State = Flase 时的状态</param> 
            /// <param name="SUI">需要设定的控件</param> 
            public static void SetVable(bool State, string Tstr, bool Fstr, ref CheckBox SUI)
            {
                if (State == true)
                {
                    if (Tstr == "1" || Tstr.ToLower() == "true")
                    {
                        SUI.Checked = true;
                    }
                    else
                    {
                        SUI.Checked = false;
                    }
                }
                else
                {
                    SUI.Checked = Fstr;
                }
            }

            /// <summary> 
            /// 设定控件的Checked属性
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的状态</param> 
            /// <param name="Fstr">当State = Flase 时的状态</param> 
            /// <param name="SUI">需要设定的控件</param> 
            public static void SetVable(bool State, string Tstr, bool Fstr, ref RadioButton SUI)
            {
                if (State == true)
                {
                    if (Tstr == "1" || Tstr.ToLower() == "true")
                    {
                        SUI.Checked = true;
                    }
                    else
                    {
                        SUI.Checked = false;
                    }
                }
                else
                {
                    SUI.Checked = Fstr;
                }
            }

            /// <summary> 
            /// 设定控件的Text属性
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的文字</param> 
            /// <param name="Fstr">当State = Flase 时的文字</param> 
            /// <param name="SUI">需要设定的控件</param> 
            public static void SetVable(bool State, float Tstr, string Fstr, ref Label SUI)
            {
                if (State == true)
                {
                    SUI.Text = Tstr.ToString();
                }
                else
                {
                    SUI.Text = Fstr;
                }
            }
            /// <summary> 
            /// 设定控件的Text属性
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的文字</param> 
            /// <param name="Fstr">当State = Flase 时的文字</param> 
            /// <param name="SUI">需要设定的控件</param> 
            public static void SetVable(bool State, string Tstr, string Fstr, ref Label SUI)
            {
                if (State == true)
                {
                    SUI.Text = Tstr;
                }
                else
                {
                    SUI.Text = Fstr;
                }
            }
            /// <summary> 
            /// 设定控件的Text属性
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的文字</param> 
            /// <param name="Fstr">当State = Flase 时的文字</param> 
            /// <param name="SUI">需要设定的控件</param> 
            public static void SetVable(bool State, string Tstr, string Fstr, ref TextBox SUI)
            {
                if (State == true)
                {
                    SUI.Text = Tstr;
                }
                else
                {
                    SUI.Text = Fstr;
                }
            }
            /// <summary> 
            /// 设定控件的Text属性
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的文字</param> 
            /// <param name="Fstr">当State = Flase 时的文字</param> 
            /// <param name="SUI">需要设定的控件</param> 
            public static void SetVable(bool State, string Tstr, string Fstr, ref Button SUI)
            {
                if (State == true)
                {
                    SUI.Text = Tstr;
                }
                else
                {
                    SUI.Text = Fstr;
                }
            }

            /// <summary> 
            /// 设定控件的Value属性
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的文字</param> 
            /// <param name="Fstr">当State = Flase 时的文字</param> 
            /// <param name="SUI">需要设定的控件</param> 
            public static void SetVable(bool State, string Tstr, int Fstr, ref TrackBar SUI)
            {
                if (State == true)
                {
                    try
                    {
                        if (Tstr != null)
                        {
                            SUI.Value = int.Parse(Tstr);
                        }
                        else
                        {
                            SUI.Value = Fstr;
                        }
                    }
                    catch
                    {
                        SUI.Value = Fstr;
                    }
                }
                else
                {
                    SUI.Value = Fstr;
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, string Tstr, string Fstr, ref string SUI)
            {
                if (State == true)
                {
                    if (Tstr != null)
                    {
                        SUI = Tstr;
                    }
                }
                else
                {
                    if (Fstr != null)
                    {
                        SUI = Fstr;
                    }
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, int Tstr, int Fstr, ref string SUI)
            {
                if (State == true)
                {
                    SUI = Tstr.ToString();
                }
                else
                {
                    SUI = Fstr.ToString();
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, string Tstr, int Fstr, ref string SUI)
            {
                if (State == true)
                {
                    if (Tstr != null)
                    {
                        SUI = Tstr;
                    }
                }
                else
                {
                    SUI = Fstr.ToString();
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, int Tstr, string Fstr, ref string SUI)
            {
                if (State == true)
                {
                    SUI = Tstr.ToString();
                }
                else
                {
                    if (Fstr != null)
                    {
                        SUI = Fstr;
                    }
                }
            }

            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, string Tstr, string Fstr, ref float SUI)
            {
                if (State == true)
                {
                    if (Tstr != null)
                    {
                        SUI = float.Parse(Tstr);
                    }
                }
                else
                {
                    if (Fstr != null)
                    {
                        SUI = float.Parse(Fstr);
                    }
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, float Tstr, float Fstr, ref float SUI)
            {
                if (State == true)
                {
                    SUI = Tstr;
                }
                else
                {
                    SUI = Fstr;
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, string Tstr, float Fstr, ref float SUI)
            {
                if (State == true)
                {
                    if (Tstr != null)
                    {
                        SUI = float.Parse(Tstr);
                    }
                }
                else
                {
                    SUI = Fstr;
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, float Tstr, string Fstr, ref float SUI)
            {
                if (State == true)
                {
                    SUI = Tstr;
                }
                else
                {
                    if (Fstr != null)
                    {
                        SUI = float.Parse(Fstr);
                    }
                }
            }

            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, string Tstr, string Fstr, ref double SUI)
            {
                if (State == true)
                {
                    if (Tstr != null)
                    {
                        SUI = double.Parse(Tstr);
                    }
                }
                else
                {
                    if (Fstr != null)
                    {
                        SUI = double.Parse(Fstr);
                    }
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, double Tstr, double Fstr, ref double SUI)
            {
                if (State == true)
                {
                    SUI = Tstr;
                }
                else
                {
                    SUI = Fstr;
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, string Tstr, double Fstr, ref double SUI)
            {
                if (State == true)
                {
                    if (Tstr != null)
                    {
                        SUI = double.Parse(Tstr);
                    }
                }
                else
                {
                    SUI = Fstr;
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, double Tstr, string Fstr, ref double SUI)
            {
                if (State == true)
                {
                    SUI = Tstr;
                }
                else
                {
                    if (Fstr != null)
                    {
                        SUI = double.Parse(Fstr);
                    }
                }
            }

            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, string Tstr, string Fstr, ref int SUI)
            {
                if (State == true)
                {
                    if (Tstr != null)
                    {
                        SUI = int.Parse(Tstr);
                    }
                }
                else
                {
                    if (Fstr != null)
                    {
                        SUI = int.Parse(Fstr);
                    }
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, int Tstr, int Fstr, ref int SUI)
            {
                if (State == true)
                {
                    SUI = Tstr;
                }
                else
                {
                    SUI = Fstr;
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, string Tstr, int Fstr, ref int SUI)
            {
                if (State == true)
                {
                    if (Tstr != null)
                    {
                        SUI = int.Parse(Tstr);
                    }
                }
                else
                {
                    SUI = Fstr;
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, int Tstr, string Fstr, ref int SUI)
            {
                if (State == true)
                {
                    SUI = Tstr;
                }
                else
                {
                    if (Fstr != null)
                    {
                        SUI = int.Parse(Fstr);
                    }
                }
            }

            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, string Tstr, string Fstr, ref uint SUI)
            {
                if (State == true)
                {
                    if (Tstr != null)
                    {
                        SUI = uint.Parse(Tstr);
                    }
                }
                else
                {
                    if (Fstr != null)
                    {
                        SUI = uint.Parse(Fstr);
                    }
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, uint Tstr, uint Fstr, ref uint SUI)
            {
                if (State == true)
                {
                    SUI = Tstr;
                }
                else
                {
                    SUI = Fstr;
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, string Tstr, uint Fstr, ref uint SUI)
            {
                if (State == true)
                {
                    if (Tstr != null)
                    {
                        SUI = uint.Parse(Tstr);
                    }
                }
                else
                {
                    SUI = Fstr;
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, uint Tstr, string Fstr, ref uint SUI)
            {
                if (State == true)
                {
                    SUI = Tstr;
                }
                else
                {
                    if (Fstr != null)
                    {
                        SUI = uint.Parse(Fstr);
                    }
                }
            }

            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, string Tstr, string Fstr, ref ushort SUI)
            {
                if (State == true)
                {
                    if (Tstr != null)
                    {
                        SUI = ushort.Parse(Tstr);
                    }
                }
                else
                {
                    if (Fstr != null)
                    {
                        SUI = ushort.Parse(Fstr);
                    }
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, ushort Tstr, ushort Fstr, ref ushort SUI)
            {
                if (State == true)
                {
                    SUI = Tstr;
                }
                else
                {
                    SUI = Fstr;
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, string Tstr, ushort Fstr, ref ushort SUI)
            {
                if (State == true)
                {
                    if (Tstr != null)
                    {
                        SUI = ushort.Parse(Tstr);
                    }
                }
                else
                {
                    SUI = Fstr;
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, ushort Tstr, string Fstr, ref ushort SUI)
            {
                if (State == true)
                {
                    SUI = Tstr;
                }
                else
                {
                    if (Fstr != null)
                    {
                        SUI = ushort.Parse(Fstr);
                    }
                }
            }

            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, string Tstr, string Fstr, ref short SUI)
            {
                if (State == true)
                {
                    if (Tstr != null)
                    {
                        SUI = short.Parse(Tstr);
                    }
                }
                else
                {
                    if (Fstr != null)
                    {
                        SUI = short.Parse(Fstr);
                    }
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, short Tstr, short Fstr, ref short SUI)
            {
                if (State == true)
                {
                    SUI = Tstr;
                }
                else
                {
                    SUI = Fstr;
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, string Tstr, short Fstr, ref short SUI)
            {
                if (State == true)
                {
                    if (Tstr != null)
                    {
                        SUI = short.Parse(Tstr);
                    }
                }
                else
                {
                    SUI = Fstr;
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, short Tstr, string Fstr, ref short SUI)
            {
                if (State == true)
                {
                    SUI = Tstr;
                }
                else
                {
                    if (Fstr != null)
                    {
                        SUI = short.Parse(Fstr);
                    }
                }
            }

            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, string Tstr, string Fstr, ref bool SUI)
            {
                if (State == true)
                {
                    if (Tstr != null)
                    {
                        if (Tstr == "1" || Tstr.ToLower() == "true")
                        {
                            SUI = true;
                        }
                        else if (Tstr == "0" || Tstr.ToLower() == "false")
                        {
                            SUI = false;
                        }
                    }
                }
                else
                {
                    if (Fstr != null)
                    {
                        if (Fstr == "1" || Fstr.ToLower() == "true")
                        {
                            SUI = true;
                        }
                        else if (Fstr == "0" || Fstr.ToLower() == "false")
                        {
                            SUI = false;
                        }
                    }
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, int Tstr, int Fstr, ref bool SUI)
            {
                if (State == true)
                {
                    if (Tstr == 1)
                    {
                        SUI = true;
                    }
                    else
                    {
                        SUI = false;
                    }

                }
                else
                {
                    if (Fstr == 1)
                    {
                        SUI = true;
                    }
                    else
                    {
                        SUI = false;
                    }
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, string Tstr, int Fstr, ref bool SUI)
            {
                if (State == true)
                {
                    if (Tstr != null)
                    {
                        if (Tstr == "1" || Tstr.ToLower() == "true")
                        {
                            SUI = true;
                        }
                        else if (Tstr == "0" || Tstr.ToLower() == "false")
                        {
                            SUI = false;
                        }
                    }
                }
                else
                {
                    if (Fstr == 1)
                    {
                        SUI = true;
                    }
                    else
                    {
                        SUI = false;
                    }
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, int Tstr, string Fstr, ref bool SUI)
            {
                if (State == true)
                {
                    if (Tstr == 1)
                    {
                        SUI = true;
                    }
                    else
                    {
                        SUI = false;
                    }
                }
                else
                {
                    if (Fstr != null)
                    {
                        if (Fstr == "1" || Fstr.ToLower() == "true")
                        {
                            SUI = true;
                        }
                        else if (Fstr == "0" || Fstr.ToLower() == "false")
                        {
                            SUI = false;
                        }
                    }
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, bool Tstr, bool Fstr, ref bool SUI)
            {
                if (State == true)
                {
                    SUI = Tstr;
                }
                else
                {
                    SUI = Fstr;
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, string Tstr, bool Fstr, ref bool SUI)
            {
                if (State == true)
                {
                    if (Tstr != null)
                    {
                        if (Tstr == "1" || Tstr.ToLower() == "true")
                        {
                            SUI = true;
                        }
                        else if (Tstr == "0" || Tstr.ToLower() == "false")
                        {
                            SUI = false;
                        }
                    }
                }
                else
                {
                    SUI = Fstr;
                }
            }

            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, string Tstr, string Fstr, ref Color SUI)
            {
                if (State == true)
                {
                    if (Fstr != null)
                    {
                        string data = Tstr.Substring(7, Tstr.Length - 3 - 5);
                        string[] datalist = data.Split(',');
                        if (datalist.Length > 1)
                        {
                            string[] A = datalist[0].Split('=');
                            string[] R = datalist[1].Split('=');
                            string[] G = datalist[2].Split('=');
                            string[] B = datalist[3].Split('=');

                            SUI = Color.FromArgb(byte.Parse(A[1]), byte.Parse(R[1]), byte.Parse(G[1]), byte.Parse(B[1]));
                        }
                        else
                        {
                            SUI = System.Drawing.ColorTranslator.FromHtml(Tstr);
                        }
                    }
                }
                else
                {
                    if (Fstr != null)
                    {
                        string data = Fstr.Substring(7, Fstr.Length - 3 - 5);
                        string[] datalist = data.Split(',');
                        if (datalist.Length > 1)
                        {
                            string[] A = datalist[0].Split('=');
                            string[] R = datalist[1].Split('=');
                            string[] G = datalist[2].Split('=');
                            string[] B = datalist[3].Split('=');

                            SUI = Color.FromArgb(byte.Parse(A[1]), byte.Parse(R[1]), byte.Parse(G[1]), byte.Parse(B[1]));
                        }
                        else
                        {
                            SUI = System.Drawing.ColorTranslator.FromHtml(Fstr);
                        }
                    }
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, Color Tstr, string Fstr, ref Color SUI)
            {
                if (State == true)
                {
                    if (Fstr != null)
                    {
                        SUI = Tstr;
                    }
                }
                else
                {
                    if (Fstr != null)
                    {
                        string data = Fstr.Substring(7, Fstr.Length - 3 - 5);
                        string[] datalist = data.Split(',');
                        if (datalist.Length > 1)
                        {
                            string[] A = datalist[0].Split('=');
                            string[] R = datalist[1].Split('=');
                            string[] G = datalist[2].Split('=');
                            string[] B = datalist[3].Split('=');

                            SUI = Color.FromArgb(byte.Parse(A[1]), byte.Parse(R[1]), byte.Parse(G[1]), byte.Parse(B[1]));
                        }
                        else
                        {
                            SUI = System.Drawing.ColorTranslator.FromHtml(Fstr);
                        }
                    }
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, string Tstr, Color Fstr, ref Color SUI)
            {
                if (State == true)
                {
                    if (Fstr != null)
                    {
                        string data = Tstr.Substring(7, Tstr.Length - 3 - 5);
                        string[] datalist = data.Split(',');
                        if (datalist.Length > 1)
                        {
                            string[] A = datalist[0].Split('=');
                            string[] R = datalist[1].Split('=');
                            string[] G = datalist[2].Split('=');
                            string[] B = datalist[3].Split('=');

                            SUI = Color.FromArgb(byte.Parse(A[1]), byte.Parse(R[1]), byte.Parse(G[1]), byte.Parse(B[1]));
                        }
                        else
                        {
                            SUI = System.Drawing.ColorTranslator.FromHtml(Tstr);
                        }
                    }
                }
                else
                {
                    if (Fstr != null)
                    {
                        SUI = Fstr;
                    }
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, Color Tstr, Color Fstr, ref Color SUI)
            {
                if (State == true)
                {
                    if (Fstr != null)
                    {
                        SUI = Tstr;
                    }
                }
                else
                {
                    if (Fstr != null)
                    {
                        SUI = Fstr;
                    }
                }
            }

            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, string Tstr, string Fstr, ref System.Windows.Forms.DataVisualization.Charting.SeriesChartType SUI)
            {
                if (State == true)
                {
                    if (Fstr != null)
                    {
                        SUI = StringToEnum(Tstr);
                    }
                }
                else
                {
                    if (Fstr != null)
                    {
                        SUI = StringToEnum(Fstr);
                    }
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, System.Windows.Forms.DataVisualization.Charting.SeriesChartType Tstr, string Fstr, ref System.Windows.Forms.DataVisualization.Charting.SeriesChartType SUI)
            {
                if (State == true)
                {
                    if (Fstr != null)
                    {
                        SUI = Tstr;
                    }
                }
                else
                {
                    if (Fstr != null)
                    {
                        SUI = StringToEnum(Fstr);
                    }
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, string Tstr, System.Windows.Forms.DataVisualization.Charting.SeriesChartType Fstr, ref System.Windows.Forms.DataVisualization.Charting.SeriesChartType SUI)
            {
                if (State == true)
                {
                    SUI = StringToEnum(Tstr);
                }
                else
                {
                    SUI = Fstr;
                }
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            /// <param name="SUI">需要设定的变量</param> 
            public static void SetVable(bool State, System.Windows.Forms.DataVisualization.Charting.SeriesChartType Tstr, System.Windows.Forms.DataVisualization.Charting.SeriesChartType Fstr, ref System.Windows.Forms.DataVisualization.Charting.SeriesChartType SUI)
            {
                if (State == true)
                {
                    SUI = Tstr;
                }
                else
                {
                    SUI = Fstr;
                }
            }
            
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            public static string SetVable(bool State, string Tstr, string Fstr)
            {
                string SUI = "";
                if (State == true)
                {
                    if (Tstr != null)
                    {
                        SUI = Tstr;
                    }
                }
                else
                {
                    if (Fstr != null)
                    {
                        SUI = Fstr;
                    }
                }
                return SUI;
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            public static float SetVable(bool State, float Tstr, float Fstr)
            {
                float SUI = 0;
                if (State == true)
                {
                    SUI = Tstr;
                }
                else
                {
                    SUI = Fstr;
                }
                return SUI;
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            public static double SetVable(bool State, double Tstr, double Fstr)
            {
                double SUI = 0;
                if (State == true)
                {
                    SUI = Tstr;
                }
                else
                {
                    SUI = Fstr;
                }
                return SUI;
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            public static int SetVable(bool State, int Tstr, int Fstr)
            {
                int SUI = 0;
                if (State == true)
                {
                    SUI = Tstr;
                }
                else
                {
                    SUI = Fstr;
                }
                return SUI;
            }
            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            public static bool SetVable(bool State, bool Tstr, bool Fstr)
            {
                bool SUI = false;
                if (State == true)
                {
                    SUI = Tstr;
                }
                else
                {
                    SUI = Fstr;
                }
                return SUI;
            }

            /// <summary> 
            /// 设定变量数据,包含字符串返回值，整型返回值，浮点返回值,布尔返回值等
            /// </summary> 
            /// <param name="State">状态</param> 
            /// <param name="Tstr">当State = True 时的数据</param> 
            /// <param name="Fstr">当State = Flase 时的数据</param> 
            public static Color SetVable(bool State, Color Tstr, Color Fstr)
            {
                Color SUI = Color.Transparent;
                if (State == true)
                {
                    if (Fstr != null)
                    {
                        SUI = Tstr;
                    }
                }
                else
                {
                    if (Fstr != null)
                    {
                        SUI = Fstr;
                    }
                }
                return SUI;
            }

            /// <summary> 
            /// 单选按钮转化为Int整型，如果按钮未进行选择返回-1，其它按顺序返回0-N
            /// </summary> 
            /// <param name="a">传入的单选按钮1</param> 
            /// <param name="b">传入的单选按钮2</param> 
            public static int Radio_To_Int(RadioButton a, RadioButton b)
            {
                if (a.Checked == true)
                {
                    return 0;
                }
                else if (b.Checked == true)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            /// <summary> 
            /// 单选按钮转化为Int整型，如果按钮未进行选择返回-1，其它按顺序返回0-N
            /// </summary> 
            /// <param name="a">传入的单选按钮</param> 
            /// <param name="b">传入的单选按钮</param> 
            /// <param name="c">传入的单选按钮</param> 
            public static int Radio_To_Int(RadioButton a, RadioButton b, RadioButton c)
            {
                if (a.Checked == true)
                {
                    return 0;
                }
                else if (b.Checked == true)
                {
                    return 1;
                }
                else if (c.Checked == true)
                {
                    return 2;
                }
                else
                {
                    return -1;
                }

            }
            /// <summary> 
            /// 单选按钮转化为Int整型，如果按钮未进行选择返回-1，其它按顺序返回0-N
            /// </summary> 
            /// <param name="a">传入的单选按钮1</param> 
            /// <param name="b">传入的单选按钮2</param> 
            /// <param name="c">传入的单选按钮3</param> 
            /// <param name="d">传入的单选按钮4</param> 
            public static int Radio_To_Int(RadioButton a, RadioButton b, RadioButton c, RadioButton d)
            {
                if (a.Checked == true)
                {
                    return 0;
                }
                else if (b.Checked == true)
                {
                    return 1;
                }
                else if (c.Checked == true)
                {
                    return 2;
                }
                else if (d.Checked == true)
                {
                    return 3;
                }
                else
                {
                    return -1;
                }

            }
            /// <summary> 
            /// 单选按钮转化为Int整型，如果按钮未进行选择返回-1，其它按顺序返回0-N
            /// </summary> 
            /// <param name="a">传入的单选按钮1</param> 
            /// <param name="b">传入的单选按钮2</param> 
            /// <param name="c">传入的单选按钮3</param> 
            /// <param name="d">传入的单选按钮4</param> 
            /// <param name="e">传入的单选按钮5</param> 
            public static int Radio_To_Int(RadioButton a, RadioButton b, RadioButton c, RadioButton d, RadioButton e)
            {
                if (a.Checked == true)
                {
                    return 0;
                }
                else if (b.Checked == true)
                {
                    return 1;
                }
                else if (c.Checked == true)
                {
                    return 2;
                }
                else if (d.Checked == true)
                {
                    return 3;
                }
                else if (e.Checked == true)
                {
                    return 4;
                }
                else
                {
                    return -1;
                }

            }

            /// <summary> 
            /// 单选按钮转化为Int整型，如果按钮未进行选择返回-1，其它按顺序返回0-N
            /// </summary> 
            /// <param name="a">传入的单选按钮1</param> 
            /// <param name="b">传入的单选按钮2</param> 
            public static short Radio_To_Short(RadioButton a, RadioButton b)
            {
                if (a.Checked == true)
                {
                    return 0;
                }
                else if (b.Checked == true)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            /// <summary> 
            /// 单选按钮转化为Int整型，如果按钮未进行选择返回-1，其它按顺序返回0-N
            /// </summary> 
            /// <param name="a">传入的单选按钮</param> 
            /// <param name="b">传入的单选按钮</param> 
            /// <param name="c">传入的单选按钮</param> 
            public static short Radio_To_Short(RadioButton a, RadioButton b, RadioButton c)
            {
                if (a.Checked == true)
                {
                    return 0;
                }
                else if (b.Checked == true)
                {
                    return 1;
                }
                else if (c.Checked == true)
                {
                    return 2;
                }
                else
                {
                    return -1;
                }

            }
            /// <summary> 
            /// 单选按钮转化为Int整型，如果按钮未进行选择返回-1，其它按顺序返回0-N
            /// </summary> 
            /// <param name="a">传入的单选按钮1</param> 
            /// <param name="b">传入的单选按钮2</param> 
            /// <param name="c">传入的单选按钮3</param> 
            /// <param name="d">传入的单选按钮4</param> 
            public static short Radio_To_Short(RadioButton a, RadioButton b, RadioButton c, RadioButton d)
            {
                if (a.Checked == true)
                {
                    return 0;
                }
                else if (b.Checked == true)
                {
                    return 1;
                }
                else if (c.Checked == true)
                {
                    return 2;
                }
                else if (d.Checked == true)
                {
                    return 3;
                }
                else
                {
                    return -1;
                }

            }
            /// <summary> 
            /// 单选按钮转化为Int整型，如果按钮未进行选择返回-1，其它按顺序返回0-N
            /// </summary> 
            /// <param name="a">传入的单选按钮1</param> 
            /// <param name="b">传入的单选按钮2</param> 
            /// <param name="c">传入的单选按钮3</param> 
            /// <param name="d">传入的单选按钮4</param> 
            /// <param name="e">传入的单选按钮5</param> 
            public static short Radio_To_Short(RadioButton a, RadioButton b, RadioButton c, RadioButton d, RadioButton e)
            {
                if (a.Checked == true)
                {
                    return 0;
                }
                else if (b.Checked == true)
                {
                    return 1;
                }
                else if (c.Checked == true)
                {
                    return 2;
                }
                else if (d.Checked == true)
                {
                    return 3;
                }
                else if (e.Checked == true)
                {
                    return 4;
                }
                else
                {
                    return -1;
                }

            }

            /// <summary> 
            /// Int整型转单选按钮
            /// </summary> 
            /// <param name="num">单选按钮选择的序号</param> 
            /// <param name="a">当num参数为1时单选按钮选择当前，需要使用ref调用传值</param> 
            /// <param name="b">当num参数为2时单选按钮选择当前，需要使用ref调用传值</param> 
            public static void Int_To_Radio(int num, ref RadioButton a, ref RadioButton b)
            {
                if (num == 0)
                {
                    a.Checked = true;
                    b.Checked = false;
                }
                else if (num == 1)
                {
                    a.Checked = false;
                    b.Checked = true;
                }
                else
                {
                    a.Checked = false;
                    b.Checked = false;
                }
            }
            /// <summary> 
            /// Int整型转单选按钮
            /// </summary> 
            /// <param name="num">单选按钮选择的序号</param> 
            /// <param name="a">当num参数为1时单选按钮选择当前，需要使用ref调用传值</param> 
            /// <param name="b">当num参数为2时单选按钮选择当前，需要使用ref调用传值</param> 
            /// <param name="c">当num参数为3时单选按钮选择当前，需要使用ref调用传值</param> 
            public static void Int_To_Radio(int num, ref RadioButton a, ref RadioButton b, ref RadioButton c)
            {
                if (num == 0)
                {
                    a.Checked = true;
                    b.Checked = false;
                    c.Checked = false;
                }
                else if (num == 1)
                {
                    a.Checked = false;
                    b.Checked = true;
                    c.Checked = false;
                }
                else if (num == 2)
                {
                    a.Checked = false;
                    b.Checked = false;
                    c.Checked = true;
                }
                else
                {
                    a.Checked = false;
                    b.Checked = false;
                    c.Checked = false;
                }
            }
            /// <summary> 
            /// Int整型转单选按钮
            /// </summary> 
            /// <param name="num">单选按钮选择的序号</param> 
            /// <param name="a">当num参数为1时单选按钮选择当前，需要使用ref调用传值</param> 
            /// <param name="b">当num参数为2时单选按钮选择当前，需要使用ref调用传值</param> 
            /// <param name="c">当num参数为3时单选按钮选择当前，需要使用ref调用传值</param> 
            /// <param name="d">当num参数为4时单选按钮选择当前，需要使用ref调用传值</param> 
            public static void Int_To_Radio(int num, ref RadioButton a, ref RadioButton b, ref RadioButton c, ref RadioButton d)
            {
                if (num == 0)
                {
                    a.Checked = true;
                    b.Checked = false;
                    c.Checked = false;
                    d.Checked = false;
                }
                else if (num == 1)
                {
                    a.Checked = false;
                    b.Checked = true;
                    c.Checked = false;
                    d.Checked = false;
                }
                else if (num == 2)
                {
                    a.Checked = false;
                    b.Checked = false;
                    c.Checked = true;
                    d.Checked = false;
                }
                else if (num == 3)
                {
                    a.Checked = false;
                    b.Checked = false;
                    c.Checked = false;
                    d.Checked = true;
                }
                else
                {
                    a.Checked = false;
                    b.Checked = false;
                    c.Checked = false;
                    d.Checked = false;
                }
            }

            /// <summary> 
            /// Bool转化为Int整型，返回值为1或0
            /// </summary> 
            /// <param name="a">需要转化的布尔值</param> 
            public static int Bool_To_Int(bool a)
            {
                if (a == true)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            /// <summary> 
            /// String字符串转化为Int整型，返回为1或0
            /// </summary> 
            /// <param name="a">需要转化的字符串</param> 
            public static int Bool_To_Int(string a)
            {
                a = a.ToLower();
                if (a == "true" || a == "1")
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            /// <summary> 
            /// Int字符串转化为Bool
            /// </summary> 
            /// <param name="a">需要转化的整型值</param> 
            public static bool Int_To_Bool(int a)
            {
                if (a == 0)
                {
                    return false;
                }
                else if (a == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            /// <summary> 
            /// Int字符串转化为Bool
            /// </summary> 
            /// <param name="a">需要转化的整型值</param> 
            public static bool Int_To_Bool(float a)
            {
                if (a == 0)
                {
                    return false;
                }
                else if (a == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            /// <summary> 
            /// Int字符串转化为Bool
            /// </summary> 
            /// <param name="a">需要转化的整型值</param> 
            public static bool Int_To_Bool(double a)
            {
                if (a == 0)
                {
                    return false;
                }
                else if (a == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            /// <summary> 
            /// String字符串转化为Bool
            /// </summary> 
            /// <param name="a">需要转化的字符串</param> 
            public static bool Int_To_Bool(string a)
            {
                a = a.ToLower();
                if (a == "true" || a == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            /// <summary> 
            /// 将十进制转化为二进制，然后判断右侧向左的某一位是1还是0，1为true，0为false
            /// </summary> 
            /// <param name="num">需要转化的数</param> 
            /// <param name="Bit">获取的位置</param> 
            public static bool Int_To_Bit(int num, int Bit)
            {
                string data = Convert.ToString(num, 2).PadLeft(16, '0'); ;
                if (data.Substring(data.Length - Bit, 1) == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            /// <summary> 
            /// 将十进制转化为二进制，然后判断右侧向左的某一位是1还是0，1为true，0为false
            /// </summary> 
            /// <param name="num">需要转化的数</param> 
            /// <param name="Bit">获取的位置</param> 
            public static bool Int_To_Bit(double num, int Bit)
            {
                string data = Convert.ToString((int)num, 2).PadLeft(16, '0');
                if (data.Substring(data.Length - Bit, 1) == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary> 
            /// 消息确认框
            /// </summary> 
            /// <param name="mes">消息框中叙述的文字</param> 
            public static DialogResult Message(string mes)
            {
                DialogResult Result = MessageBox.Show(mes, "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return Result;
            }
            /// <summary> 
            /// 消息确认框
            /// </summary> 
            /// <param name="mes">消息框中叙述的文字</param> 
            /// <param name="tip">标题栏的文字</param> 
            public static DialogResult Message(string mes, string tip)
            {
                DialogResult Result = MessageBox.Show(mes, tip, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return Result;
            }
            /// <summary> 
            /// 消息确认框
            /// </summary> 
            /// <param name="mes">消息框中叙述的文字</param> 
            /// <param name="tip">标题栏的文字</param> 
            /// <param name="Symbol">消息框中的图形符号</param> 
            public static DialogResult Message(string mes, string tip, MessageBoxIcon Symbol)
            {
                DialogResult Result = MessageBox.Show(mes, tip, MessageBoxButtons.OK, Symbol, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return Result;
            }
            /// <summary> 
            /// 消息确认框
            /// </summary> 
            /// <param name="mes">消息框中叙述的文字</param> 
            /// <param name="tip">标题栏的文字</param> 
            /// <param name="But">消息框中拥有的按钮</param> 
            public static DialogResult Message(string mes, string tip, MessageBoxButtons But)
            {
                DialogResult Result = MessageBox.Show(mes, tip, But, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return Result;
            }
            /// <summary> 
            /// 消息确认框
            /// </summary> 
            /// <param name="mes">消息框中叙述的文字</param> 
            /// <param name="tip">标题栏的文字</param> 
            /// <param name="But">消息框中拥有的按钮</param> 
            /// <param name="Symbol">消息框中的图形符号</param> 
            public static DialogResult Message(string mes, string tip, MessageBoxButtons But, MessageBoxIcon Symbol)
            {
                DialogResult Result = MessageBox.Show(mes, tip, But, Symbol, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return Result;
            }

            /// <summary> 
            /// 判断是否为纯数字 
            /// </summary> 
            /// <param name="str">需要判断的字符串</param> 
            public static bool IsInt(string str) //接收一个string类型的参数,保存到str里
            {
                if (str == null || str.Length == 0)    //验证这个参数是否为空
                {
                    return false;                           //是，就返回False
                }
                else
                {
                    try
                    {
                        int.Parse(str);

                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            /// <summary> 
            /// 判断是否为纯数字 
            /// </summary> 
            /// <param name="str">需要判断的字符串</param> 
            public static bool IsShort(string str) //接收一个string类型的参数,保存到str里
            {
                if (str == null || str.Length == 0)    //验证这个参数是否为空
                {
                    return false;                           //是，就返回False
                }
                else
                {
                    try
                    {
                        short.Parse(str);

                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            /// <summary> 
            /// 判断是否为纯数字 
            /// </summary> 
            /// <param name="str">需要判断的字符串</param> 
            public static bool IsUint(string str) //接收一个string类型的参数,保存到str里
            {
                if (str == null || str.Length == 0)    //验证这个参数是否为空
                {
                    return false;                           //是，就返回False
                }
                else
                {
                    try
                    {
                        uint.Parse(str);

                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            /// <summary> 
            /// 判断是否为纯数字 
            /// </summary> 
            /// <param name="str">需要判断的字符串</param> 
            public static bool IsUShort(string str) //接收一个string类型的参数,保存到str里
            {
                if (str == null || str.Length == 0)    //验证这个参数是否为空
                {
                    return false;                           //是，就返回False
                }
                else
                {
                    try
                    {
                        ushort.Parse(str);

                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            /// <summary> 
            /// 判断是否为空或者为null 
            /// </summary> 
            /// <param name="str">需要判断的字符串</param> 
            public static bool IsString(string str) //接收一个string类型的参数,保存到str里
            {
                if (str == null || str.Length == 0 || str.ToLower() == "err")    //验证这个参数是否为空
                {
                    return false;                           //是，就返回False
                }
                else
                {
                    return true;                                        //是，就返回True
                }
            }

            /// <summary> 
            /// 判断字符串是否为浮点数
            /// </summary> 
            /// <param name="str">需要判断的字符串</param> 
            public static bool IsFloat(string str)
            {
                if (str != null)
                {
                    string regextext = @"^(-?\d+)(\.\d+)?$";
                    Regex regex = new Regex(regextext, RegexOptions.None);
                    return regex.IsMatch(str.Trim());
                }
                else
                {
                    return false;
                }
            }

            /// <summary> 
            /// 判断字符串是否为浮点数
            /// </summary> 
            /// <param name="str">需要判断的字符串</param> 
            public static bool IsDouble(string str)
            {
                if (str != null)
                {
                    string regextext = @"^(-?\d+)(\.\d+)?$";
                    Regex regex = new Regex(regextext, RegexOptions.None);
                    return regex.IsMatch(str.Trim());
                }
                else
                {
                    return false;
                }
            }
            /// <summary> 
            /// 判断字符串是否为布尔值
            /// </summary> 
            /// <param name="str">需要判断的字符串</param> 
            public static bool IsBool(string str)
            {
                if (str != null)
                {
                    if (str.ToLower() == "true" || str.ToLower() == "false" || str == "0" || str == "1")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            /// <summary> 
            /// 判断字符串是否为颜色值
            /// </summary> 
            /// <param name="str">需要判断的字符串</param> 
            public static bool IsColor(string str)
            {
                try
                {
                    Color NowColor;

                    if (str.Substring(0, 5).ToLower() == "Color".ToLower())
                    {
                        string data = str.Substring(7, str.Length - 3 - 5);
                        string[] datalist = data.Split(',');
                        if (datalist.Length > 1)
                        {
                            string[] A = datalist[0].Split('=');
                            string[] R = datalist[1].Split('=');
                            string[] G = datalist[2].Split('=');
                            string[] B = datalist[3].Split('=');

                             NowColor = Color.FromArgb(byte.Parse(A[1]), byte.Parse(R[1]), byte.Parse(G[1]), byte.Parse(B[1]));
                        }
                        else
                        {
                            NowColor = System.Drawing.ColorTranslator.FromHtml(str);
                        }

                        if (NowColor.GetType() == typeof(Color))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }

            /// <summary> 
            /// 判断字符串是否为曲线类型
            /// </summary> 
            /// <param name="str">需要判断的字符串</param> 
            public static bool IsSeriesChartType(string str)
            {
                bool Ret = false;
                try
                {
                    foreach (System.Windows.Forms.DataVisualization.Charting.SeriesChartType item in Enum.GetValues(typeof(System.Windows.Forms.DataVisualization.Charting.SeriesChartType)))
                    {
                        if (item.ToString().ToLower() == str.ToLower())
                        {
                            Ret = true;
                            break;
                        }
                    }
                }
                catch
                {
                    Ret = false;
                }
                return Ret;
            }

            /// <summary>
            /// 判断第二个字符串数据的类型是否与第一个变量类型一致
            /// </summary>
            /// <param name="data">变量类型</param>
            /// <param name="str">字符串类型</param>
            /// <returns>是或否</returns>
            public static bool IsIdentical(object data, string str)
            {
                bool ret = false;

                if (data != null && str != null)
                {
                    if (data.GetType() == typeof(string))
                    {
                        ret = IsString(str);
                    }
                    else if (data.GetType() == typeof(short))
                    {
                        ret = IsShort(str);
                    }
                    else if (data.GetType() == typeof(ushort))
                    {
                        ret = IsUShort(str);
                    }
                    else if (data.GetType() == typeof(int))
                    {
                        ret = IsInt(str);
                    }
                    else if (data.GetType() == typeof(uint))
                    {
                        ret = IsUint(str);
                    }
                    else if (data.GetType() == typeof(float))
                    {
                        ret = IsFloat(str);
                    }
                    else if (data.GetType() == typeof(double))
                    {
                        ret = IsDouble(str);
                    }
                    else if (data.GetType() == typeof(bool))
                    {
                        ret = IsBool(str);
                    }
                    else if (data.GetType() == typeof(Color))
                    {
                        ret = IsColor(str);
                    }
                    else if (data.GetType() == typeof(System.Windows.Forms.DataVisualization.Charting.SeriesChartType))
                    {
                        ret = IsSeriesChartType(str);
                    }
                }
                else
                {
                    ret = false;
                }
                return ret;
            }


            /// <summary> 
            /// 使用字符串转换为控件，返回为Control，需转换为对应控件类型。
            /// <param>Label lab = col as Label; //转为Label</param> 
            /// </summary> 
            /// <param name="box">需要查找的控件父层</param> 
            /// <param name="str">需要判断的字符串</param> 
            public static Control FindControl(Control box, string name)
            {
                name = name.Replace(" ", "");
                Control col = new Control();
                try
                {
                    col = box.Controls.Find(name, true)[0];
                }
                catch
                {
                    col = null;
                }
                return col;
            }

            /// <summary>
            /// 在一个范围内生产出指定列表外的随机数
            /// </summary>
            /// <param name="MinNum">最小数</param>
            /// <param name="MaxNum">最大数</param>
            /// <param name="List">当前已经存在的列表</param>
            /// <returns></returns>
            public static int RandomNumber(int MinNum, int MaxNum, ArrayList List)
            {
                int Num = 0;
                Random Ran = new Random();
                if (MaxNum - MinNum != List.Count)
                {
                    do
                    {
                        Thread.Sleep(1);
                        Num = Ran.Next(MinNum, MaxNum);
                    }
                    while (List.Contains(Num));
                }
                return Num;
            }

            /// <summary> 
            ///  计算两点温度补偿系数
            /// </summary> 
            /// <param name="One">第一点温度补偿（温度，补偿系数）</param> 
            /// <param name="Two">第二点温度补偿（温度，补偿系数）</param> 
            public static float CalculationTempOffset(PointF One, PointF Two, double SetTemp)
            {
                double k = ((double)(One.Y - Two.Y)) / (One.X - Two.X);             // 计算斜率
                if (!double.IsNaN(k) && !double.IsInfinity(k))
                {
                    double y = k * (SetTemp - One.X) + One.Y;                           // 根据斜率，计算y坐标
                    return float.Parse(y.ToString("0.0"));
                }
                else
                {
                    return 0;
                }
            }

            /// <summary>
            /// 将字符串转换为枚举 Enem
            /// </summary>
            /// <param name="str">枚举字符串</param>
            /// <returns>枚举</returns>
            public static System.Windows.Forms.DataVisualization.Charting.SeriesChartType StringToEnum(string str)
            {
                System.Windows.Forms.DataVisualization.Charting.SeriesChartType ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                try
                {
                    ChartType = (System.Windows.Forms.DataVisualization.Charting.SeriesChartType)Enum.Parse(typeof(System.Windows.Forms.DataVisualization.Charting.SeriesChartType), str);
                }
                catch
                {
                    return ChartType;
                }

                return ChartType;
            }
            /// <summary>
            /// 将枚举转换为数值
            /// </summary>
            /// <param name="ChartType">枚举</param>
            /// <returns>数值</returns>
            public static int EnumToInt(System.Windows.Forms.DataVisualization.Charting.SeriesChartType ChartType)
            {
                return (int)ChartType;
            }
            /// <summary>
            /// 将枚举转换为字符串
            /// </summary>
            /// <param name="ChartType">枚举</param>
            /// <returns>字符串</returns>
            public static string EnumToString(System.Windows.Forms.DataVisualization.Charting.SeriesChartType ChartType)
            {
                return ChartType.ToString();
            }

            #region 格式化时间
            [DllImport("kernel32.dll", EntryPoint = "GetSystemDefaultLCID")]
            public static extern int GetSystemDefaultLCID();
            [DllImport("kernel32.dll", EntryPoint = "SetLocaleInfoA")]
            public static extern int SetLocaleInfo(int Locale, int LCType, string lpLCData);
            public const int LOCALE_SLONGDATE = 0x20;
            public const int LOCALE_SSHORTDATE = 0x1F;
            public const int LOCALE_STIME = 0x1003;
            /// <summary> 
            /// 格式化当前系统时间 
            /// </summary> 
            public static string SetDateTimeFormat()
            {
                try
                {
                    int x = GetSystemDefaultLCID();
                    SetLocaleInfo(x, LOCALE_STIME, "HH:mm:ss");          //时间格式  
                    SetLocaleInfo(x, LOCALE_SSHORTDATE, "yyyy-MM-dd");   //短日期格式    
                    SetLocaleInfo(x, LOCALE_SLONGDATE, "yyyy-MM-dd");    //长日期格式 
                    return "";
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            #endregion
        }

        public class Hook
        {
            //定义客户端键盘处理事件 
            public event KeyEventHandler KeyDown;                 //按键被按下检测
            public event KeyPressEventHandler KeyPress;           //按键被按下中检测
            public event KeyEventHandler KeyUp;                   //按键被抬起检测
            public event HookProcessKey HandleKey;                //按键是否拦截判断

            /// <summary>
            /// 接收键盘钩子返回状态
            /// </summary>
            private IntPtr KeyBoaradHookValue = IntPtr.Zero;
            /// <summary>
            /// 接收鼠标钩子返回状态
            /// </summary>
            private IntPtr MouseHookValue = IntPtr.Zero;

            // 添加
            private const byte VK_SHIFT = 0x10;
            private const byte VK_CAPITAL = 0x14;
            private const byte VK_NUMLOCK = 0x90;
            /// <summary>
            /// 键盘钩子委托实例
            /// </summary>
            /// <remarks>
            /// 不要试图省略此变量,否则将会导致
            /// 激活 CallbackOnCollectedDelegate 托管调试助手 (MDA)。 
            /// 详细请参见MSDN中关于 CallbackOnCollectedDelegate 的描述
            /// </remarks>
            private HookProc m_KeyboardHookProcedure;

            #region 委托定义

            /// <summary>
            /// 钩子委托声明,不可使用
            /// </summary>
            /// <param name="nCode"></param>
            /// <param name="wParam"></param>
            /// <param name="lParam"></param>
            /// <returns></returns>
            public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
            /// <summary>
            /// 按键屏蔽委托声明,不可使用
            /// </summary>
            /// <param name="Param">由Hook程序发送的按键信息</param>
            /// <param name="handle">是否屏蔽该按键</param>
            /// <returns></returns>
            public delegate void HookProcessKey(KeyboardHookStruct Param, out bool handle);
            /// <summary>
            /// 鼠标更新事件委托声明,不可使用
            /// </summary>
            /// <param name="x">x坐标</param>
            /// <param name="y">y坐标</param>
            public delegate void MouseUpdateEventHandler(int x, int y);

            /// <summary>
            /// 无返回委托声明,不可使用
            /// </summary>
            public delegate void VoidCallback();

            #endregion 委托定义

            #region 枚举定义

            private enum WH_Codes : int
            {
                /// <summary>
                /// 底层键盘钩子
                /// </summary>
                WH_KEYBOARD_LL = 13,

                /// <summary>
                /// 底层鼠标钩子
                /// </summary>
                WH_MOUSE_LL = 14
            }

            private enum WM_MOUSE : int
            {
                /// <summary>
                /// 鼠标开始
                /// </summary>
                WM_MOUSEFIRST = 0x200,

                /// <summary>
                /// 鼠标移动
                /// </summary>
                WM_MOUSEMOVE = 0x200,

                /// <summary>
                /// 左键按下
                /// </summary>
                WM_LBUTTONDOWN = 0x201,

                /// <summary>
                /// 左键释放
                /// </summary>
                WM_LBUTTONUP = 0x202,

                /// <summary>
                /// 左键双击
                /// </summary>
                WM_LBUTTONDBLCLK = 0x203,

                /// <summary>
                /// 右键按下
                /// </summary>
                WM_RBUTTONDOWN = 0x204,

                /// <summary>
                /// 右键释放
                /// </summary>
                WM_RBUTTONUP = 0x205,

                /// <summary>
                /// 右键双击
                /// </summary>
                WM_RBUTTONDBLCLK = 0x206,

                /// <summary>
                /// 中键按下
                /// </summary>
                WM_MBUTTONDOWN = 0x207,

                /// <summary>
                /// 中键释放
                /// </summary>
                WM_MBUTTONUP = 0x208,

                /// <summary>
                /// 中键双击
                /// </summary>
                WM_MBUTTONDBLCLK = 0x209,

                /// <summary>
                /// 滚轮滚动
                /// </summary>
                /// <remarks>WINNT4.0以上才支持此消息</remarks>
                WM_MOUSEWHEEL = 0x020A
            }

            private enum WM_KEYBOARD : int
            {
                /// <summary>
                /// 非系统按键按下
                /// </summary>
                WM_KEYDOWN = 0x100,

                /// <summary>
                /// 非系统按键释放
                /// </summary>
                WM_KEYUP = 0x101,

                /// <summary>
                /// 系统按键按下
                /// </summary>
                WM_SYSKEYDOWN = 0x104,

                /// <summary>
                /// 系统按键释放
                /// </summary>
                WM_SYSKEYUP = 0x105
            }

            /// <summary>
            /// SetWindowPos标志位枚举
            /// </summary>
            /// <remarks>详细说明,请参见MSDN中关于SetWindowPos函数的描述</remarks>
            private enum SetWindowPosFlags : int
            {
                /// <summary>
                /// 
                /// </summary>
                SWP_NOSIZE = 0x0001,

                /// <summary>
                /// 
                /// </summary>
                SWP_NOMOVE = 0x0002,

                /// <summary>
                /// 
                /// </summary>
                SWP_NOZORDER = 0x0004,

                /// <summary>
                /// 
                /// </summary>
                SWP_NOREDRAW = 0x0008,

                /// <summary>
                /// 
                /// </summary>
                SWP_NOACTIVATE = 0x0010,

                /// <summary>
                /// 
                /// </summary>
                SWP_FRAMECHANGED = 0x0020,

                /// <summary>
                /// 
                /// </summary>
                SWP_SHOWWINDOW = 0x0040,

                /// <summary>
                /// 
                /// </summary>
                SWP_HIDEWINDOW = 0x0080,

                /// <summary>
                /// 
                /// </summary>
                SWP_NOCOPYBITS = 0x0100,

                /// <summary>
                /// 
                /// </summary>
                SWP_NOOWNERZORDER = 0x0200,

                /// <summary>
                /// 
                /// </summary>
                SWP_NOSENDCHANGING = 0x0400,

                /// <summary>
                /// 
                /// </summary>
                SWP_DRAWFRAME = 0x0020,

                /// <summary>
                /// 
                /// </summary>
                SWP_NOREPOSITION = 0x0200,

                /// <summary>
                /// 
                /// </summary>
                SWP_DEFERERASE = 0x2000,

                /// <summary>
                /// 
                /// </summary>
                SWP_ASYNCWINDOWPOS = 0x4000

            }

            #endregion 枚举定义

            #region 结构定义

            [StructLayout(LayoutKind.Sequential)]
            public struct POINT
            {
                public int X;
                public int Y;
            }

            /// <summary>
            /// 鼠标钩子事件结构定义
            /// </summary>
            /// <remarks>详细说明请参考MSDN中关于 MSLLHOOKSTRUCT 的说明</remarks>
            [StructLayout(LayoutKind.Sequential)]
            public struct MouseHookStruct
            {
                /// <summary>
                /// Specifies a POINT structure that contains the x- and y-coordinates of the cursor, in screen coordinates.
                /// </summary>
                public POINT Point;
                public UInt32 MouseData;
                public UInt32 Flags;
                public UInt32 Time;
                public UInt32 ExtraInfo;
            }

            /// <summary>
            /// 键盘钩子事件结构定义
            /// </summary>
            /// <remarks>详细说明请参考MSDN中关于 KBDLLHOOKSTRUCT 的说明</remarks>
            [StructLayout(LayoutKind.Sequential)]
            public struct KeyboardHookStruct
            {
                /// <summary>
                /// Specifies a virtual-key code. The code must be a value in the range 1 to 254. 
                /// </summary>
                public UInt32 VKCode;

                /// <summary>
                /// Specifies a hardware scan code for the key.
                /// </summary>
                public UInt32 ScanCode;

                /// <summary>
                /// Specifies the extended-key flag, event-injected flag, context code, 
                /// and transition-state flag. This member is specified as follows. 
                /// An application can use the following values to test the keystroke flags. 
                /// </summary>
                public UInt32 Flags;

                /// <summary>
                /// Specifies the time stamp for this message. 
                /// </summary>
                public UInt32 Time;

                /// <summary>
                /// Specifies extra information associated with the message. 
                /// </summary>
                public UInt32 ExtraInfo;
            }

            #endregion 结构定义

            #region DLL导入

            /// <summary>
            /// 用于设置窗口
            /// </summary>
            /// <param name="hWnd"></param>
            /// <param name="hWndInsertAfter"></param>
            /// <param name="X"></param>
            /// <param name="Y"></param>
            /// <param name="cx"></param>
            /// <param name="cy"></param>
            /// <param name="uFlags"></param>
            /// <returns></returns>
            [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
            private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

            /// <summary>
            /// 安装钩子
            /// </summary>
            /// <param name="idHook"></param>
            /// <param name="lpfn"></param>
            /// <param name="hInstance"></param>
            /// <param name="threadId"></param>
            /// <returns></returns>
            [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
            private static extern IntPtr SetWindowsHookEx(WH_Codes idHook, HookProc lpfn, IntPtr pInstance, int threadId);

            /// <summary>
            /// 卸载钩子
            /// </summary>
            /// <param name="idHook"></param>
            /// <returns></returns>
            [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
            private static extern bool UnhookWindowsHookEx(IntPtr pHookHandle);

            /// <summary>
            /// 传递钩子
            /// </summary>
            /// <param name="idHook"></param>
            /// <param name="nCode"></param>
            /// <param name="wParam"></param>
            /// <param name="lParam"></param>
            /// <returns></returns>
            [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
            private static extern int CallNextHookEx(IntPtr pHookHandle, int nCode, Int32 wParam, IntPtr lParam);

            /// <summary>
            /// 获取关联进程的主模块
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            [DllImport("kernel32.dll")]
            private static extern IntPtr GetModuleHandle(string name);

            /// <summary>
            /// 转换当前按键信息
            /// </summary>
            /// <param name="uVirtKey"></param>
            /// <param name="uScanCode"></param>
            /// <param name="lpbKeyState"></param>
            /// <param name="lpwTransKey"></param>
            /// <param name="fuState"></param>
            /// <returns></returns>
            [DllImport("user32.dll")]
            private static extern int ToAscii(UInt32 uVirtKey, UInt32 uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, UInt32 fuState);

            /// <summary>
            /// 获取按键状态
            /// </summary>
            /// <param name="pbKeyState"></param>
            /// <returns>非0表示成功</returns>
            [DllImport("user32.dll")]
            private static extern int GetKeyboardState(byte[] pbKeyState);

            [DllImport("user32.dll")]
            private static extern short GetKeyStates(int vKey);

            /// <summary>
            /// 获取当前鼠标位置
            /// </summary>
            /// <param name="lpPoint"></param>
            /// <returns></returns>
            [DllImport("user32.dll")]
            private extern static int GetCursorPos(ref POINT lpPoint);


            #endregion DLL导入

            /// <summary>
            /// 安装钩子
            /// </summary>
            /// <returns></returns>
            public bool InstallHook()
            {
                // 假如没有安装键盘钩子
                if (this.KeyBoaradHookValue == IntPtr.Zero)
                {
                    IntPtr pInstance = GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);
                    this.m_KeyboardHookProcedure = new HookProc(KeyboardHookProc);
                    this.KeyBoaradHookValue = SetWindowsHookEx(WH_Codes.WH_KEYBOARD_LL, this.m_KeyboardHookProcedure, pInstance, 0);
                    if (this.KeyBoaradHookValue == IntPtr.Zero)
                    {
                        this.UnInstallHook();
                        return false;
                    }
                }

                return true;
            }
            /// <summary>
            /// 卸载钩子
            /// </summary>
            /// <returns></returns>
            public bool UnInstallHook()
            {
                bool result = true;

                if (this.KeyBoaradHookValue != null)
                {
                    if (this.KeyBoaradHookValue != IntPtr.Zero)
                    {
                        result = (UnhookWindowsHookEx(this.KeyBoaradHookValue) && result);
                        this.KeyBoaradHookValue = IntPtr.Zero;
                    }
                }

                return result;
            }
            //钩子事件内部调用,调用_clientMethod方法转发到客户端应用。 
            private int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
            {
                bool handled = false;

                //it was ok and someone listens to events
                if ((nCode >= 0) && (this.KeyDown != null || this.KeyUp != null || this.KeyPress != null))
                {
                    //read structure KeyboardHookStruct at lParam
                    KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));

                    //检测按键被按下
                    if (this.KeyDown != null && (wParam == (int)WM_KEYBOARD.WM_KEYDOWN || wParam == (int)WM_KEYBOARD.WM_SYSKEYDOWN))
                    {
                        Keys keyData = (Keys)MyKeyboardHookStruct.VKCode;
                        KeyEventArgs e = new KeyEventArgs(keyData);
                        this.KeyDown(this, e);
                        handled = handled || e.Handled;

                    }

                    //检测按键被按下中
                    if (this.KeyPress != null && wParam == (int)WM_KEYBOARD.WM_KEYDOWN)
                    {
                        bool isDownShift, isDownCapslock;
                        try
                        {
                            isDownShift = ((GetKeyStates(VK_SHIFT) & 0x80) == 0x80 ? true : false);
                            isDownCapslock = (GetKeyStates(VK_CAPITAL) != 0 ? true : false);
                        }
                        catch
                        {
                            isDownCapslock = false;
                            isDownShift = false;
                        }

                        byte[] keyState = new byte[256];
                        GetKeyboardState(keyState);
                        byte[] inBuffer = new byte[2];
                        if (ToAscii(MyKeyboardHookStruct.VKCode, MyKeyboardHookStruct.ScanCode, keyState, inBuffer, MyKeyboardHookStruct.Flags) == 1)
                        {
                            char key = (char)inBuffer[0];
                            if ((isDownCapslock ^ isDownShift) && Char.IsLetter(key)) key = Char.ToUpper(key);
                            KeyPressEventArgs e = new KeyPressEventArgs(key);
                            this.KeyPress(this, e);
                            handled = handled || e.Handled;

                        }
                    }
                    //检测按键被抬起
                    if (this.KeyUp != null && (wParam == (int)WM_KEYBOARD.WM_KEYUP || wParam == (int)WM_KEYBOARD.WM_SYSKEYUP))
                    {
                        Keys keyData = (Keys)MyKeyboardHookStruct.VKCode;
                        KeyEventArgs e = new KeyEventArgs(keyData);
                        this.KeyUp(this, e);
                        handled = handled || e.Handled;

                    }
                    if (HandleKey != null)
                    {
                        KeyboardHookStruct hookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));//转换结构 
                        bool handled1;
                        HandleKey(hookStruct, out handled1);
                        handled = handled || handled1;
                    }
                }
                if (handled == true)
                {
                    return 1;
                }
                else
                {
                    return CallNextHookEx(this.KeyBoaradHookValue, nCode, wParam, lParam);
                }
            }
        }

    }
}

