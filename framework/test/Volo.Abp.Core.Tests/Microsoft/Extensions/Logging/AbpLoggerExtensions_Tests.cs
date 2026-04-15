using System;
using System.Collections.Generic;
using System.Globalization;
using Shouldly;
using Xunit;

namespace Microsoft.Extensions.Logging;

public class AbpLoggerExtensions_Tests
{
    [Fact]
    public void LogException_Should_Format_String_Data()
    {
        var logger = new FakeLogger();
        var exception = new Exception("test");
        exception.Data["Name"] = "John";

        logger.LogException(exception);

        logger.LastLoggedMessage.ShouldContain("Name = John");
    }

    [Fact]
    public void LogException_Should_Format_Primitive_Data()
    {
        var logger = new FakeLogger();
        var exception = new Exception("test");
        exception.Data["Count"] = 42;
        exception.Data["IsActive"] = true;

        logger.LogException(exception);

        logger.LastLoggedMessage.ShouldContain("Count = 42");
        logger.LastLoggedMessage.ShouldContain("IsActive = True");
    }

    [Fact]
    public void LogException_Should_Format_Complex_Object_As_Json()
    {
        var logger = new FakeLogger();
        var exception = new Exception("test");
        exception.Data["Details"] = new Dictionary<string, object>
        {
            { "RuleName", "FixedWindow" },
            { "Limit", 10 }
        };

        logger.LogException(exception);

        logger.LastLoggedMessage.ShouldContain("\"RuleName\":\"FixedWindow\"");
        logger.LastLoggedMessage.ShouldContain("\"Limit\":10");
    }

    [Fact]
    public void LogException_Should_Format_List_Of_Complex_Objects_As_Json()
    {
        var logger = new FakeLogger();
        var exception = new Exception("test");
        exception.Data["RuleDetails"] = new List<Dictionary<string, object>>
        {
            new() { { "RuleName", "FixedWindow" }, { "Limit", 10 } },
            new() { { "RuleName", "SlidingWindow" }, { "Limit", 20 } }
        };

        logger.LogException(exception);

        var message = logger.LastLoggedMessage;
        message.ShouldNotContain("System.Collections.Generic.List");
        message.ShouldContain("FixedWindow");
        message.ShouldContain("SlidingWindow");
    }

    [Fact]
    public void LogException_Should_Format_Null_Data_As_Empty()
    {
        var logger = new FakeLogger();
        var exception = new Exception("test");
        exception.Data["NullKey"] = null;

        logger.LogException(exception);

        logger.LastLoggedMessage.ShouldContain("NullKey = ");
    }

    [Fact]
    public void LogException_Should_Format_Enum_Data()
    {
        var logger = new FakeLogger();
        var exception = new Exception("test");
        exception.Data["Level"] = LogLevel.Warning;

        logger.LogException(exception);

        logger.LastLoggedMessage.ShouldContain("Level = Warning");
    }

    [Fact]
    public void LogException_Should_Format_DateTime_Data()
    {
        var logger = new FakeLogger();
        var exception = new Exception("test");
        var now = new DateTime(2025, 1, 15, 10, 30, 0);
        exception.Data["Timestamp"] = now;

        logger.LogException(exception);

        logger.LastLoggedMessage.ShouldContain($"Timestamp = {now.ToString(CultureInfo.CurrentCulture)}");
    }

    [Fact]
    public void LogException_Should_Format_Guid_Data()
    {
        var logger = new FakeLogger();
        var exception = new Exception("test");
        var id = Guid.NewGuid();
        exception.Data["Id"] = id;

        logger.LogException(exception);

        logger.LastLoggedMessage.ShouldContain($"Id = {id}");
    }

    [Fact]
    public void LogException_Should_Format_Anonymous_Object_As_Json()
    {
        var logger = new FakeLogger();
        var exception = new Exception("test");
        exception.Data["Info"] = new { Name = "Test", Value = 123 };

        logger.LogException(exception);

        var message = logger.LastLoggedMessage;
        message.ShouldContain("\"Name\":\"Test\"");
        message.ShouldContain("\"Value\":123");
    }

    [Fact]
    public void LogException_Should_Fallback_To_ToString_For_Non_Serializable_Object()
    {
        var logger = new FakeLogger();
        var exception = new Exception("test");
        var selfRef = new SelfReferencingObject { Name = "Loop" };
        selfRef.Self = selfRef;
        exception.Data["BadObject"] = selfRef;

        logger.LogException(exception);

        logger.LastLoggedMessage.ShouldNotBeNull();
        logger.LastLoggedMessage.ShouldContain("BadObject = SelfRef:Loop");
        logger.LastLoggedMessage.ShouldNotContain("\"Name\"");
    }

    [Fact]
    public void LogException_Should_Truncate_Large_Json_Output()
    {
        var logger = new FakeLogger();
        var exception = new Exception("test");
        var largeList = new List<Dictionary<string, string>>();
        for (var i = 0; i < 500; i++)
        {
            largeList.Add(new Dictionary<string, string>
            {
                { "Key", new string('x', 100) }
            });
        }
        exception.Data["LargeData"] = largeList;

        logger.LogException(exception);

        logger.LastLoggedMessage.ShouldNotBeNull();
        logger.LastLoggedMessage.ShouldContain("...(truncated)");
    }

    private class SelfReferencingObject
    {
        public string Name { get; set; } = default!;
        public override string ToString() => $"SelfRef:{Name}";
        public SelfReferencingObject? Self { get; set; }
    }

    private class FakeLogger : ILogger
    {
        public string? LastLoggedMessage { get; private set; }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            LastLoggedMessage = formatter(state, exception);
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state) where TState : notnull => NullDisposable.Instance;

        private class NullDisposable : IDisposable
        {
            public static readonly NullDisposable Instance = new();
            public void Dispose() { }
        }
    }
}
