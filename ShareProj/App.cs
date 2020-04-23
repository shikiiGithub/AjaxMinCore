
using dotNetLab.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AjaxMinMsBuildTask.MSBuildTasks
{
   public class App
    {

        public static void  Main(string[] args)
        {
            BundleMinifier minifier = new BundleMinifier();

            String dir = Path.GetDirectoryName(typeof(App).Assembly.Location);
            

            dir = Path.GetDirectoryName(dir)+"/wwwroot";

             
            
            String JSCSSSrcPath = Path.GetFullPath(dir);
            String  TargetPath  = Path.GetDirectoryName(typeof(App).Assembly.Location) + $"/Debug/{args[0]}/wwwroot";

            Console.WriteLine("开始缩减js/css任务:");
            minifier.SearchCSSAndJS(JSCSSSrcPath, TargetPath);
           Console.WriteLine("缩减js/css任务已经完成");

           


        }
    }
}
