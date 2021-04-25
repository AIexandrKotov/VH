using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VH
{
    public class VHFile
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; }
        public int Revision { get; set; }
        public DateTime LastUpdate { get; private set; }

        string path;
        string[] Lines;
        int majorid, minorid, buildid, revisionid, lastupdateid;

        void Find()
        {
            majorid = Array.FindIndex(Lines, x => x.Trim().StartsWith("public const string Major") && x.EndsWith("//vh"));
            minorid = Array.FindIndex(Lines, x => x.Trim().StartsWith("public const string Minor") && x.EndsWith("//vh"));
            buildid = Array.FindIndex(Lines, x => x.Trim().StartsWith("public const string Build") && x.EndsWith("//vh"));
            revisionid = Array.FindIndex(Lines, x => x.Trim().StartsWith("public const string Revision") && x.EndsWith("//vh"));
            lastupdateid = Array.FindIndex(Lines, x => x.Trim().StartsWith("public const long LastUpdate") && x.EndsWith("//vh"));
            if (majorid == -1 || minorid == -1 || buildid == -1 || revisionid == -1 || lastupdateid == -1) throw new Exception("vh: one of the constants was not detected");
            Major = int.Parse(GetValue(Lines[majorid], "Major"));
            Minor = int.Parse(GetValue(Lines[minorid], "Minor"));
            Build = int.Parse(GetValue(Lines[buildid], "Build"));
            Revision = int.Parse(GetValue(Lines[revisionid], "Revision"));
            LastUpdate = new DateTime(long.Parse(GetValue(Lines[lastupdateid], "LastUpdate", false)));
        }

        static string GetValue(string s, string name, bool remq = true)
        {
            var ret = s.Substring(s.IndexOf(name) + 3 + name.Length, s.IndexOf(';') - s.IndexOf(name) - name.Length - 3);
            if (remq) return ret.Replace("\"", "");
            else return ret;
        }

        public VHFile(string path)
        {
            this.path = path;
            Lines = File.ReadAllLines(path);
            Find();
        }

        public void Save()
        {
            Lines[majorid] = Lines[majorid].Substring(0, Lines[majorid].IndexOf("Major")) + $@"Major = ""{Major}""; //vh";
            Lines[minorid] = Lines[minorid].Substring(0, Lines[minorid].IndexOf("Minor")) + $@"Minor = ""{Minor}""; //vh";
            Lines[buildid] = Lines[buildid].Substring(0, Lines[buildid].IndexOf("Build")) + $@"Build = ""{Build}""; //vh";
            Lines[revisionid] = Lines[revisionid].Substring(0, Lines[revisionid].IndexOf("Revision")) + $@"Revision = ""{Revision}""; //vh";
            Lines[lastupdateid] = Lines[lastupdateid].Substring(0, Lines[lastupdateid].IndexOf("LastUpdate")) + $@"LastUpdate = {DateTime.UtcNow.Ticks}; //vh";
            File.WriteAllLines(path, Lines);
        }
    }
}
