using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task19_CustomExceptionFilter : ICodingTask
{
    public int Id => 19;
    public string Title => "Custom exception + exception filters (when)";
    public string Category => "Exceptions";

    public string Description => """
        The retry logic catches every exception, then re-inspects it with an
        if/throw to decide whether the failure is retryable — which unwinds and
        re-throws, losing the original stack for non-retryable cases.

        Define a proper custom exception that carries a status code, and use an
        exception filter so only retryable failures enter the catch block;
        everything else propagates untouched.
        """;

    public string StarterCode => """
        try {
            CallService();
        }
        catch (Exception ex) {                 // catches everything
            var http = ex as HttpRequestEx;
            if (http is null || http.StatusCode != 429)
                throw;                         // rethrow loses original site
            Retry();
        }
        """;

    public string SolutionExplanation => """
        A custom exception should derive from Exception, provide the standard
        constructors, and expose typed data (StatusCode). An exception *filter*
        — `catch (T ex) when (predicate)` — decides whether to enter the handler
        *without unwinding the stack*. If the predicate is false the catch is
        skipped entirely and the exception keeps travelling with its original
        stack trace intact, so you never re-throw to "put it back". Retryable
        (429) is handled; anything else propagates naturally.
        """;

    public string SolutionCode => """
        public sealed class ServiceException : Exception {
            public int StatusCode { get; }
            public ServiceException(int statusCode, string message)
                : base(message) => StatusCode = statusCode;
        }

        try {
            CallService();
        }
        catch (ServiceException ex) when (ex.StatusCode == 429) {
            Retry();                  // only transient failures land here
        }
        // 500s, other types: never caught, propagate with original stack
        """;
}
