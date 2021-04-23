using System.Collections.Generic;

namespace OracleScraper
{
    public class PathFile
    {
        internal string Path { get; set; }
        internal string SettingUpload { get; set; }



        internal static List<PathFile> GetList ()
        {
            var result = new List<PathFile>
            {
                new PathFile { Path = "Акт", SettingUpload = "ACT         " },
                new PathFile { Path = "АСКУЭ", SettingUpload = "ASKUE       " },
                new PathFile { Path = "ИСУ", SettingUpload = "ISU         " },
                new PathFile { Path = "Клиент", SettingUpload = "ABONENT     " },
                new PathFile { Path = "Контроллер", SettingUpload = "KONTR       " },
                new PathFile { Path = "Платежный", SettingUpload = "PLAT-AGENT  " },
                new PathFile { Path = "Принципал", SettingUpload = "AGENT       " },
                new PathFile { Path = "Старший", SettingUpload = "VOLONTER    " },
                new PathFile { Path = "ТСО", SettingUpload = "TSO         " },
                new PathFile { Path = "Управляющая", SettingUpload = "UPR-KOMP    " }
            };

            return result;
        }
    }
}