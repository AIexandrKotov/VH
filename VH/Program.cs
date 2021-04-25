using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VH
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "VersionHelper";
            if (args.Length < 1)
            {
                Console.WriteLine(
@"
Подробности: https://github.com/AIexandrKotov/VH

Для использования этого приложения, запускайте через командную строку с минимум одним аргументом следующего формата:
> <path> ...
В качестве <path> укажите путь к .cs файлу, следующему шаблону VHFileTemplate.cs

Следующие аргументы могут быть представлены в любом порядке:
 - revision++ / build++ / minor++ / major++ 
 Увеличивает значение компоненты версии на 1
 - revision=<value> / build=<value> / minor=<value> / major=<value>
 Присваивает значение <value> компоненте версии

Пример команды для .bat-файла: vh.exe ""bin\VHFileTemplate.cs"" ""revision++""

Для выхода нажмите любую клавишу
"
);
                Console.ReadKey(true);
                return;
            }
            if (args.Length < 1) return;
            else
            {
                var vh = new VHFile(args[0]);
                for (var i = 1; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "revision++":
                            vh.Revision++;
                            break;
                        case "build++":
                            vh.Build++;
                            break;
                        case "minor++":
                            vh.Minor++;
                            break;
                        case "major++":
                            vh.Major++;
                            break;
                        default:
                            {
                                if (args[i].StartsWith("revision=")) vh.Revision = int.Parse(args[i].Substring(9));
                                if (args[i].StartsWith("build=")) vh.Build = int.Parse(args[i].Substring(6));
                                if (args[i].StartsWith("minor=")) vh.Minor = int.Parse(args[i].Substring(6));
                                if (args[i].StartsWith("major=")) vh.Major = int.Parse(args[i].Substring(6));
                            }
                            break;
                    }
                        
                }
                vh.Save();
            }
        }
    }
}
