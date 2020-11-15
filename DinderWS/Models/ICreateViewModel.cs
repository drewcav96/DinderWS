namespace DinderWS.Models {
    /// <summary>
    /// Interface for View Models that create a new contextual entity.
    /// </summary>
    /// <typeparam name="TModel">The contextual entity type.</typeparam>
    public interface ICreateViewModel<TModel>
            where TModel : class, new() {
        /// <summary>
        /// Creates a new contextual entity <typeparamref name="TModel"/> from the view model.
        /// </summary>
        /// <returns>The new <typeparamref name="TModel"/> contextual entity.</returns>
        TModel Create();
    }
}
