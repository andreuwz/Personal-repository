using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Wrap;

namespace Cart.API.Web.TransientFaultsPolicies
{
    public class TransientFaultPolicy
    {
        private CircuitBreakerPolicy messageBusCircuitBreakerPolicy;
        private RetryPolicy messageBusRetryProcedurePolicy;
        private PolicyWrap messageBusCombinedPolicies;

        internal CircuitBreakerPolicy MessageBusCircuitBreakerPolicy
        {
            get => messageBusCircuitBreakerPolicy;
            private set => messageBusCircuitBreakerPolicy = value;
        }
        internal RetryPolicy MessageBusRetryProcedurePolicy
        {
            get => messageBusRetryProcedurePolicy;
            private set => messageBusRetryProcedurePolicy = value;
        }
        internal PolicyWrap MessageBusCombinedPolicies
        {
            get => messageBusCombinedPolicies;
            private set => messageBusCombinedPolicies = value;
        }

        public TransientFaultPolicy()
        {
            MessageBusCircuitBreakerPolicy = Policy
                .Handle<Exception>()
                    .CircuitBreaker(3, TimeSpan.FromMinutes(5));

            MessageBusRetryProcedurePolicy = Policy
                .Handle<Exception>()
                    .WaitAndRetry(3, retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(3, retryAttempt)));

            MessageBusCombinedPolicies = Policy.Wrap(MessageBusCircuitBreakerPolicy, MessageBusRetryProcedurePolicy);
        }
    }
}
