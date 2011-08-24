using System;
using Microsoft.SPOT;

namespace Hellevator.Behavior.Interface
{
    public static class Playlists
    {
        public static class Accept
        {
            private const string Folder = "accept\\";

            public const string TravelExterior = Folder + "travel-exterior.mp3";
            public const string DestintationInterior = Folder + "dest\\";
        }

        public static class Idle
        {
            private const string Folder = "idle\\";
            public const string DestinationExterior = Folder + "dest-exterior.mp3";
        }

        public static class Heaven
        {
            private const string Folder = "heaven\\";

            public const string DestinationInterior = Folder + "dest-interior.mp3";
            public const string GotoInterior = Folder + "goto-interior.mp3";
            public const string GotoExterior = Folder + "goto-exterior.mp3";
            public const string ExitInterior = Folder + "exit-interior.mp3";
            public const string ExitExterior = Folder + "exit-exterior.mp3";
        }

        public static class Purgatory
        {
            private const string Folder = "purg\\";

            public const string DestinationInterior = Folder + "dest-interior.mp3";
            public const string GotoInterior = Folder + "goto-interior.mp3";
            public const string GotoExterior = Folder + "goto-exterior.mp3";
            public const string ExitInterior = Folder + "exit-interior.mp3";
            public const string ExitExterior = Folder + "exit-exterior.mp3";
        }

        public static class Hell
        {
            private const string Folder = "hell\\";

            public const string DestinationInterior = Folder + "dest-interior.mp3";
            public const string GotoInterior = Folder + "goto-interior.mp3";
            public const string GotoExterior = Folder + "goto-exterior.mp3";
            public const string ExitInterior = Folder + "exit-interior.mp3";
            public const string ExitExterior = Folder + "exit-exterior.mp3";
        }
    }
}
