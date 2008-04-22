using System;

namespace Game465P3
{
    static class Program
    {
        static void Main(string[] args)
        {
            bool profile = false;
            foreach (string s in args)
            {
                if (s.Equals("-profile"))
                    profile = true;
            }
            using (MyGame game = new MyGame(profile))
            {
                game.Run();
            }
        }
    }
}

