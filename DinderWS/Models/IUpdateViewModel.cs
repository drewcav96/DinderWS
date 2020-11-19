namespace DinderWS.Models {
    /// <summary>
    /// Interface for View Models that update an existing contextual entity.
    /// </summary>
    /// <typeparam name="TModel">The contextual entity type.</typeparam>
    public interface IUpdateViewModel<TModel>
            where TModel : class {
        /// <summary>
        /// Updates an existing contextual entity <see cref="TModel"/> from the view model.
        /// </summary>
        /// <param name="model">The existing <typeparamref name="TModel"/> contextual entity.</param>
        void Update(TModel model);
    }
}
