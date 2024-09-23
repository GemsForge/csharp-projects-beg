namespace TaskTrackerConsole.model
{
    /// <summary>
    /// Represents the status of a task.
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// The task is yet to be started.
        /// </summary>
        TODO,

        /// <summary>
        /// The task is currently in progress.
        /// </summary>
        PENDING,

        /// <summary>
        /// The task is completed.
        /// </summary>
        COMPLETE
    }
}
