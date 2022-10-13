﻿namespace Skalm
{
    internal class Settings : ISettings
    {
        public string GameTitle { get; private set; } = "";
        public bool DisplayCursor { get; private set; }
        public int UpdateFrequency { get; private set; }

        public int WindowPadding { get; private set; }
        public int BorderThickness { get; private set; }
        public int CellWidth { get; private set; }
        public int CellHeight { get; private set; }
        public int MapWidth { get; private set; }
        public int MapHeight { get; private set; }
        public int MessageBoxHeight { get; private set; }
        public int StatsWidth { get; private set; }
        public int MainStatsHeight { get; private set; }
        public int SubStatsHeight { get; private set; }
        public int HudPadding { get; private set; }

        public char SpriteBorder { get; private set; }
        public char SpriteWall { get; private set; }
        public char SpriteFloor { get; private set; }
        public char SpriteDoor { get; private set; }

        public string SoundsFolderPath { get; private set; } = "";

        public ConsoleColor foregroundColor { get; private set; }

        public ConsoleColor backgroundColor { get; private set; }

        public virtual bool LoadSettings(string[] settingsFile)
        {
            return ApplySettings(settingsFile);
        }


        private bool ApplySettings(string[] settingsFile)
        {
            foreach (var line in settingsFile)
            {
                if (line.Trim() == "")
                    continue;

                string type = line.Split(' ')[0];
                if (type == "#")
                    continue;

                string name = line.Split(' ')[1];
                string value = line.Split('=').Last().Trim();

                if (this.GetType().GetProperty(name) is null)
                    return false;

                try
                {
                    switch (type)
                    {
                        case "Int32":
                            this.GetType().GetProperty(name)!.SetValue(this, ParseInt(value));
                            break;
                        case "Char":
                            this.GetType().GetProperty(name)!.SetValue(this, ParseChar(value));
                            break;
                        case "String":
                            this.GetType().GetProperty(name)!.SetValue(this, value);
                            break;
                        case "Boolean":
                            this.GetType().GetProperty(name)!.SetValue(this, ParseBool(value));
                            break;
                        case "ConsoleColor":
                            this.GetType().GetProperty(name)!.SetValue(this, ParseColor(value));
                            break;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

     

        private char ParseChar(string field)
        {
            return field.ToCharArray().SingleOrDefault();
        }

        private int ParseInt(string field)
        {
            int.TryParse(field, out int value);
            return value;
        }

        private bool ParseBool(string field)
        {
            bool.TryParse(field, out bool value);
            return value;
        }

        private ConsoleColor ParseColor(string field)
        {
            ConsoleColor color = ConsoleColor.Gray;
            switch (field)
            {
                case "White":
                    color = ConsoleColor.White;
                    break;
                case "Black":
                    color = ConsoleColor.Black;
                    break;
                case "Red":
                    color = ConsoleColor.Red;
                    break;
            }
            return color;
        }
    }
}
