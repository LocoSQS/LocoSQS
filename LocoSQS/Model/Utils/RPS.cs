using static System.Net.Mime.MediaTypeNames;

namespace LocoSQS.Model.Utils
{
    public class RPS
    {
        public static string? Check(string data)
        {
            List<string> types = new() { "rock", "paper", "scissors" };
            if (types.Contains(data))
            {
                int responseIdx = Random.Shared.Next(0, 3);
                int playerIdx = types.IndexOf(data);
                string result = (((playerIdx + 1) % 3) == responseIdx) ? "I win" : (playerIdx == responseIdx ? "We draw" : "I lose");
                return $"I pick {types[responseIdx]}. {result}";
            }

            return null;
        }
    }
}
