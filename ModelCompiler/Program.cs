/* ========================================================================
 * Copyright (c) 2005-2021 The OPC Foundation, Inc. All rights reserved.
 *
 * OPC Foundation MIT License 1.00
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * The complete license agreement can be found here:
 * http://opcfoundation.org/License/MIT/1.00/
 * ======================================================================*/

using Opc.Ua;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ModelCompiler
{
  public static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    public static void Main(bool noGui)
    {
      try
      {
        if (!noGui)
        {
          Application.EnableVisualStyles();
          Application.SetCompatibleTextRenderingDefault(false);
        }

        if (MeasurementUnits.ProcessCommandLine())
        {
          return;
        }

        ServiceMessageContext context = ServiceMessageContext.GlobalContext;

        if (!ProcessCommandLine(noGui))
        {
          StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ModelCompiler.HelpFile.txt"));
          if (noGui)
          {
            System.Console.Error.WriteLine(reader.ReadToEnd());
          }
          else
          {
            MessageBox.Show(reader.ReadToEnd(), "ModelCompiler");
          }
          reader.Close();
          Environment.Exit(2);
        }
      }
      catch (Exception e)
      {
        if (noGui)
        {
          System.Console.Error.WriteLine(e.Message);
          System.Console.Error.WriteLine(e.StackTrace);
          Environment.Exit(3);
        }
        else
        {
          Application.Run(new ExceptionDlg(e));
        }
      }
    }

    /// <summary>
    /// Extracts the tokens from the command line.
    /// </summary>
    private static List<string> GetTokens(string commandLine)
    {
      List<string> tokens = new List<string>();

      bool quotedToken = false;
      StringBuilder token = new StringBuilder();

      for (int ii = 0; ii < commandLine.Length; ii++)
      {
        char ch = commandLine[ii];

        if (quotedToken)
        {
          if (ch == '"')
          {
            if (token.Length > 0)
            {
              tokens.Add(token.ToString());
              token = new StringBuilder();
            }

            quotedToken = false;
            continue;
          }

          token.Append(ch);
        }
        else
        {
          if (token.Length == 0)
          {
            if (ch == '"')
            {
              quotedToken = true;
              continue;
            }
          }

          if (Char.IsWhiteSpace(ch))
          {
            if (token.Length > 0)
            {
              tokens.Add(token.ToString());
              token = new StringBuilder();
            }

            continue;
          }

          token.Append(ch);
        }
      }

      if (token.Length > 0)
      {
        tokens.Add(token.ToString());
      }

      return tokens;
    }

    /// <summary>
    /// Processes the command line arguments.
    /// </summary>
    private static bool ProcessCommandLine(bool noGui)
    {
      string commandLine = Environment.CommandLine;

      if (commandLine.IndexOf("-?") != -1)
      {
        StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ModelCompiler.HelpFile.txt"));
        MessageBox.Show(reader.ReadToEnd(), "ModelCompiler");
        reader.Close();
        return true;
      }

      List<string> tokens = GetTokens(commandLine);

      // launch gui if no arguments provided.
      if (tokens.Count == 1)
      {
        return false;
      }

      return ProcessCommandLine2(tokens, noGui);
    }

    /// <summary>
    /// Processes the command line arguments.
    /// </summary>
    private static bool ProcessCommandLine2(List<string> tokens, bool noGui)
    {
      try
      {
        List<string> designFiles = new List<string>();
        string identifierFile = null;
        string outputDir = null;
        bool generateIds = false;
        uint startId = 1;
        string stackRootDir = null;
        string ansicRootDir = null;
        bool generateMultiFile = false;
        bool useXmlInitializers = false;
        string[] excludeCategories = null;
        bool includeDisplayNames = false;
        bool useAllowSubtypes = false;

        bool updateHeaders = false;
        string inputDirectory = ".";
        string filePattern = "*.xml";
        string specificationVersion = "";
        var licenseType = HeaderUpdateTool.LicenseType.MITXML;
        bool silent = false;

        for (int ii = 1; ii < tokens.Count; ii++)
        {
          if (tokens[ii] == "-input")
          {
            if (ii >= tokens.Count - 1)
            {
              throw new ArgumentException("Incorrect number of parameters specified with the -input option.");
            }

            inputDirectory = tokens[++ii];
            continue;
          }

          if (tokens[ii] == "-pattern")
          {
            if (ii >= tokens.Count - 1)
            {
              throw new ArgumentException("Incorrect number of parameters specified with the -pattern option.");
            }

            filePattern = tokens[++ii];
            continue;
          }

          if (tokens[ii] == "-silent")
          {
            silent = true;
            continue;
          }

          if (tokens[ii] == "-license")
          {
            if (ii >= tokens.Count - 1)
            {
              throw new ArgumentException("Incorrect number of parameters specified with the -license option.");
            }

            updateHeaders = true;
            licenseType = (HeaderUpdateTool.LicenseType)Enum.Parse(typeof(HeaderUpdateTool.LicenseType), tokens[++ii]);
            continue;
          }

          if (tokens[ii] == "-d2")
          {
            if (ii >= tokens.Count - 1)
            {
              throw new ArgumentException("Incorrect number of parameters specified with the -d2 option.");
            }

            designFiles.Add(tokens[++ii]);
            continue;
          }

          if (tokens[ii] == "-d2")
          {
            if (ii >= tokens.Count - 1)
            {
              throw new ArgumentException("Incorrect number of parameters specified with the -d2 option.");
            }

            designFiles.Add(tokens[++ii]);
            continue;
          }

          if (tokens[ii] == "-d2")
          {
            if (ii >= tokens.Count - 1)
            {
              throw new ArgumentException("Incorrect number of parameters specified with the -d2 option.");
            }

            designFiles.Add(tokens[++ii]);
            continue;
          }

          if (tokens[ii] == "-c" || tokens[ii] == "-cg")
          {
            if (ii >= tokens.Count - 1)
            {
              throw new ArgumentException("Incorrect number of parameters specified with the -c or -cg option.");
            }

            generateIds = tokens[ii] == "-cg";
            identifierFile = tokens[++ii];
            continue;
          }

          if (tokens[ii] == "-o")
          {
            if (ii >= tokens.Count - 1)
            {
              throw new ArgumentException("Incorrect number of parameters specified with the -o option.");
            }

            outputDir = tokens[++ii];
            continue;
          }

          if (tokens[ii] == "-o2")
          {
            if (ii >= tokens.Count - 1)
            {
              throw new ArgumentException("Incorrect number of parameters specified with the -o option.");
            }

            outputDir = tokens[++ii];
            generateMultiFile = true;
            continue;
          }

          if (tokens[ii] == "-id")
          {
            startId = Convert.ToUInt32(tokens[++ii]);
            continue;
          }

          if (tokens[ii] == "-version")
          {
            if (ii >= tokens.Count - 1)
            {
              throw new ArgumentException("Incorrect number of parameters specified with the -version option.");
            }

            specificationVersion = tokens[++ii];
            continue;
          }

          if (tokens[ii] == "-ansic")
          {
            if (ii >= tokens.Count - 1)
            {
              throw new ArgumentException("Incorrect number of parameters specified with the -ansic option.");
            }

            ansicRootDir = tokens[++ii];
            continue;
          }

          if (tokens[ii] == "-useXmlInitializers")
          {
            useXmlInitializers = true;
            continue;
          }

          if (tokens[ii] == "-includeDisplayNames")
          {
            includeDisplayNames = true;
            continue;
          }

          if (tokens[ii] == "-stack")
          {
            if (ii >= tokens.Count - 1)
            {
              throw new ArgumentException("Incorrect number of parameters specified with the -stack option.");
            }

            stackRootDir = tokens[++ii];
            continue;
          }

          if (tokens[ii] == "-exclude")
          {
            if (ii >= tokens.Count - 1)
            {
              throw new ArgumentException("Incorrect number of parameters specified with the -exclude option.");
            }

            excludeCategories = tokens[++ii].Split(',', '+');
            continue;
          }

          if (tokens[ii] == "-useAllowSubtypes")
          {
            useAllowSubtypes = true;
            continue;
          }
        }

        if (updateHeaders)
        {
          HeaderUpdateTool.ProcessDirectory(inputDirectory, filePattern, licenseType, silent);
          return true;
        }

        ModelGenerator2 generator = new ModelGenerator2();

        for (int ii = 0; ii < designFiles.Count; ii++)
        {
          if (String.IsNullOrEmpty(designFiles[ii]))
          {
            throw new ArgumentException("No design file specified.");
          }

          if (!new FileInfo(designFiles[ii]).Exists)
          {
            throw new ArgumentException("The design file does not exist: " + designFiles[ii]);
          }
        }

        if (String.IsNullOrEmpty(identifierFile))
        {
          throw new ArgumentException("No identifier file specified.");
        }

        if (!new FileInfo(identifierFile).Exists)
        {
          if (!generateIds)
          {
            throw new ArgumentException("The identifier file does not exist: " + identifierFile);
          }

          File.Create(identifierFile).Close();
        }

        generator.ValidateAndUpdateIds(
            designFiles,
            identifierFile,
            startId,
            specificationVersion,
            useAllowSubtypes);

        if (!String.IsNullOrEmpty(stackRootDir))
        {
          if (!new DirectoryInfo(stackRootDir).Exists)
          {
            throw new ArgumentException("The directory does not exist: " + stackRootDir);
          }

          StackGenerator.GenerateDotNet(stackRootDir, specificationVersion);
        }

        if (!String.IsNullOrEmpty(ansicRootDir))
        {
          if (!new DirectoryInfo(ansicRootDir).Exists)
          {
            throw new ArgumentException("The directory does not exist: " + ansicRootDir);
          }

          StackGenerator.GenerateAnsiC(ansicRootDir, specificationVersion);
          generator.GenerateIdentifiersAndNamesForAnsiC(ansicRootDir, excludeCategories);
        }

        if (!String.IsNullOrEmpty(outputDir))
        {
          if (generateMultiFile)
          {
            generator.GenerateMultipleFiles(outputDir, useXmlInitializers, excludeCategories, includeDisplayNames);
          }
          else
          {
            generator.GenerateInternalSingleFile(outputDir, useXmlInitializers, excludeCategories, includeDisplayNames);
          }
        }
      }
      catch (Exception e)
      {
        if (! noGui)
        {
          System.Console.Error.WriteLine(e.Message);
          System.Console.Error.WriteLine(e.StackTrace);
          Environment.Exit(4);
        }
        else
        {
          new ExceptionDlg(e).ShowDialog();
        }

        return true;
      }

      return true;
    }
  }
}