using SadConsole;
using SadRogue.Primitives;

namespace ResidentSurvivor {
    public class MassageScreen : SadConsole.Console {
        private List<string> messages = new List<string>();

        public MassageScreen(int width, int height) : base(width, height){
            this.DefaultBackground = Color.AnsiRed;
            this.Position = new Point(1,31);
        }

        public void Add(string message){
            messages.Add(message);
            if (messages.Count > 5){
                messages.RemoveAt(0);
            }
            this.Clear();
            for (int i = 0; i < messages.Count; i++){
                this.Print(1,i,messages[i]);
            }
        }
    }
}