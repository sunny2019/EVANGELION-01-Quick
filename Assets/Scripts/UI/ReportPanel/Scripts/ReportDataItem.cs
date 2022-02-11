namespace Game.UI
{
    using Sirenix.OdinInspector;
    using TMPro;

    public class ReportDataItem : SerializedMonoBehaviour
    {
        public TextMeshProUGUI name;
        public TextMeshProUGUI Score;
        public TextMeshProUGUI Time;
        public TextMeshProUGUI detailed;
        
        public ReportDataItem Assignment(string name, string score, string time, string detailed)
        {
            this.name.text = name;
            this.Score.text = score;
            this.Time.text = time;
            this.detailed.text = detailed;
            return this;
        }
    }
}