﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace release_assist
{
  class Program
  {
    static void Main(string[] args)
    {
      string root = "../../";
      try
      {
        var files = Directory.GetFiles(root + "/Final Build/");

        for (int i = 0; i < files.Length; i++)
          if (files[i].Contains(".pdb") || files[i].Contains(".config"))
            File.Delete(files[i]);

        files = Directory.GetFiles(root + "/build/deps/");

        for (int i = 0; i < files.Length; i++)
          if (files[i].Contains(".iobj") || files[i].EndsWith("pdb"))
            File.Delete(files[i]);

        Directory.Move(root + "/build/deps/", root + "/Final Build/deps");
        Directory.Delete(root + "/build/", true);
      }
      finally
      {
        Process.GetProcessesByName("release-assist")[0].Kill();
      }
    }
  }
}
