using System;
using System.Collections.Generic;
using System.Linq;

namespace JetBrains.FeaturedImageGenerator.Models
{
    public static class Products
    {
        public static readonly List<string> All = new()
        {
            BigDataTools, CodeWithMe, CLion, DotMemory, GoLand,
            IntelliJ, Kotlin, PyCharm, ReSharper,
            ResharperCPlusPlus, Rider, RubyMine,
            TeamCity, WebStorm
        };

        public const string BigDataTools = "Big Data Tools";
        public const string CodeWithMe = "Code With Me";
        public const string CLion = nameof(CLion);
        public const string DotMemory = nameof(DotMemory);
        public const string GoLand = nameof(GoLand);
        public const string IntelliJ = nameof(IntelliJ);
        public const string Kotlin = nameof(Kotlin);
        public const string PyCharm = nameof(PyCharm);
        public const string ReSharper = nameof(ReSharper);
        public const string ResharperCPlusPlus = "ReSharper C++";
        public const string Rider = nameof(Rider);
        public const string RubyMine = nameof(RubyMine);
        public const string TeamCity = nameof(TeamCity);
        public const string WebStorm = nameof(WebStorm);

        public static bool Contains(string productName)
        {
            return All.Contains(productName, StringComparer.OrdinalIgnoreCase);
        }
    }
}