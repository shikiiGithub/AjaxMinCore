using dotNetLab.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
 

namespace dotNetLab.Web
{
   public class BundleMinifier
    {
        private string[] js, css;

        
               Microsoft.Ajax.Utilities.Minifier minifier = new Microsoft.Ajax.Utilities.Minifier();
        public BundleMinifier()
        {
        

        }
        String GetSavedFilePath(String sourceDirPath, string filePath, String saveRootPath)
        {
            sourceDirPath = Path.GetFullPath(sourceDirPath);
            saveRootPath = Path.GetFullPath(saveRootPath);
            filePath = Path.GetFullPath(filePath);
            string fileName = Path.GetFileName(filePath);
         
            String temp = Path.GetDirectoryName(filePath);
           

            string outputFile = null;
            if (temp.Equals(sourceDirPath)) //如果位于根目录
            {
                if (!Directory.Exists(saveRootPath))
                    Directory.CreateDirectory(saveRootPath);

                outputFile = Path.Combine(saveRootPath, fileName);
            }
            else //不位于根目录
            {
                temp = temp.Replace(sourceDirPath+"/", "");
                String dirPath = Path.Combine(saveRootPath, temp);
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                outputFile = Path.Combine(dirPath, fileName);

            }
            return outputFile;
        }

        void MinifyCSSAsync(String sourceDirPath,string filePath,String saveRootPath   )
        {
             

            string outputFile = GetSavedFilePath(  sourceDirPath,   filePath,   saveRootPath);



            // Gets the StreamReader to get the file contents
            using (StreamReader cssReader = File.OpenText(filePath))
            {
                // Save the contents to a string
                string css =  cssReader.ReadToEnd ();



                // Compresses the CSS
                css = minifier.MinifyStyleSheet(css);


                // Creates a StreamWriter to write the contents to the new file
                using (StreamWriter cssWriter = File.CreateText(outputFile))
                {
                    // Writes the minified CSS to the <filename without extension>.min.css
                    cssWriter.Write(css);

                    // Flushes the written contents
                    cssWriter.Flush ();

                    // Closes the stream
                    cssWriter.Close();
                }
                // Using automatically disposes of it.

                // Closes the reader stream
                cssReader.Close();
            }
         

             
        }

        void MinifyJSAsync(String sourceDirPath, string filePath, String saveRootPath)
        { 
          
            string outputFile = GetSavedFilePath(sourceDirPath, filePath, saveRootPath);
            // Gets the StreamReader to get the file contents
            using (StreamReader jsReader = File.OpenText(filePath))
            {
                // Save the contents to a string
                string js =   jsReader.ReadToEnd ();
                // Compresses the Js
                try
                {
                     js = minifier.MinifyJavaScript(js);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error "+ex.Message+" "+ex.StackTrace);
                }
 
                // Creates a StreamWriter to write the contents to the new file
                using (StreamWriter jsWriter = File.CreateText(outputFile))
                {
                    // Writes the minified CSS to the <filename without extension>.min.js
                    jsWriter.Write(js);

                    // Flushes the written contents
                     jsWriter.Flush ();

                    // Closes the stream
                    jsWriter.Close();
                }
                // Using automatically disposes of it.

                // Closes the reader stream
                jsReader.Close();
            }
            // Using automatically disposes of it

           
           

           
        }
        public void SearchCSSAndJS(string path,String saveRootFolder)
        {
            FileSystem.CopyDirectory(path, saveRootFolder);
            js = Directory.GetFiles(path, "*.js", SearchOption.AllDirectories);
            css = Directory.GetFiles(path, "*.css", SearchOption.AllDirectories);
            foreach (string file in js)
            {
                MinifyJSAsync(path, file, saveRootFolder);
            }

            foreach (string file in css)
            {
                MinifyCSSAsync(path, file, saveRootFolder);

            }
            String runtimedirPath = null;
              
                 
               runtimedirPath = Path.GetDirectoryName(saveRootFolder) +"/runtimes";
             

            if (Directory.Exists(runtimedirPath))
            {
                string [] dirs = Directory.GetDirectories(runtimedirPath);
                dirs.ToList().ForEach(x => { bool b = Path.GetFileName(x).StartsWith("win");if (b) FileSystem.DeleteDir(x); });
            }

        }

        
    }
}
