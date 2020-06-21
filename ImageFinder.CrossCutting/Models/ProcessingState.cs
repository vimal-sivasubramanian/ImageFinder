namespace ImageFinder.CrossCutting.Models
{
    public class ProcessingState
    {
        public bool IsProcessing { get; set; }

        public static ProcessingState Started => new ProcessingState { IsProcessing = true };

        public static ProcessingState Completed => new ProcessingState { IsProcessing = false };
    }
}
