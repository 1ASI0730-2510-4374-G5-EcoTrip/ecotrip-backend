namespace Experience.Domain.Enum
{
    /// <summary>
    /// Represents the status of an Experience in the system
    /// </summary>
    public enum ExperienceStatus
    {
        /// <summary>
        /// The experience is in draft mode and not yet visible to customers
        /// </summary>
        Draft = 0,

        /// <summary>
        /// The experience is published and available for booking
        /// </summary>
        Published = 1,

        /// <summary>
        /// The experience has been cancelled by the agent or administrator
        /// </summary>
        Cancelled = 2,

        /// <summary>
        /// The experience date has passed and it has been completed
        /// </summary>
        Completed = 3
    }
}