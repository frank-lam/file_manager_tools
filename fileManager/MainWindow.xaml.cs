using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace fileManager
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void selectDir_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog m_Dialog = new FolderBrowserDialog();
            DialogResult result = m_Dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            string m_Dir = m_Dialog.SelectedPath.Trim();
            dirPath.Text = m_Dir;


        }



        //获取文件夹-包括子文件夹
        public void ListFiles(FileSystemInfo info, List<string> result)
        {
            //String[] result = new String[9999];
            if (!info.Exists) return;
            DirectoryInfo dir = info as DirectoryInfo;
            //不是目录   
            if (dir == null) return;
            FileSystemInfo[] files = dir.GetFileSystemInfos();
            for (int i = 0, j = 0; i < files.Length; i++)
            {
                FileInfo file = files[i] as FileInfo;
                //是文件   
                if (file != null)
                {
                    //Console.WriteLine(file.FullName + "\t " + file.Length);
                    //MessageBox.Show(file.FullName);
                    //result[file_count] = file.FullName;
                    //file_count++;
                    result.Add(file.FullName);
                    //j++;
                }

                //对于子目录，进行递归调用   
                else
                    ListFiles(files[i], result);
            }
            return;
        }



        public String getFileNameByPath(String path)
        {
            string filename = System.IO.Path.GetFileName(path);
            return filename;
        }
        

        private void generateDirInfo_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.MessageBox.Show(GetMediaTimeLenSecond(@"E:\学习课程系列\thinkphp5开发restful-api接口\01 课程简介.mp4").ToString());

            return;
            //String frank = @"D:\a\abby.jpg";
            //String to = getFileNameByPath(frank);
            //dirTextInfo.Text = to;

            //return;


            String path = dirPath.Text;
            //String path = @"D:\frank";

            string[] dataFiles = new string[] { }; //不定长 
            List<string> list = new List<string>();
            if (true)  //包含子目录
            {
                FileSystemInfo info = new DirectoryInfo(path);
                ListFiles(info, list);
                dataFiles = list.ToArray();
            }
            else //不包含子目录
            {
                dataFiles = Directory.GetFiles(path);   // 获取当前文件目录 下的所有文件信息
            }

            String dirinfo = "";
            String rootinfo = "";
            String fatherFolderName = "";
            foreach (String fileName in dataFiles)
            {
                String realName = getFileNameByPath(fileName);
                String pathStr = "";
                pathStr = fileName.Replace(path + @"\", "");

                if (!pathStr.Contains(@"\"))
                {
                    String folderName = pathStr.Replace(@"\" + realName, "");

                    folderName = "root";
                    if (fatherFolderName != "" && folderName == fatherFolderName)
                    {
                        rootinfo += "\t" + realName + "\n";
                    }
                    else
                    {
                        rootinfo += folderName + "\n";
                        rootinfo += "\t" + realName + "\n";
                        fatherFolderName = folderName;
                    }


                }
                else
                {
                    String folderName = pathStr.Replace(@"\"+realName, "");

                    if (fatherFolderName != "" && folderName == fatherFolderName)
                    {
                        dirinfo +="\t" + realName + "\n";
                    }
                    else
                    {
                        dirinfo += folderName + "\n";
                        dirinfo += "\t" + realName + "\n";
                        fatherFolderName = folderName;
                    }
                }

                
            }

            dirTextInfo.Text = rootinfo + dirinfo;


        }


        protected String Director(string dir)
        {
            String dirinfo = "";
            DirectoryInfo d = new DirectoryInfo(dir);
            FileSystemInfo[] fsinfos = d.GetFileSystemInfos();
            foreach (FileSystemInfo fsinfo in fsinfos)
            {
                if (fsinfo is DirectoryInfo)     //判断是否为文件夹  
                {
                    Director(fsinfo.FullName);//递归调用  
                }
                else
                {
                    dirinfo += fsinfo.FullName;
                }
            }

            return dirinfo;

        }

        public static String ListFiles(FileSystemInfo info)
        {
            String dirinfo = "";

            if (!info.Exists) return "";

            DirectoryInfo dir = info as DirectoryInfo;
            //不是目录 
            if (dir == null) return "";

            FileSystemInfo[] files = dir.GetFileSystemInfos();
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i] as FileInfo;
                //是文件 
                if (file != null){
                    dirinfo += file.FullName + "\t " + file.Length;
                    Console.WriteLine(file.FullName + "\t " + file.Length);
                }
                    
                    
                //对于子目录，进行递归调用 
                else
                    ListFiles(files[i]);

            }

            return dirinfo;
        }


        public static string GetMediaTimeLen(string path)
        {
            try
            {
                Shell32.Shell shell = new Shell32.Shell();
                //文件路径               
                Shell32.Folder folder = shell.NameSpace(path.Substring(0, path.LastIndexOf("\\")));
                //文件名称             
                Shell32.FolderItem folderitem = folder.ParseName(path.Substring(path.LastIndexOf("\\") + 2));
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    return folder.GetDetailsOf(folderitem, 27);
                }
                else
                {
                    return folder.GetDetailsOf(folderitem, 21);
                }
            }
            catch (Exception ex) { return null; }
        }

        public static int GetMediaTimeLenSecond(string path)
        {
            try
            {
                Shell32.Shell shell = new Shell32.Shell();
                //文件路径               
                Shell32.Folder folder = shell.NameSpace(path.Substring(0, path.LastIndexOf("\\")));
                //文件名称             
                Shell32.FolderItem folderitem = folder.ParseName(path.Substring(path.LastIndexOf("\\") + 2));
                string len;
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    len = folder.GetDetailsOf(folderitem, 27);
                }
                else
                {
                    len = folder.GetDetailsOf(folderitem, 21);
                }

                string[] str = len.Split(new char[] { ':' });
                int sum = 0;
                sum = int.Parse(str[0]) * 60 * 60 + int.Parse(str[1]) * 60 + int.Parse(str[2]);

                return sum;
            }
            catch (Exception ex) { return 0; }
        }
 

    }
}
