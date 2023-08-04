namespace LocalizationKeySynchronizer;

public static class Questions
{
    public const string _2 = "Enter the absolute path to the exported file:";

    public const string _3 = "Enter the default language path:";

    public const string _6 = "Enter the absolute path to export the keys that do not match the number of arguments:";

    public const string _7 = "Enter the absolute path to export the missing keys:";

    public const string _8 = "Enter the export path:";

    public const string _9 = "Enter the localization folder path:";

    public const string _10 = "Enter the old key:";

    public const string _11 = "Enter the new key:";

    public static class _1
    {
        public const string Question =
            "Do you want to find asynchronous keys, apply changes in the exported file or replace the keys?";

        public static class Options
        {
            public const string Find = "Find asynchronous keys";
            public const string Apply = "Apply changes in the exported file";
            public const string Replace = "Replace keys";
        }
    }

    public static class _4
    {
        public const string Question =
            "Find keys that do not match the number of arguments, find missing keys, or both?";

        public static class Options
        {
            public const string ArgumentsCount = "Not matching arguments count";
            public const string MissingKeys = "Missing keys";
        }
    }

    public static class _5
    {
        public const string Question =
            "Should the keys that do not match the number of arguments be deleted, exported or both?";

        public static class Options
        {
            public const string Delete = "Delete";
            public const string Export = "Export";
        }
    }
}