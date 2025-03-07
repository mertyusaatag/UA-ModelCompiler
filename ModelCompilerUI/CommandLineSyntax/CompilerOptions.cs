﻿//__________________________________________________________________________________________________
//
//  Copyright (C) 2022, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GitHub: https://github.com/mpostol/OPC-UA-OOI/discussions
//__________________________________________________________________________________________________

using CommandLine;
using OOI.ModelCompiler;
using System;
using System.IO;

namespace OOI.ModelCompilerUI.CommandLineSyntax
{
  [Verb("compile", false, HelpText = compileHelp)]
  internal class CompilerOptions : OptionsBase, ICompilerOptions
  {
    #region ICompilerOptions

    [Option('g', OptionsNames.GenerateIdentifierFile, SetName = "csv", HelpText = cgHelp, MetaValue = "CSVfileGenrate", Required = false)]
    public bool CreateIdentifierFile { get; set; }

    [Option('i', OptionsNames.StartId, HelpText = idHelp, Required = false, Default = (uint)0)]
    public uint StartId { get; set; }

    [Option('s', OptionsNames.UseAllowSubtypes, HelpText = useAllowSubtypesHelp, Required = false)]
    public bool UseAllowSubtypes { get; set; }

    [Option('m', OptionsNames.ModelVersion, HelpText = mvHelp, Required = false)]
    public string ModelVersion { get; set; }

    [Option(OptionsNames.ModelPublicationDate, HelpText = pdHelp, Required = false)]
    public string ModelPublicationDate { get; set; }

    [Option(OptionsNames.ReleaseCandidate, HelpText = rcHelp, Required = false)]
    public bool ReleaseCandidate { get; set; }

    #endregion ICompilerOptions

    internal void ValidateOptionsConsistency()

    {
      if (string.IsNullOrEmpty(OutputPath))
        throw new ArgumentOutOfRangeException(nameof(OutputPath), "Parameters cannot be null or empty");
      if ((DesignFiles == null) || (DesignFiles.Count == 0))
        throw new ArgumentException("No design file specified.");
      foreach (string path in DesignFiles)
      {
        if (String.IsNullOrEmpty(path))
          throw new ArgumentOutOfRangeException(nameof(DesignFiles), "The design file path is null or empty.");
        if (!File.Exists(path))
          throw new ArgumentOutOfRangeException(nameof(DesignFiles), $"The design file does not exist: {path}");
      }
      if (String.IsNullOrEmpty(IdentifierFile))
        throw new ArgumentOutOfRangeException(nameof(IdentifierFile), "The identifier file path is null or empty.");
      if (!File.Exists(IdentifierFile))
      {
        if (!CreateIdentifierFile)
          throw new ArgumentOutOfRangeException(nameof(IdentifierFile), $"The required identifier file does not exist: {IdentifierFile}");
        File.Create(IdentifierFile).Close();
      }
    }

    public override string ToString()
    {
      return Parser.Default.FormatCommandLine<CompilerOptions>(this);
    }

    #region private

    private const string compileHelp = "Takes an OPC UA ModelDesign file and generates a NodeSet and code for the .NETStandard stack.";
    private const string cgHelp = "Creates the identifier file if it does not exist (used instead of the -c option).";
    private const string idHelp = "The first identifier to use when assigning new ids to nodes.";
    private const string useAllowSubtypesHelp = "When subtypes are allowed for a field, C# code with the class name from the model is created instead of ExtensionObject. No effect when subtypes are not allowed.";
    private const string mvHelp = "The version of the model to produce.";
    private const string pdHelp = "The publication date of the model to produce.";
    private const string rcHelp = "Indicates that a release candidate nodeset is being generated.";

    private static class OptionsNames
    {
      public const string GenerateIdentifierFile = "cg";
      public const string UseAllowSubtypes = "useAllowSubtypes";
      public const string StartId = "id";
      public const string ModelVersion = "mv";
      public const string ModelPublicationDate = "pd";
      public const string ReleaseCandidate = "rc";
    }

    #endregion private
  }
}