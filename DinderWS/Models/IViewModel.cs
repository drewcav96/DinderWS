namespace DinderWS.Models {
    /// <summary>
    /// Interface for the view model.
    /// </summary>
    public interface IViewModel {
        /// <summary>
        /// The Title or main header for this view.
        /// </summary>
        string ViewTitle { get; }
        /// <summary>
        /// The error message from the server, if any.
        /// </summary>
        string Error { get; }
    }
}
