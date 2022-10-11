﻿using System.Media;

namespace Skalm.Sounds
{
    internal class SoundManager
    {
        public ISoundPlayer player;
        public List<Sound> Tracks { get; private set; }
        public SoundManager(ISoundPlayer soundPlayer)
        {
            player = soundPlayer;
            Tracks = CreateSoundsList(Globals.G_SOUNDS_FOLDER_PATH);

            Random random = new Random();
            player.Play(Tracks[random.Next(Tracks.Count)]);
        }

        /// <summary>
        /// Creates a list of Sounds to play.
        /// </summary>
        /// <param name="path">Path to sounds folder.</param>
        /// <returns></returns>
        private List<Sound> CreateSoundsList(string path)
        {
            List<string> fileNames = LoadFileNamesFromFolder(path);
            List<Sound> sounds = new List<Sound>();
            foreach (string fileName in fileNames)
            {
                string soundName = fileName.Replace('_', ' ').Split('.').First();
                sounds.Add(new Sound(soundName, fileName));
            }
            return sounds;
        }

        /// <summary>
        /// Loads .wav files from Sounds folder on startup and places their file names into a List.
        /// </summary>
        /// <param name="path">Path to sounds folder.</param>
        /// <returns></returns>
        private List<string> LoadFileNamesFromFolder(string path)
        {
            string[] files = Directory.GetFiles(path, "*.wav");
            List<string> fileNames = new();
            foreach (string file in files)
                fileNames.Add(Path.GetFileName(file));
            return fileNames;
        }

        /// <summary>
        /// The type of SFX to be played.
        /// </summary>
        public enum SoundType
        {
            Move,
            Confirm
        }
    }
}