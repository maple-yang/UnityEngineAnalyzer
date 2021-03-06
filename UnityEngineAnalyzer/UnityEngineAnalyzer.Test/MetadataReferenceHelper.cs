﻿using System;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.IO;
using System.Linq;


namespace UnityEngineAnalyzer.Test
{
    static class MetadataReferenceHelper
    {
        public static readonly ImmutableList<MetadataReference> UsingUnityEngine =
            ImmutableList.Create(GetUnityMetadataReference(), GetSystem(), GetSystemCore());

        private static MetadataReference GetUnityMetadataReference()
        {
            const string unityEngineFilePath = @"Editor\Data\Managed\UnityEngine.dll";
            var programFilesFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            var unityDirectories =
                Directory.EnumerateDirectories(programFilesFolderPath, "*Unity*", SearchOption.TopDirectoryOnly);

            foreach (var unityDirectory in unityDirectories)
            {
                var unityEngineFullPath = Path.Combine(unityDirectory, unityEngineFilePath);

                if (File.Exists(unityEngineFullPath))
                {
                    return MetadataReference.CreateFromFile(unityEngineFullPath);
                }
            }

            throw new FileNotFoundException("Unable to locate UnityEngine.dll");
        }

        private static MetadataReference GetSystem()
        {
            var assemblyPath = typeof(object).Assembly.Location;
            return MetadataReference.CreateFromFile(assemblyPath);
        }

        private static MetadataReference GetSystemCore()
        {
            var assemblyPath = typeof(Enumerable).Assembly.Location;
            return MetadataReference.CreateFromFile(assemblyPath);
        }
    }
}
