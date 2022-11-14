using System.Text.Json;

namespace Identity.API.Common.Constants
{
    public static class AppConstants
    {
        public const string userNotFound = "User not found!";
        public const string duplicateCredentials = "User with such username or email already exists!";
        public const string duplicateUsername = "User with such username already exists!";
        public const string duplicateEmail = "User with such email already exists!";
        public const string unauthorizedAdminEdit = "Admin accounts can be edited only by their owner!";
        public const string successfulLogin = "You logged in successfully!";
        public const string successfulLogout = "You logged out successfully!";
        public const string successfulUserDelete = "User deleted successfully!";
        public const string successfulUserEdit = "User edited successfully! You've been logged out.";
        public const string successfulAdminUserEdit = "User edited successfully!";
        public const string successfulUserCreate = "User created successfully!";
        public const string cannotChangeAdminAccount = "Cannot delete or edit admin account!";
        public const string userInRole = "The user already is in this role!";
        public const string userIsAssigned = "The user was assigned to the role!";
        public const string userNotInRole = "The user was not administrator before!";
        public const string userIsUnassigned = "The user is not adminstrator, anymore.";
        public const string notLoggedIn = "You are not logged in!";
        public const string registerComplete = "Successful registration! You are automatically logged in!";
        public const string sessionLoggedIn = "You are currently logged in! Please, logout first!";
        public const string rabbitMQConnected = "--> Connected to RabbitMQ Message Bus";
        public const string rabbitMQNotConnected = "--> Could not connect to the Message Bus:";
        public const string rabbitMQShutDown = "--> RabbitMQ connection SHUTDOWN";
        public const string rabbitMQDisposed = "--> MessageBus DISPOSED";
        public const string rabbitMQSentMessage = "--> RabbitMQ Message SENT";
        public const string rabbitMQNotSentMessage = "--> Rabbit MQ connection closed, not sending...";
        public const string eventTypePublishLoggedUser = "PublishLoggedUser";
        public const string evenTypeLoggedOut = "LoggedOut";
        public const string undeterminedEventType = "UndeturminedEvent";
        public const string determiningEvent = "--> Determining event...";
        public const string eventTypeUpdatedUserBalance = "UpdatedBalance";
        public const string updatedUserBalance = "--> New information about user balance was published and retrieved";
        public const string recievedEvent = "--> Event recieved !";
        public const string eventTypeUpdatedUserInfo = "UpdatedUserInfo";
        public const string eventTypeUserDelete = "UserDelete";
        public const string infrastructureFailure = "One ore more internal API components are not working properly!";
        public const string circuitBreakerOpenMessage = "Please, wait for 5 minutes, before trying again.";

        public static string RabbitMQTrySentMessage(int tryAttempts)
        {
            return $"--> RabbitMQ Initialize sending message ({tryAttempts})";
        }

        public static string RabbitMQInfrastructureError(int tryAttempts)
        {
            return $"--> System Communication Malfunction. Retry attempts: {tryAttempts}. " +
                $"Please, contact your Administrator immediately. Your information is not stored properly in the system.";
        }

        public static string SerializeSingleMessage(string message)
        {
            return JsonSerializer.Serialize(message);
        }

        public static string SerializeMultipleMessages(IEnumerable<string> messages)
        {
            return JsonSerializer.Serialize(messages);
        }
    }
}
