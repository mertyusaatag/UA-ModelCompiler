﻿//__________________________________________________________________________________________________
//
//  Copyright (C) 2021, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GitHub: https://github.com/mpostol/OPC-UA-OOI/discussions
//__________________________________________________________________________________________________

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace OOI.ModelCompilerUI
{
  [TestClass]
  public class ModelCompilerAPIInternalUnitTest
  {
    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void ConstructorTest()
    {
      ModelCompilerAPIInternal instance = new ModelCompilerAPIInternal();
      instance.ProcessCommandLine(null);
    }
    [TestMethod]
    public void StackGenerationTest()
    {
      ModelCompilerAPIInternal instance = new ModelCompilerAPIInternal();
      List<string> commandLine = new List<string>()
      {
        "-d2", @".\ModelCompiler\Design.v104\StandardTypes.xml",
        "-version",  "v104",
        "-d2",  @".\ModelCompiler\Design.v104\UA Core Services.xml",
        "-c", @".\ModelCompiler\CSVs\StandardTypes.csv",
        "-o2",  @".\Bin\nodesets\master\Schema\",
        "-stack",  @".\Bin\nodesets\master\DotNet\",
        "-ansic", @".\Bin\nodesets\master\AnsiC\"
      };
      instance.ProcessCommandLine(commandLine);
    }
  }
}