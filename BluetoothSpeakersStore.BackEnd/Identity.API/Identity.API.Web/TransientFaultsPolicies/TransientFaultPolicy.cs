using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Wrap;

namespace Identity.API.Web.TransientFaultsPolicies
{
    public class TransientFaultPolicy
    {
        private CircuitBreakerPolicy messageBusCircuitBreakerPolicy;
        private RetryPolicy messageBusRetryProcedurePolicy;
        private PolicyWrap messageBusCombinedPolicies;

        public CircuitBreakerPolicy MessageBusCircuitBreakerPolicy
        {
            get => messageBusCircuitBreakerPolicy;
            private set => messageBusCircuitBreakerPolicy = value;
        }
        public RetryPolicy MessageBusRetryProcedurePolicy
        {
            get => messageBusRetryProcedurePolicy;
            private set => messageBusRetryProcedurePolicy = value;
        }
        public PolicyWrap MessageBusCombinedPolicies 
        { 
            get => messageBusCombinedPolicies; 
            private set => messageBusCombinedPolicies = value; 
        }

        public TransientFaultPolicy()
        {
            MessageBusCircuitBreakerPolicy = Policy
                .Handle<Exception>()
                    .CircuitBreaker(3, TimeSpan.FromMinutes(5)); //circuit breaker

            MessageBusRetryProcedurePolicy = Policy
                .Handle<Exception>()
                    .WaitAndRetry(3, retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(3, retryAttempt))); //exponential backoff

            MessageBusCombinedPolicies = Policy.Wrap(MessageBusCircuitBreakerPolicy,MessageBusRetryProcedurePolicy); 
            //policies combined execution, starts execution from the right
        }
    }
}
